using DurableTask.Core.History;
using Microsoft.Azure.Amqp;
using Microsoft.Extensions.Logging;
using ParcelCommon;
using ParcelCommon.Interfaces;
using ParcelCommon.Models;
using ParcelTrack.Interfaces;
using ParcelTrack.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParcelTrack.Services
{
    public class EventConsumerSvc : IEventConsumerSvc
    {
        ICDBManager _cDBManager;
        ILogger<EventConsumerSvc> _logger;
        IValidateScan _validateScan;
        IAlertSender _alertSender;

        public EventConsumerSvc(ILogger<EventConsumerSvc> iLogger, ICDBManager cDBManager, IValidateScan validateScan, IAlertSender alertSender)
        {
            _cDBManager = cDBManager;
            _logger = iLogger;
            _validateScan = validateScan;
            _alertSender = alertSender;
        }
        
        async Task<bool> IEventConsumerSvc.ProcessEventData(dynamic eventData)
        {
            if (eventData == null) return false;

            var eventTypeData = ((IDictionary<string, object>)eventData)["EventType"].ToString();
            
            if (eventTypeData.Equals(ParcelEventTypes.COLLECTED.ToString()))
            {
                return await ProcessNewBooking(eventData);
            }
            else
            {
                return await ProcessNewScan(eventData);
            }
        }

        private async Task<bool> ProcessNewBooking(dynamic bookingRequest)
        {

            dynamic senderData = JsonSerializer.Deserialize<ExpandoObject>(bookingRequest.SenderData);
            dynamic receiverData = JsonSerializer.Deserialize<ExpandoObject>(bookingRequest.ReceiverData);
            dynamic parcelData = JsonSerializer.Deserialize<ExpandoObject>(bookingRequest.ParcelData);
            dynamic sourceAddress = JsonSerializer.Deserialize<ExpandoObject>(parcelData.SourceAddress);
            dynamic destinationAddress = JsonSerializer.Deserialize<ExpandoObject>(parcelData.DestinationAddress);

            var dictSenderData = (IDictionary<string, object>)senderData;
            var dictReceiverData = (IDictionary<string, object>)receiverData;
            var dictParcelData = (IDictionary<string, object>)parcelData;
            var dictSourceAddress = (IDictionary<string, object>)sourceAddress;
            var dictDestinationAddress = (IDictionary<string, object>)destinationAddress;

            var resultBookingValidation = await _validateScan.ValidateBookingAsync(dictParcelData["TrackingID"].ToString()!);

            if(resultBookingValidation)
            {
                Parcel objParcel = new()
                {
                    ParcelId = Guid.NewGuid().ToString().Replace("-", ""),
                    TrackingID = dictParcelData["TrackingID"].ToString()!,
                    ParcelDimension = new ParcelDimension { Width = Convert.ToInt32(dictParcelData["Width"].ToString()), Height = Convert.ToInt32(dictParcelData["Height"].ToString()), Length = Convert.ToInt32(dictParcelData["Length"].ToString()), Weight = Convert.ToInt32(dictParcelData["Weight"].ToString()) },
                    SenderName = dictSenderData["Name"].ToString()!,
                    SenderPhone = dictSenderData["Phone"].ToString()!,
                    SourceAddress = new AddressDto { Address1 = dictSourceAddress["Address1"].ToString()!, Address2 = dictSourceAddress["Address2"]?.ToString(), City = dictSourceAddress["City"]?.ToString()!, Country = dictSourceAddress["Country"]?.ToString()!, PostalCode = Convert.ToInt32(dictSourceAddress["PostalCode"].ToString())!, Region = dictSourceAddress["Region"]?.ToString()! },
                    DestinationAddess = new AddressDto { Address1 = dictDestinationAddress["Address1"].ToString()!, Address2 = dictDestinationAddress["Address2"]?.ToString(), City = dictDestinationAddress["City"]?.ToString()!, Country = dictDestinationAddress["Country"]?.ToString()!, PostalCode = Convert.ToInt32(dictDestinationAddress["PostalCode"].ToString())!, Region = dictDestinationAddress["Region"]?.ToString()! },
                    ReceiverName = dictReceiverData["Name"].ToString()!,
                    ReceiverPhone = dictReceiverData["Phone"].ToString()!
                };
                objParcel.ShippingCost = _validateScan.CalculateShippingCost (objParcel.ParcelDimension.Width.Value, objParcel.ParcelDimension.Height.Value, objParcel.ParcelDimension.Length.Value, objParcel.ParcelDimension.Weight.Value);
                var dictBookingReq = (IDictionary<string, object>)bookingRequest;
                objParcel.ReceiverAlert = bool.Parse(dictBookingReq["ReceiverNotification"].ToString()!);

                ParcelEvent parcelEvent = new ParcelEvent
                {
                    TrackingId = objParcel.TrackingID,
                    ActorId = dictBookingReq["ActorId"].ToString()!,
                    ActorType = dictBookingReq["ActorType"].ToString()!,
                    LocationId = dictBookingReq["LocationId"].ToString()!,
                    HubType = dictBookingReq["HubType"].ToString()!,
                    EventType = dictBookingReq["EventType"].ToString()!
                };

                try
                {
                    var result = await _cDBManager.CreateNewParcel(objParcel);
                    if (!result) return false;
                    var eventResult = await _cDBManager.CreateEvent(parcelEvent);
                    if (!eventResult) return false;
                    _alertSender.SendMobileUpdate(objParcel.SenderPhone);
                    if(objParcel.ReceiverAlert) _alertSender.SendMobileUpdate(objParcel.ReceiverPhone);
                    return eventResult;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    if (ex.InnerException != null) _logger.LogError(ex.InnerException.ToString());
                    return false;
                }
            }
            return false;
        }

        private async Task<bool> ProcessNewScan(dynamic scanRequest)
        {
            var dictScanReq = (IDictionary<string, object>)scanRequest;

            ParcelEvent parcelEvent = new ParcelEvent
            {
                TrackingId = dictScanReq["TrackingId"].ToString()!,
                ActorId = dictScanReq["ActorId"].ToString()!,
                ActorType = dictScanReq["ActorType"].ToString()!,
                LocationId = dictScanReq["LocationId"].ToString()!,
                HubType = dictScanReq["HubType"].ToString()!,
                EventType = dictScanReq["EventType"].ToString()!
            };

            var validateEvtData = await _validateScan.ValidateScanEventAsync(parcelEvent.TrackingId, parcelEvent.EventType, parcelEvent.LocationId);
            if (validateEvtData)
            {
                try
                {
                    var result = await _cDBManager.CreateEvent(parcelEvent);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    if (ex.InnerException != null) _logger.LogError(ex.InnerException?.ToString());
                    return false;
                }
            }

            return false;
        }
    }
}

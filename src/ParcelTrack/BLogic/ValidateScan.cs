using Microsoft.Extensions.Logging;
using ParcelCommon.Interfaces;
using ParcelCommon.Models;
using ParcelTrack.Interfaces;
using ParcelTrack.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelTrack.BLogic
{
    public class ValidateScan : IValidateScan
    {
        ICDBManager _dbManager;
        ILogger<ValidateScan> _logger;

        public ValidateScan(ICDBManager dbManager, ILogger<ValidateScan> logger)
        {
            _dbManager = dbManager;
            _logger = logger;
        }

        decimal IValidateScan.CalculateShippingCost(int Width, int Height, int Length, int Weight)
        {
            bool isLargeParcel = (Width > ShippingPrices.PriceSlab) ||
                (Height > ShippingPrices.PriceSlab) ||
                (Length > ShippingPrices.PriceSlab) ||
                (Weight > ShippingPrices.PriceSlab);

            if (isLargeParcel) return ShippingPrices.InitialCost + (ShippingPrices.PriceSlab * ShippingPrices.ExtraSlabCost);
            else return ShippingPrices.InitialCost;

        }

        async Task<bool> IValidateScan.ValidateBookingAsync(string TrackingID)
        {
            var parcel = await _dbManager.GetParcelDetail(TrackingID);

            if (parcel == null) { return true; } //No parcel data found
            return false;
        }

        async Task<bool> IValidateScan.ValidateScanEventAsync(string TrackingID, string EventTypeCode, string LocId)
        {
            var scanEvent = await _dbManager.GetParcelEvent(TrackingID, EventTypeCode, LocId);

            //Get last event from all events for a tracking id to check event type is not moving backward
            var allEvents = await _dbManager.GetAllOrderedEventsByTrackingId(TrackingID);
            var lastEvent = allEvents.FirstOrDefault<ParcelEvent>();
            
            int eventType = (int)((ParcelEventTypes)Enum.Parse(typeof(ParcelEventTypes), EventTypeCode));
            int oldEventType = (int)((ParcelEventTypes)Enum.Parse(typeof(ParcelEventTypes), lastEvent.EventType));

            if (scanEvent == null && eventType > oldEventType) return true; //No scan event with same tracking ID, Status, Location ID, and event type is not like same (review?)

            return false;
        }
    }
}

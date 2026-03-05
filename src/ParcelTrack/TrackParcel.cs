using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ParcelCommon.Interfaces;
using ParcelTrack.DTOs;

namespace ParcelTrack;

public class TrackParcel
{
    private readonly ILogger<TrackParcel> _logger;
    private readonly ICDBManager _dbManager;

    public TrackParcel(ILogger<TrackParcel> logger, ICDBManager cDBManager)
    {
        _logger = logger;
        _dbManager = cDBManager;
    }

    [Function("TrackParcel")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "trackparcel/{trackingId:long}")] HttpRequest req, long trackingId)
    {
        //_logger.LogInformation("C# HTTP trigger function processed a request.");
        var parcel = await _dbManager.GetParcelDetail(trackingId.ToString());
        var parcelEvents = await _dbManager.GetAllOrderedEventsByTrackingId(trackingId.ToString());
        if (parcel != null && parcelEvents.Count > 0)
        {
            var trackingDetails = new ParcelStatusDto
            {
                TrackingId = parcel.TrackingID,
                SourceCity = parcel.SourceAddress.City,
                SourceZipcode = parcel.SourceAddress.PostalCode.ToString(),
                DestinationCity = parcel.DestinationAddess.City,
                DestinationZipcode = parcel.DestinationAddess.PostalCode.ToString(),
                Status = parcelEvents.FirstOrDefault().EventType,
                TrackingDetails = parcelEvents.Select(events => new ParcelStatusTrackDto
                {
                    StatusDate = events.TimeStamp.ToShortDateString(),
                    StatusLoc = events.LocationId,
                    StatusName = events.EventType
                }).ToList()
            };

            return new OkObjectResult(trackingDetails);
        }
        return new BadRequestObjectResult("Tracking Information Not Found!");
    }
}
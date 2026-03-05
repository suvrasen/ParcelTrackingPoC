using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ParcelCommon.Interfaces;
using ParcelCommon.Utilities;
using ParcelTrack.DTOs;
using System.Dynamic;

namespace ParcelTrack
{
    public class BookingStatus
    {
        private readonly ILogger<BookingStatus> _logger;
        private ICDBManager _cDBManager;

        public BookingStatus(ILogger<BookingStatus> logger, ICDBManager cDBManager)
        {
            _logger = logger;
            _cDBManager = cDBManager;
        }

        [Function("BookingStatus")]
        public async Task<IActionResult> GetBookingStatus([HttpTrigger(AuthorizationLevel.Anonymous, "get" , Route = "bookingstatus/{trackingId:long}")] HttpRequestData req, long trackingId)
        {
            var parcel = await _cDBManager.GetParcelDetail(trackingId.ToString());
            if (parcel != null)
            {
                var statusResponse = new BookingStatusDto
                {
                    ParcelId = parcel.ParcelId,
                    ShipmentConfirmed = true,
                    ShipmentCost = parcel.ShippingCost.ToString()
                };
                return new OkObjectResult(statusResponse);
            }
            //_logger.LogInformation("C# HTTP trigger function processed a request.");
            return new BadRequestObjectResult("Parcel details not found");
        }
    }
}

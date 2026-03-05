using Microsoft.AspNetCore.Mvc;
using ParcelCommon.Interfaces;
using ParcelCreate.DTOs;
using ParcelCreate.Interfaces;
using System.Text.Json;

namespace ParcelCreate.Services
{
    public class BookingSvc : IBookingSvc
    {
        private IEHManager _ehManager;
        private ILogger<BookingSvc> _logger;
        private IHttpClientFactory _httpClientFactory;
        public BookingSvc(ILogger<BookingSvc> logger, IEHManager EvtHubManager, IHttpClientFactory httpClientFactory) 
        { 
            _ehManager = EvtHubManager;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        async Task<BookingResponseDto?> IBookingSvc.CreateNewBooking(BookingRequestDto request)
        {
            try
            {
                string reqMsg = JsonSerializer.Serialize(request);
                _logger.LogInformation(reqMsg);
                _= await _ehManager.PublishEvent(reqMsg);
                var client = _httpClientFactory.CreateClient("ParcelAPIBackend");
                var clientResponse = await client.GetAsync(string.Format($"/api/bookingstatus/{request.ParcelData.TrackingID}"));
                if(clientResponse != null && clientResponse.IsSuccessStatusCode)
                {
                    BookingResponseDto? response = await clientResponse.Content.ReadFromJsonAsync<BookingResponseDto>();
                    return response;
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }
    }
}

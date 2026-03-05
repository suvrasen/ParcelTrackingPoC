using Azure.Core;
using ParcelCommon.Interfaces;
using ParcelCreate.DTOs;
using ParcelCreate.Interfaces;
using System.Text.Json;

namespace ParcelCreate.Services
{
    public class ParcelScanSvc : IParcelScanSvc
    {
        ILogger<ParcelScanSvc> _logger;
        IEHManager _ehManager;
        IHttpClientFactory _httpClientFactory;
        
        public ParcelScanSvc(ILogger<ParcelScanSvc> logger, IEHManager EvtHubManager, IHttpClientFactory httpClientFactory) 
        {
            _logger = logger;
            _ehManager = EvtHubManager;
            _httpClientFactory = httpClientFactory;
        }
        async Task<ParcelScanResponseDto> IParcelScanSvc.CreateScanEvent(ParcelScanRequestDto request)
        {
            string scanEventMsg = JsonSerializer.Serialize(request);
            _logger.LogInformation(scanEventMsg);
            var ehPublishResult = await _ehManager.PublishEvent(scanEventMsg);
            if (ehPublishResult)
            {
                ParcelScanResponseDto parcelScanResponseDto = new ParcelScanResponseDto { ScanEventSucess = true, TrackingId = request.TrackingId };
                return parcelScanResponseDto;
            }
            return new ParcelScanResponseDto { ScanEventSucess = false, TrackingId = request.TrackingId };
        }

        async Task<ParcelTrackResponseDto> IParcelScanSvc.TrackParcelStatus(long TrackingId)
        {
            var client = _httpClientFactory.CreateClient("ParcelAPIBackend");
            var clientResponse = await client.GetAsync(string.Format($"/api/trackparcel/{TrackingId}"));
            if (clientResponse != null && clientResponse.IsSuccessStatusCode)
            {
                ParcelTrackResponseDto? response = await clientResponse.Content.ReadFromJsonAsync<ParcelTrackResponseDto>();
                return response!;
            }
            return default(ParcelTrackResponseDto)!;
        }
    }
}

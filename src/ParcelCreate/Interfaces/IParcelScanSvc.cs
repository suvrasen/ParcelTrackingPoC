using ParcelCreate.DTOs;

namespace ParcelCreate.Interfaces
{
    public interface IParcelScanSvc
    {
        Task<ParcelScanResponseDto> CreateScanEvent(ParcelScanRequestDto request);
        Task<ParcelTrackResponseDto> TrackParcelStatus(long TrackingId);
    }
}

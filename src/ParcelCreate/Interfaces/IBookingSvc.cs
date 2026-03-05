using Microsoft.AspNetCore.Mvc;
using ParcelCreate.DTOs;

namespace ParcelCreate.Interfaces
{
    public interface IBookingSvc
    {
        Task<BookingResponseDto?> CreateNewBooking(BookingRequestDto request);
    }
}

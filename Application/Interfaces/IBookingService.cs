using Application.DTO.Requests.Booking;
using Application.DTO.Responses;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request);
        Task<BookingResponse> GetBookingByIdAsync(Guid bookingId);
    }
}

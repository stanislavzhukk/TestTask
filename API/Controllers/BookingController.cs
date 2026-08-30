using API.Extensions;
using Application.DTO.Requests.Booking;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>Hall booking endpoints. All actions require an authenticated user.</summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>Books a hall for the given date and time range.</summary>
        /// <response code="404">Hall not found.</response>
        /// <response code="409">The hall is already booked for the selected time range.</response>
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            var userId = User.GetUserId();
            var response = await _bookingService.CreateBookingAsync(userId, request);
            return CreatedAtAction(nameof(GetBookingById), new { id = response.Id }, response);
        }

        /// <summary>Cancels a booking. Only the user who created the booking may cancel it.</summary>
        /// <response code="403">The booking belongs to a different user.</response>
        /// <response code="404">Booking not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(Guid id)
        {
            var userId = User.GetUserId();
            await _bookingService.CancelBookingAsync(userId, id);
            return NoContent();
        }

        /// <summary>Retrieves a single booking by id.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var response = await _bookingService.GetBookingByIdAsync(id);
            return Ok(response);
        }
    }
}
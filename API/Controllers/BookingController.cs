using API.Extensions;
using Application.DTO.Requests.Booking;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            var userId = User.GetUserId();
            var response = await _bookingService.CreateBookingAsync(userId, request);
            return CreatedAtAction(nameof(GetBookingById), new { id = response.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(Guid id)
        {
            var userId = User.GetUserId();
            await _bookingService.CancelBookingAsync(userId, id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var response = await _bookingService.GetBookingByIdAsync(id);
            return Ok(response);
        }
    }
}

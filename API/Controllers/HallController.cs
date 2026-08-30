using Application.DTO.Requests.Amenity;
using Application.DTO.Requests.Hall;
using Application.DTO.Responses;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>Conference hall management and search endpoints.</summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class HallController : ControllerBase
    {
        private readonly IHallService _hallService;
        public HallController(IHallService hallService)
        {
            _hallService = hallService;
        }

        /// <summary>Creates a new hall, optionally with an initial list of amenities.</summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateHall([FromBody] CreateHallRequest request)
        {
            var response = await _hallService.CreateHallAsync(request);
            return CreatedAtAction(nameof(GetHallById), new { id = response.Id }, response);
        }

        /// <summary>Searches for halls available in the given date/time range, optionally filtered by minimum capacity.</summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchHalls([FromQuery] SearchHallsRequest request)
        {
            var response = await _hallService.SearchHallsAsync(request);
            return Ok(response);
        }

        /// <summary>Retrieves a single hall by id.</summary>
        /// <response code="404">Hall not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHallById(Guid id)
        {
            var response = await _hallService.GetHallByIdAsync(id);
            return Ok(response);
        }

        /// <summary>Retrieves all active halls.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAllHalls()
        {
            var response = await _hallService.GetAllHallsAsync();
            return Ok(response);
        }

        /// <summary>Partially updates a hall's base fields (name, capacity, price). Amenities are not affected.</summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<HallResponse>> UpdateHall(Guid id, [FromBody] UpdateHallRequest request)
        {
            var result = await _hallService.UpdateHallAsync(id, request);
            return Ok(result);
        }

        /// <summary>Deactivates a hall (soft delete).</summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHall(Guid id)
        {
            await _hallService.DeleteHallAsync(id);
            return NoContent();
        }

        /// <summary>Adds a new amenity to a hall.</summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("{hallId}/amenities")]
        public async Task<ActionResult<HallAmenityResponse>> AddAmenity(Guid hallId, [FromBody] CreateAmenityRequest request)
        {
            var result = await _hallService.AddAmenityAsync(hallId, request);
            return CreatedAtAction(nameof(GetHallById), new { id = hallId }, result);
        }

        /// <summary>Removes an amenity from a hall.</summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{hallId}/amenities/{amenityId}")]
        public async Task<IActionResult> RemoveAmenity(Guid hallId, Guid amenityId)
        {
            await _hallService.RemoveAmenityAsync(hallId, amenityId);
            return NoContent();
        }

        /// <summary>Updates the price of an amenity already attached to a hall.</summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{hallId}/amenities/{amenityId}")]
        public async Task<ActionResult<HallAmenityResponse>> UpdateAmenityPrice(Guid hallId, Guid amenityId, [FromBody] UpdateHallAmenityPriceRequest request)
        {
            var result = await _hallService.UpdateAmenityPriceAsync(hallId, amenityId, request);
            return Ok(result);
        }
    }
}
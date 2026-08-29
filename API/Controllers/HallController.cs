using Application.DTO.Requests.Amenity;
using Application.DTO.Requests.Hall;
using Application.DTO.Responses;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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

        [HttpPost]
        public async Task<IActionResult> CreateHall([FromBody] CreateHallRequest request)
        {
            var response = await _hallService.CreateHallAsync(request);
            return CreatedAtAction(nameof(GetHallById), new { id = response.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHallById(Guid id)
        {
            var response = await _hallService.GetHallByIdAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHalls()
        {
            var response = await _hallService.GetAllHallsAsync();
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<HallResponse>> UpdateHall(Guid id, [FromBody] UpdateHallRequest request)
        {
            var result = await _hallService.UpdateHallAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHall(Guid id)
        {
            await _hallService.DeleteHallAsync(id);
            return NoContent();
        }

        [HttpPut("{hallId}/amenities")]
        public async Task<ActionResult<HallAmenityResponse>> AddAmenity(Guid hallId, [FromBody] CreateAmenityRequest request)
        {
            var result = await _hallService.AddAmenityAsync(hallId, request);
            return CreatedAtAction(nameof(GetHallById), new { id = hallId }, result);
        }

        [HttpDelete("{hallId}/amenities/{amenityId}")]
        public async Task<IActionResult> RemoveAmenity(Guid hallId, Guid amenityId)
        {
            await _hallService.RemoveAmenityAsync(hallId, amenityId);
            return NoContent();
        }

        [HttpPatch("{hallId}/amenities/{amenityId}")]
        public async Task<ActionResult<HallAmenityResponse>> UpdateAmenityPrice(Guid hallId, Guid amenityId, [FromBody] UpdateHallAmenityPriceRequest request)
        {
            var result = await _hallService.UpdateAmenityPriceAsync(hallId, amenityId, request);
            return Ok(result);
        }
    }
}

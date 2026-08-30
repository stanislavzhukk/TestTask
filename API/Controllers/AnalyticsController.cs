using Application.DTO.Requests.Analytics;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        public AnalyticsController(IAnalyticsService analyticsService) {
            _analyticsService = analyticsService;
        }

        /// <summary>Occupancy percentage per hall over a date range.</summary>
        [HttpGet("occupancy")]
        public async Task<IActionResult> GetOccupancy([FromQuery] AnalyticsDateRangeRequest request)
        {
            var response = await _analyticsService.GetOccupancyAsync(request.DateFrom, request.DateTo);
            return Ok(response);
        }


        /// <summary>Earning breakdown by hall and by source (hall rental vs amenities).</summary>
        [HttpGet("earning")]
        public async Task<IActionResult> GetEarning([FromQuery] AnalyticsDateRangeRequest request)
        {
            var response = await _analyticsService.GetEarningAsync(request.DateFrom, request.DateTo);
            return Ok(response);
        }   

        /// <summary>Most-booked halls over a date range.</summary>
        [HttpGet("popular-halls")]
        public async Task<IActionResult> GetPopularHalls([FromQuery] AnalyticsDateRangeRequest request, [FromQuery, DefaultValue(5)] int top = 5)
        {
            var response = await _analyticsService.GetPopularHallsAsync(request.DateFrom, request.DateTo, top);
            return Ok(response);
        }   

        /// <summary>How often each amenity was selected, and the revenue it generated.</summary>
        [HttpGet("amenities-usage")]
        public async Task<IActionResult> GetAmenitiesUsage([FromQuery] AnalyticsDateRangeRequest request)
        {
            var response = await _analyticsService.GetAmenitiesUsageAsync(request.DateFrom, request.DateTo);
            return Ok(response);
        }
    }
}

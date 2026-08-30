using Application.DTO.Responses.Analytics;

namespace Application.Interfaces
{
    public interface IAnalyticsService
    {
        Task<List<HallOccupancyResponse>> GetOccupancyAsync(DateOnly dateFrom, DateOnly dateTo);
        Task<EarningReportResponse> GetEarningAsync(DateOnly dateFrom, DateOnly dateTo);
        Task<List<HallOccupancyResponse>> GetPopularHallsAsync(DateOnly dateFrom, DateOnly dateTo, int top);
        Task<List<AmenityUsageResponse>> GetAmenitiesUsageAsync(DateOnly dateFrom, DateOnly dateTo);
    }
}

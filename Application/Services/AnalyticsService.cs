using Application.DTO.Responses.Analytics;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IBookingRepository _bookingRepository;
        private const decimal OperatingHoursPerDay = 17m;   // 06:00–23:00

        public AnalyticsService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<List<HallOccupancyResponse>> GetOccupancyAsync(DateOnly dateFrom, DateOnly dateTo)
        {
            var (start, end) = ToRange(dateFrom, dateTo);
            var totalAvailableHours = GetTotalAvailableHours(dateFrom, dateTo);

            var result = await _bookingRepository.QueryInRange(start, end)
                .GroupBy(b => new { b.HallId, b.Hall.Name })
                .Select(g => new HallOccupancyResponse
                {
                    HallId = g.Key.HallId,
                    HallName = g.Key.Name,
                    BookedHours = g.Sum(b => (decimal)(b.EndTime - b.StartTime).TotalHours),
                    BookingsCount = g.Count()
                })
                .ToListAsync();

            ApplyOccupancyPercentage(result, totalAvailableHours);

            return result.OrderByDescending(r => r.OccupancyPercentage).ToList();
        }

        public async Task<EarningReportResponse> GetEarningAsync(DateOnly dateFrom, DateOnly dateTo)
        {
            var (start, end) = ToRange(dateFrom, dateTo);
            var query = _bookingRepository.QueryInRange(start, end);

            var totalRevenue = await query.SumAsync(b => b.TotalCost);
            var amenitiesRevenue = await query
                .SelectMany(b => b.SelectedAmenities)
                .SumAsync(a => a.PriceAtBooking);

            var byHall = await query
                .GroupBy(b => new { b.HallId, b.Hall.Name })
                .Select(g => new EarningByHallItem
                {
                    HallId = g.Key.HallId,
                    HallName = g.Key.Name,
                    Earning = g.Sum(b => b.TotalCost),
                    BookingsCount = g.Count()
                })
                .OrderByDescending(r => r.Earning)
                .ToListAsync();

            return new EarningReportResponse
            {
                TotalEarning = totalRevenue,
                HallEarning = totalRevenue - amenitiesRevenue,
                AmenitiesEarning = amenitiesRevenue,
                ByHall = byHall
            };
        }

        public async Task<List<HallOccupancyResponse>> GetPopularHallsAsync(DateOnly dateFrom, DateOnly dateTo, int top)
        {
            var (start, end) = ToRange(dateFrom, dateTo);
            var totalAvailableHours = GetTotalAvailableHours(dateFrom, dateTo);

            var result = await _bookingRepository.QueryInRange(start, end)
                .GroupBy(b => new { b.HallId, b.Hall.Name })
                .Select(g => new HallOccupancyResponse
                {
                    HallId = g.Key.HallId,
                    HallName = g.Key.Name,
                    BookedHours = g.Sum(b => (decimal)(b.EndTime - b.StartTime).TotalHours),
                    BookingsCount = g.Count()
                })
                .OrderByDescending(r => r.BookingsCount)   // "popular" = the most count of bookings, not % occupancy
                .Take(top)
                .ToListAsync();

            ApplyOccupancyPercentage(result, totalAvailableHours);

            return result;
        }

        public async Task<List<AmenityUsageResponse>> GetAmenitiesUsageAsync(DateOnly dateFrom, DateOnly dateTo)
        {
            var (start, end) = ToRange(dateFrom, dateTo);

            return await _bookingRepository.QueryInRange(start, end)
                .SelectMany(b => b.SelectedAmenities)
                .GroupBy(a => a.Amenity.Name)
                .Select(g => new AmenityUsageResponse
                {
                    AmenityName = g.Key,
                    TimesSelected = g.Count(),
                    TotalRevenue = g.Sum(a => a.PriceAtBooking)
                })
                .OrderByDescending(r => r.TimesSelected)
                .ToListAsync();
        }

        private static (DateTime start, DateTime end) ToRange(DateOnly dateFrom, DateOnly dateTo)
        {
            return (
                dateFrom.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
                dateTo.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc)
            );
        }

        private static decimal GetTotalAvailableHours(DateOnly dateFrom, DateOnly dateTo)
        {
            return (dateTo.DayNumber - dateFrom.DayNumber + 1) * OperatingHoursPerDay;
        }

        private static void ApplyOccupancyPercentage(List<HallOccupancyResponse> items, decimal totalAvailableHours)
        {
            foreach (var r in items)
            {
                r.OccupancyPercentage = totalAvailableHours > 0
                    ? Math.Round(r.BookedHours / totalAvailableHours * 100, 1)
                    : 0;
            }
        }
    }
}
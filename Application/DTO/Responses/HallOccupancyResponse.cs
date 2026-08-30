namespace Application.DTO.Responses.Analytics
{
    /// <summary>Occupancy percentage for a single hall over the reporting period.</summary>
    public class HallOccupancyResponse
    {
        public Guid HallId { get; set; }
        public string HallName { get; set; } = string.Empty;

        /// <summary>Total hours the hall was booked during the period.</summary>
        public decimal BookedHours { get; set; }

        /// <summary>Booked hours as a percentage of total available operating hours (06:00–23:00) in the period.</summary>
        public decimal OccupancyPercentage { get; set; }

        public int BookingsCount { get; set; }
    }
}
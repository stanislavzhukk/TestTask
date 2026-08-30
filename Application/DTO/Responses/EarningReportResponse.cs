namespace Application.DTO.Responses.Analytics
{
    /// <summary>Earning breakdown for the reporting period.</summary>
    public class EarningReportResponse
    {
        public decimal TotalEarning { get; set; }

        /// <summary>Earning attributed to base hall rental (before amenities).</summary>
        public decimal HallEarning { get; set; }

        /// <summary>Earning attributed to selected amenities.</summary>
        public decimal AmenitiesEarning { get; set; }

        /// <summary>Earning broken down by hall.</summary>
        public List<EarningByHallItem> ByHall { get; set; } = new();
    }

    public class EarningByHallItem
    {
        public Guid HallId { get; set; }
        public string HallName { get; set; } = string.Empty;
        public decimal Earning { get; set; }
        public int BookingsCount { get; set; }
    }
}
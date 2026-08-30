namespace Application.DTO.Responses.Analytics
{
    /// <summary>Usage frequency of amenities across all bookings in the period.</summary>
    public class AmenityUsageResponse
    {
        public string AmenityName { get; set; } = string.Empty;
        public int TimesSelected { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
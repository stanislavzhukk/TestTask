using System.ComponentModel;

namespace Application.DTO.Requests.Analytics
{
    /// <summary>Common date range filter shared across analytics endpoints.</summary>
    public class AnalyticsDateRangeRequest
    {
        /// <summary>Start of the reporting period (inclusive).</summary>
        [DefaultValue("2026-08-30")]
        public DateOnly DateFrom { get; set; }

        /// <summary>End of the reporting period (inclusive).</summary>
        [DefaultValue("2026-09-30")]
        public DateOnly DateTo { get; set; }
    }
}
using System.ComponentModel;

namespace Application.DTO.Requests.Hall
{
    /// <summary>Request to search for halls available for a given date and time range.</summary>
    /// <summary>Date of the search, in ISO 8601 format (yyyy-MM-dd).</summary>
    public class SearchHallsRequest
    {
        /// <summary>Date of the search (yyyy-MM-dd).</summary>
        [DefaultValue("2024-09-01")]
        public DateOnly Date { get; set; }

        /// <summary>Start of the desired time range (HH:mm).</summary>
        [DefaultValue("10:00")]
        public TimeOnly StartTime { get; set; }

        /// <summary>End of the desired time range (HH:mm).</summary>
        [DefaultValue("14:00")]
        public TimeOnly EndTime { get; set; }

        /// <summary>Minimum required capacity. If omitted, no capacity filter is applied.</summary>
        [DefaultValue(50)]
        public int? MinCapacity { get; set; }
    }
}
namespace Application.DTO.Requests.Booking
{
    /// <summary>Request to book a conference hall for a given date and time range.</summary>
    public class CreateBookingRequest
    {
        /// <summary>Id of the hall being booked.</summary>
        public required Guid HallId { get; set; }

        /// <summary>Date of the booking.</summary>
        /// <example>2024-09-01</example>
        public required DateOnly Date { get; set; }

        /// <summary>Start time of the booking (must fall within operating hours, 06:00–23:00).</summary>
        /// <example>13:00</example>
        public required TimeOnly StartTime { get; set; }

        /// <summary>End time of the booking.</summary>
        /// <example>15:00</example>
        public required TimeOnly EndTime { get; set; }

        /// <summary>Ids of the selected amenities. Each amenity may be selected at most once.</summary>
        public List<Guid> SelectedAmenityIds { get; set; } = new();
    }
}
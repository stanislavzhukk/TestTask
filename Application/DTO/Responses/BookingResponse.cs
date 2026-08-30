using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    /// <summary>Confirmation of a booking, including the amenities selected and the calculated total cost.</summary>
    public class BookingResponse
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public string HallName { get; set; }

        /// <summary>Current status of the booking (e.g. Confirmed, Cancelled).</summary>
        public BookingStatus Status { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        /// <summary>Amenities selected for this booking, with prices locked in at booking time.</summary>
        public List<BookingAmenityResponse> BookingAmenities { get; set; } = new();

        /// <summary>
        /// Total cost of the booking — base hall cost (with time-of-day discounts/surcharges applied
        /// segment by segment) plus the fixed sum of selected amenities.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
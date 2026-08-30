using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    /// <summary>An amenity selected as part of a booking, with the price locked in at booking time.</summary>
    public class BookingAmenityResponse
    {
        public Guid AmenityId { get; set; }
        public string Name { get; set; } = null!;

        /// <summary>Price at the time the booking was made — not affected by later price changes on the hall.</summary>
        public decimal Price { get; set; }
    }
}
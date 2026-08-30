using Application.DTO.Requests.Amenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Requests.Hall
{
    /// <summary>Request to partially update a hall's base fields. Amenities are not affected by this request.</summary>
    public class UpdateHallRequest
    {
        /// <summary>New hall name. If omitted, the current value is kept.</summary>
        public string? Name { get; set; }

        /// <summary>New capacity. If omitted, the current value is kept.</summary>
        public int? Capacity { get; set; }

        /// <summary>New base hourly rate. If omitted, the current value is kept.</summary>
        public decimal? PricePerHour { get; set; }
    }
}
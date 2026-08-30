using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    /// <summary>An amenity together with its price in a specific hall.</summary>
    public class HallAmenityResponse
    {
        /// <summary>Id of the amenity in the catalog.</summary>
        public required Guid Id { get; set; }

        public required string Name { get; set; } = string.Empty;

        /// <summary>Price of this amenity in this particular hall.</summary>
        public required decimal Price { get; set; }
    }
}
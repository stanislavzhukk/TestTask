using Application.DTO.Requests.Amenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    /// <summary>Full representation of a conference hall, including its available amenities.</summary>
    public class HallResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }

        /// <summary>Base rental cost per hour.</summary>
        public required decimal Price { get; set; }

        public required int Capacity { get; set; }

        public required List<HallAmenityResponse> AvailableAmenities { get; set; } = new List<HallAmenityResponse>();
    }
}
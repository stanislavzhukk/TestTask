using Application.DTO.Requests.Amenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class HallResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public required int Capacity { get; set; }

        public required List<HallAmenityResponse> Services { get; set; } = new List<HallAmenityResponse>();
    }
}

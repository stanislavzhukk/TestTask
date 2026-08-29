using Application.DTO.Requests.Amenity;
using Domain.Models;

namespace Application.DTO.Requests.Hall
{
    public class CreateHallRequest
    {
        public required string Name { get; set; } = string.Empty;
        public required int Capacity { get; set; }
        public required decimal PricePerHour { get; set; }
        public required List<CreateAmenityRequest> Services { get; set; } = new List<CreateAmenityRequest>();
    }
}

using Application.DTO.Requests.Amenity;
using Domain.Models;

namespace Application.DTO.Requests.Hall
{
    /// <summary>Request to create a new conference hall, optionally with an initial list of amenities.</summary>
    public class CreateHallRequest
    {
        /// <example>Hall A</example>
        public required string Name { get; set; } = string.Empty;

        /// <summary>Maximum number of people the hall can accommodate.</summary>
        /// <example>50</example>
        public required int Capacity { get; set; }

        /// <summary>Base rental cost per hour.</summary>
        /// <example>2000</example>
        public required decimal PricePerHour { get; set; }

        /// <summary>Amenities to attach to the hall at creation time. Can be empty.</summary>
        public required List<CreateAmenityRequest> Amenities { get; set; } = new List<CreateAmenityRequest>();
    }
}
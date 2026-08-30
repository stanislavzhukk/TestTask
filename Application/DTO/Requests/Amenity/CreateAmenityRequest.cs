namespace Application.DTO.Requests.Amenity
{
    /// <summary>Data for a service (amenity) added to a hall, either at hall creation or separately.</summary>
    public class CreateAmenityRequest
    {
        /// <summary>Name of the service.</summary>
        /// <example>Projector</example>
        public required string Name { get; set; }

        /// <summary>Cost of using this service in the hall, a fixed amount regardless of booking duration.</summary>
        /// <example>500</example>
        public required decimal Price { get; set; }
    }
}
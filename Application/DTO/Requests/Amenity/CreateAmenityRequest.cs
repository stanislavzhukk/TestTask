namespace Application.DTO.Requests.Amenity
{
    public class CreateAmenityRequest   
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }
    }
}

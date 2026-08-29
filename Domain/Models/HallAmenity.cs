namespace Domain.Models
{
    public class HallAmenity
    {
        public Guid HallId { get; set; }
        public Guid AmenityId { get; set; }
        public decimal Price { get; set; }

        public Amenity Amenity { get; set; } = null!;
        public Hall Hall { get; set; } = null!;
    }
}
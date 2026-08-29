using Domain.Models;

public class BookingAmenity
{
    public Guid BookingId { get; set; }
    public Guid AmenityId { get; set; }
    public decimal PriceAtBooking { get; set; }
    public Booking Booking { get; set; } = null!;
    public Amenity Amenity { get; set; } = null!;
}
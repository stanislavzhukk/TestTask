namespace Domain.Models
{
    public class Hall
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal BaseHourlyRate { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<HallAmenity> AvailableAmenities { get; set; } = new List<HallAmenity>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

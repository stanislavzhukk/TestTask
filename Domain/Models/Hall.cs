namespace Domain.Models
{
    public class Hall
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Capacity { get; set; }
        public decimal BaseHourlyRate { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<HallService> AvailableServices { get; set; } = new List<HallService>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

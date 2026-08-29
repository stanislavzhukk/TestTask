namespace Domain.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public DateTime StartTime { get; set; }   // UTC
        public DateTime EndTime { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public decimal TotalCost { get; set; }
       
        public Guid UserId { get; set; } // buyer
        public DateTime CreatedAt { get; set; }

        public Hall Hall { get; set; } = null!;
        public ICollection<BookingService> SelectedServices { get; set; } = new List<BookingService>();
    }
}
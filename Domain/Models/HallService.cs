namespace Domain.Models
{
    public class HallService
    {
        public Guid HallId { get; set; }
        public Guid ServiceId { get; set; }
        public decimal Price { get; set; }

        public Service Service { get; set; } = null!;
        public Hall Hall { get; set; } = null!;
    }
}
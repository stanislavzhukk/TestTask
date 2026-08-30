
namespace Application.DTO.Requests.Booking
{
    public class CreateBookingRequest
    {
        public required Guid HallId { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
        public List<Guid> SelectedAmenityIds { get; set; } = new();
    }
}

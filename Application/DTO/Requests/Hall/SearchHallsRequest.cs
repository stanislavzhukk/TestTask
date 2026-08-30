
namespace Application.DTO.Requests.Hall
{
    public class SearchHallsRequest
    {
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int? MinCapacity { get; set; }
    }
}

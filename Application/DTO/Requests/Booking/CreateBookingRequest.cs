using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Requests.Booking
{
    public class CreateBookingRequest
    {
        public Guid HallId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Guid> SelectedAmenityIds { get; set; } = new List<Guid>();
    }
}

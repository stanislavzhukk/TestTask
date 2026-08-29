using Application.DTO.Requests.Amenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Requests.Hall
{
    public class UpdateHallRequest
    {
        public string? Name { get; set; }
        public int? Capacity { get; set; }
        public decimal? PricePerHour { get; set; }
    }
}

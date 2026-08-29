using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class HallAmenityResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required decimal Price { get; set; }
    }
}

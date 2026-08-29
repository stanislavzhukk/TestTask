using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPricingCalculatorService
    {
        Task<decimal> CalculateTotalPriceAsync(Guid hallId, decimal baseHourlyRate, DateTime startTime, DateTime endTime, List<Guid> selectedAmenityIds);
    }
}

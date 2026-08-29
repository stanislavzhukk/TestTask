

using Application.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services
{
    public class PricingCalculatorService : IPricingCalculatorService
    {
        private readonly IAmenityRepository _amenityRepository;
        private readonly List<(TimeOnly Start, TimeOnly End, RateType Type)> TimeRanges = new()
        {
            (new TimeOnly(12, 0), new TimeOnly(14, 0), RateType.Peak),
            (new TimeOnly(6, 0),  new TimeOnly(9, 0),  RateType.Morning),
            (new TimeOnly(18, 0), new TimeOnly(23, 0), RateType.Evening),
            (new TimeOnly(9, 0),  new TimeOnly(18, 0), RateType.Standard),
        };

        public PricingCalculatorService(IAmenityRepository amenityRepository)
        {
            _amenityRepository = amenityRepository;
        }

        public async Task<decimal> CalculateTotalPriceAsync(Guid hallId, decimal baseHourlyRate, DateTime start, DateTime end, List<Guid> selectedAmenityIds)
        {
            var hallCost = CalculateHallCost(baseHourlyRate, start, end);
            Console.WriteLine($"Calculated hall cost: {hallCost}");
            Console.WriteLine($"Calculating total price for hall {hallId} from {start} to {end} with base hourly rate {baseHourlyRate}");
            var amenitiesCost = await CalculateAmenityCost(hallId ,selectedAmenityIds);
            Console.WriteLine($"Calculated amenities cost: {amenitiesCost}");
            return hallCost + amenitiesCost;
        }

        private async Task<decimal> CalculateAmenityCost(Guid hallId, List<Guid> selectedAmenityIds)
        {
            var cost = 0m;
            foreach (var amenityId in selectedAmenityIds)
            {
                var amenity = await _amenityRepository.GetHallAmenityAsync(hallId ,amenityId);
                if (amenity == null)
                {
                    throw new ArgumentException($"Amenity with ID {amenityId} does not exist.");
                }
                cost += amenity.Price;
            }
            return cost;
        }

        private decimal CalculateHallCost(decimal baseHourlyRate, DateTime start, DateTime end)
        {
            decimal total = 0;
            var current = start;

            while (current < end)
            {
                var segmentEnd = current.AddHours(1) < end ? current.AddHours(1) : end;
                var duration = (decimal)(segmentEnd - current).TotalHours;

                var rateType = GetRateType(TimeOnly.FromDateTime(current));
                total += baseHourlyRate * GetMultiplier(rateType) * duration;

                current = segmentEnd;
            }

            return total;
        }

        private RateType GetRateType(TimeOnly time)
        {
            foreach (var range in TimeRanges)
            {
                if (time >= range.Start && time < range.End)
                {
                    return range.Type;
                }
            }

            throw new BadRequestException($"Booking time {time} falls outside of operating hours (06:00–23:00).");
        }

        private decimal GetMultiplier(RateType rateType) => rateType switch
        {
            RateType.Morning => 0.9m,
            RateType.Peak => 1.15m,
            RateType.Evening => 0.8m,
            RateType.Standard => 1.0m,
            _ => throw new ArgumentOutOfRangeException(nameof(rateType))
        };
    }
}

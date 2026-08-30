using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders
{
    public class HallsSeeder
    {
        public static async Task SeedHallsAsync(IServiceProvider serviceProvider)
        {
            var hallRepository = serviceProvider.GetRequiredService<IHallRepository>();
            var existingHalls = await hallRepository.GetAllHallsAsync();
            if (existingHalls.Any())
            {
                return; // Halls already seeded
            }

            var halls = new List<Hall>
            {
                new Hall { Title = "Hall A", Capacity = 50, BaseHourlyRate = 2000m },
                new Hall { Title = "Hall B", Capacity = 30, BaseHourlyRate = 3500m },
                new Hall { Title = "Hall C", Capacity = 200, BaseHourlyRate = 1500m }
            };

            var amenities = new List<Amenity>
            {
                new Amenity {Id = Guid.NewGuid(), Name = "Projector" },
                new Amenity {Id = Guid.NewGuid(), Name = "Wi-Fi" },
                new Amenity {Id = Guid.NewGuid(), Name = "Sound System" },
            };

            var amenityPrices = new Dictionary<Guid, decimal>
            {
                [amenities[0].Id] = 500m,   // Projector
                [amenities[1].Id] = 300m,   // Wi-Fi
                [amenities[2].Id] = 700m,   // Sound System
            };

            foreach (var hall in halls)
            {
                hall.AvailableAmenities = amenities.Select(a => new HallAmenity
                {
                    HallId = hall.Id,
                    AmenityId = a.Id,
                    Amenity = a,
                    Price = amenityPrices[a.Id]
                }).ToList();
            }

            foreach (var hall in halls)
            {
                await hallRepository.CreateHallAsync(hall);
            }
        }
    }
}

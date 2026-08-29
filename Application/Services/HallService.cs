using Application.DTO.Requests.Amenity;
using Application.DTO.Requests.Hall;
using Application.DTO.Responses;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class HallService : IHallService
    {
        public readonly IHallRepository _hallRepository;
        public readonly IAmenityRepository _amenityRepository;
        public readonly IUnitOfWork _unitOfWork;

        public HallService(IHallRepository hallRepository, IAmenityRepository amenityRepository, IUnitOfWork unitOfWork)
        {
            _hallRepository = hallRepository;
            _amenityRepository = amenityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<HallResponse> CreateHallAsync(CreateHallRequest request)
        {
            Hall response = null!;

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var hall = new Hall
                {
                    Id = Guid.NewGuid(),
                    Title = request.Name,
                    Capacity = request.Capacity,
                    BaseHourlyRate = request.PricePerHour,
                    IsActive = true
                };

                var hallAmenities = new List<HallAmenity>();

                foreach (var s in request.Services)
                {
                    var amenity = await _amenityRepository.GetByNameAsync(s.Name);
                    if (amenity == null)
                    {
                        amenity = new Amenity { Id = Guid.NewGuid(), Name = s.Name };
                        await _amenityRepository.AddAsync(amenity);
                    }

                    hallAmenities.Add(new HallAmenity
                    {
                        HallId = hall.Id,
                        AmenityId = amenity.Id,
                        Amenity = amenity,
                        Price = s.Price
                    });
                }

                hall.AvailableAmenities = hallAmenities;
                response = await _hallRepository.CreateHallAsync(hall);
            });

            return MapToResponse(response);
        }

        public async Task DeleteHallAsync(Guid hallId)
        {
            var hall = await _hallRepository.GetHallByIdAsync(hallId);
            if (hall == null)
            {
                throw new NotFoundException($"Hall with ID {hallId} not found.");
            }

            hall.IsActive = false; 
            await _hallRepository.UpdateHallAsync(hall);
        }

        public async Task<List<HallResponse>> GetAllHallsAsync()
        {
            var halls = await _hallRepository.GetAllHallsAsync();
            return halls.Select(MapToResponse).ToList();
        }

        public async Task<HallResponse> GetHallByIdAsync(Guid hallId)
        {
            var hall = await _hallRepository.GetHallByIdAsync(hallId);
            if (hall == null)
            {
                throw new KeyNotFoundException($"Hall with ID {hallId} not found.");
            }
            return MapToResponse(hall);
        }

        public async Task<HallResponse> UpdateHallAsync(Guid hallId, UpdateHallRequest request)
        {
            var hall = await _hallRepository.GetHallByIdAsync(hallId);
            if (hall == null)
            {
                throw new NotFoundException($"Hall with ID {hallId} not found.");
            }

            hall.Title = request.Name ?? hall.Title;
            hall.Capacity = request.Capacity ?? hall.Capacity;
            hall.BaseHourlyRate = request.PricePerHour ?? hall.BaseHourlyRate;

            var updatedHall = await _hallRepository.UpdateHallAsync(hall);

            return MapToResponse(updatedHall);
        }

        public async Task<HallAmenityResponse> AddAmenityAsync(Guid hallId, CreateAmenityRequest request)
        {
            var hall = await _hallRepository.GetHallByIdAsync(hallId);
            if (hall == null)
            {
                throw new NotFoundException($"Hall with ID {hallId} not found.");
            }

            var amenity = await _amenityRepository.GetByNameAsync(request.Name);
            if (amenity == null)
            {
                amenity = new Amenity { Id = Guid.NewGuid(), Name = request.Name };
                await _amenityRepository.AddAsync(amenity);
            }

            var hallAmenity = new HallAmenity
            {
                HallId = hall.Id,
                AmenityId = amenity.Id,
                Amenity = amenity,
                Price = request.Price
            };

            hall.AvailableAmenities.Add(hallAmenity);
            await _hallRepository.UpdateHallAsync(hall);

            return new HallAmenityResponse
            {
                Id = amenity.Id,
                Name = amenity.Name,
                Price = hallAmenity.Price
            };
        }

        public async Task RemoveAmenityAsync(Guid hallId, Guid amenityId)
        {
            var hallAmenity = await _amenityRepository.GetHallAmenityAsync(hallId, amenityId);
            if (hallAmenity == null)
            {
                throw new NotFoundException($"Amenity {amenityId} not found for hall {hallId}.");
            }

            await _amenityRepository.RemoveHallAmenityAsync(hallAmenity);
        }

        public async Task<HallAmenityResponse> UpdateAmenityPriceAsync(Guid hallId, Guid amenityId, UpdateHallAmenityPriceRequest request)
        {
            var hallAmenity = await _amenityRepository.GetHallAmenityAsync(hallId, amenityId);
            if (hallAmenity == null)
            {
                throw new NotFoundException($"Amenity {amenityId} not found for hall {hallId}.");
            }

            hallAmenity.Price = request.Price;
            await _amenityRepository.UpdateHallAmenityAsync(hallAmenity);

            return new HallAmenityResponse
            {
                Id = hallAmenity.Amenity.Id,
                Name = hallAmenity.Amenity.Name,
                Price = hallAmenity.Price
            };
        }

        private static HallResponse MapToResponse(Hall hall) => new()
        {
            Id = hall.Id,
            Name = hall.Title,
            Capacity = hall.Capacity,
            Price = hall.BaseHourlyRate,
            Services = hall.AvailableAmenities.Select(a => new HallAmenityResponse
            {
                Id = a.Amenity.Id,
                Name = a.Amenity.Name,
                Price = a.Price
            }).ToList()
        };
    }
}

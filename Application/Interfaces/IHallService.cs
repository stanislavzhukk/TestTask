using Application.DTO.Requests.Amenity;
using Application.DTO.Requests.Hall;
using Application.DTO.Responses;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IHallService
    {
        Task<List<HallResponse>> GetAllHallsAsync();
        Task<HallResponse> GetHallByIdAsync(Guid hallId);
        Task<HallResponse> CreateHallAsync(CreateHallRequest request);
        Task<HallResponse> UpdateHallAsync(Guid hallId, UpdateHallRequest request);
        Task<HallAmenityResponse> AddAmenityAsync(Guid hallId, CreateAmenityRequest request);
        Task RemoveAmenityAsync(Guid hallId, Guid amenityId);
        Task DeleteHallAsync(Guid hallId);
    }
}

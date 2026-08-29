using Domain.Models;

namespace Domain.Interfaces
{
    public interface IAmenityRepository
    {
        Task<List<Amenity>> GetAllAmenitiesAsync();
        Task<Amenity?> GetAmenityByIdAsync(Guid id);
        Task<Amenity?> GetByNameAsync(string name);
        Task<Amenity> AddAsync(Amenity amenity);

        Task<HallAmenity?> GetHallAmenityAsync(Guid hallId, Guid amenityId);
        Task RemoveHallAmenityAsync(HallAmenity hallAmenity);
        Task UpdateHallAmenityAsync(HallAmenity hallAmenity);
    }
}

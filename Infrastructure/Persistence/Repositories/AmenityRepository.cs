using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class AmenityRepository : IAmenityRepository
    {
        private readonly ApplicationDbContext _context;

        public AmenityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Amenity> AddAsync(Amenity amenity)
        {
            var response = await _context.Amenities.AddAsync(amenity);
            await _context.SaveChangesAsync();
            return amenity;
        }

        public async Task<List<Amenity>> GetAllAmenitiesAsync()
        {
            return await _context.Amenities
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Amenity?> GetAmenityByIdAsync(Guid id)
        {
            return await _context.Amenities
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Amenity?> GetByNameAsync(string name)
        {
            return await _context.Amenities
                .FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<HallAmenity?> GetHallAmenityAsync(Guid hallId, Guid amenityId)
        {
            return await _context.HallAmenities
                .FirstOrDefaultAsync(ha => ha.HallId == hallId && ha.AmenityId == amenityId);
        }

        public async Task RemoveHallAmenityAsync(HallAmenity hallAmenity)
        {
            _context.HallAmenities.Remove(hallAmenity);
            await _context.SaveChangesAsync();
        }
    }
}



using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly ApplicationDbContext _context;
        public HallRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Hall> CreateHallAsync(Hall hall)
        {
            await _context.Halls.AddAsync(hall);
            await _context.SaveChangesAsync();
            return hall;
        }

        public async Task<IReadOnlyList<Hall>> GetAllHallsAsync()
        {
            return await _context.Halls
                .AsNoTracking()
                .Include(h => h.AvailableAmenities)
                    .ThenInclude(ha => ha.Amenity)
                .Where(h => h.IsActive)
                .ToListAsync();
        }

        public async Task<Hall?> GetHallByIdAsync(Guid hallId)
        {
            return await _context.Halls
                .Include(h => h.AvailableAmenities)
                    .ThenInclude(ha => ha.Amenity)
                .Where(h => h.IsActive)
                .FirstOrDefaultAsync(h => h.Id == hallId);
        }

        public async Task<Hall> UpdateHallAsync(Hall hall)
        {
            _context.Halls.Update(hall);
            await _context.SaveChangesAsync();
            return hall;
        }
    }
}

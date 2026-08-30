using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<bool> HasOverlapAsync(Guid hallId, DateTime startTime, DateTime endTime)
        {
            return await _context.Bookings
                .AnyAsync(b => b.HallId == hallId && b.StartTime < endTime && b.EndTime > startTime);
        }

        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Where(b => b.Status != BookingStatus.Cancelled)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid id)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Where(b => b.Status != BookingStatus.Cancelled)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Booking> QueryInRange(DateTime start, DateTime end)
        {
            return _context.Bookings
                .Where(b => b.Status != BookingStatus.Cancelled)
                .Where(b => b.StartTime >= start && b.StartTime <= end)
                .AsNoTracking();
        }
    }
}

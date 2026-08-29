using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId);

            builder.Entity<RefreshToken>()
                .HasIndex(rt => rt.Token)
                .IsUnique();

            builder.Entity<HallService>()
            .HasKey(rs => new { rs.HallId, rs.ServiceId });

            builder.Entity<HallService>()
                .HasOne(rs => rs.Hall)
                .WithMany(r => r.AvailableServices)
                .HasForeignKey(rs => rs.HallId);

            builder.Entity<HallService>()
                .HasOne(rs => rs.Service)
                .WithMany()
                .HasForeignKey(rs => rs.ServiceId);

            builder.Entity<BookingService>()
                .HasKey(bs => new { bs.BookingId, bs.ServiceId });

            builder.Entity<Booking>()
                .HasOne(b => b.Hall)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.HallId);

            builder.Entity<Booking>()
                .HasIndex(b => new { b.HallId, b.StartTime, b.EndTime });

            builder.Entity<Hall>()
                .Property(r => r.BaseHourlyRate)
                .HasColumnType("decimal(10,2)");

        }
    }
}

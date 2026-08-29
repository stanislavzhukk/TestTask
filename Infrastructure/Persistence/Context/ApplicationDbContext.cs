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
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<HallAmenity> HallAmenities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingAmenity> BookingAmenities { get; set; }

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

            builder.Entity<HallAmenity>()
            .HasKey(ha => new { ha.HallId, ha.AmenityId });

            builder.Entity<HallAmenity>()
                .HasOne(ha => ha.Hall)
                .WithMany(h => h.AvailableAmenities)
                .HasForeignKey(ha => ha.HallId);

            builder.Entity<HallAmenity>()
                .HasOne(ha => ha.Amenity)
                .WithMany()
                .HasForeignKey(ha => ha.AmenityId);

            builder.Entity<BookingAmenity>()
                .HasKey(bs => new { bs.BookingId, bs.AmenityId });

            builder.Entity<Booking>()
                .HasOne(b => b.Hall)
                .WithMany(h => h.Bookings)
                .HasForeignKey(b => b.HallId);

            builder.Entity<Booking>()
                .HasIndex(b => new { b.HallId, b.StartTime, b.EndTime });

            builder.Entity<Hall>()
                .Property(h => h.BaseHourlyRate)
                .HasColumnType("decimal(10,2)");

        }
    }
}

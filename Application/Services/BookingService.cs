using Application.DTO.Requests.Booking;
using Application.DTO.Responses;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IPricingCalculatorService _pricingCalculatorService;
        private readonly IHallService _hallService;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IPricingCalculatorService pricingCalculatorService, IHallService hallService, IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
        {
            _pricingCalculatorService = pricingCalculatorService;
            _hallService = hallService;
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request)
        {
            var hall = await _hallService.GetHallByIdAsync(request.HallId);
            if (hall == null)
            {
                throw new NotFoundException($"Hall with ID {request.HallId} not found.");
            }

            if (request.SelectedAmenityIds.Count != request.SelectedAmenityIds.Distinct().Count())
            {
                throw new BadRequestException("Duplicate amenity IDs are not allowed in a single booking.");
            }

            Booking booking = null!;

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var hasOverlap = await _bookingRepository.HasOverlapAsync(request.HallId, request.StartTime, request.EndTime);
                if (hasOverlap)
                {
                    throw new BadRequestException("This hall is already booked for the selected time range.");
                }

                var cost = await _pricingCalculatorService.CalculateTotalPriceAsync(
                    request.HallId, hall.Price, request.StartTime, request.EndTime, request.SelectedAmenityIds);

                booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    HallId = request.HallId,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Status = BookingStatus.Confirmed,
                    TotalCost = cost,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    SelectedAmenities = request.SelectedAmenityIds.Select(aid => new BookingAmenity
                    {
                        AmenityId = aid
                    }).ToList() //bad
                };

                await _bookingRepository.AddAsync(booking);
            });

            return new BookingResponse
            {
                Id = booking.Id,
                HallId = booking.HallId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                TotalPrice = booking.TotalCost
            };
        }

        public async Task<BookingResponse> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                throw new NotFoundException($"Booking with ID {bookingId} not found.");
            }

            return new BookingResponse
            {
                Id = booking.Id,
                HallId = booking.HallId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                TotalPrice = booking.TotalCost
            };
        }
    }
}

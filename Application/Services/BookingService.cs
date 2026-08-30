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
        private readonly IAmenityRepository _amenityRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IPricingCalculatorService pricingCalculatorService, IHallService hallService, 
            IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IAmenityRepository amenityRepository)
        {
            _pricingCalculatorService = pricingCalculatorService;
            _hallService = hallService;
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _amenityRepository = amenityRepository;

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
            var startTime = request.Date.ToDateTime(request.StartTime, DateTimeKind.Utc);
            var endTime = request.Date.ToDateTime(request.EndTime, DateTimeKind.Utc);

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var hasOverlap = await _bookingRepository.HasOverlapAsync(request.HallId, startTime, endTime);
                if (hasOverlap)
                {
                    throw new BadRequestException("This hall is already booked for the selected time range.");
                }

                var selectedAmenities = new List<HallAmenity>();
                foreach (var amenityId in request.SelectedAmenityIds)
                {
                    var hallAmenity = await _amenityRepository.GetHallAmenityAsync(request.HallId, amenityId);
                    if (hallAmenity == null)
                    {
                        throw new BadRequestException($"Amenity with ID {amenityId} is not available for this hall.");
                    }
                    selectedAmenities.Add(hallAmenity);
                }

                var hallCost = _pricingCalculatorService.CalculateHallCost(hall.Price, startTime, endTime);
                var amenitiesCost = selectedAmenities.Sum(a => a.Price);
                var totalCost = hallCost + amenitiesCost;
                var bookingId = Guid.NewGuid();

                booking = new Booking
                {
                    Id = bookingId,
                    HallId = request.HallId,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = BookingStatus.Confirmed,
                    TotalCost = totalCost,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    SelectedAmenities = selectedAmenities.Select(a => new BookingAmenity
                    {
                        BookingId = bookingId,
                        AmenityId = a.AmenityId,
                        PriceAtBooking = a.Price
                    }).ToList()
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

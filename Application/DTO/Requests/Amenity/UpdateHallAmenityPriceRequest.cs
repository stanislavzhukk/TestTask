namespace Application.DTO.Requests.Amenity
{
    /// <summary>Request to change the price of an amenity already attached to a hall.</summary>
    public class UpdateHallAmenityPriceRequest
    {
        /// <summary>New price for the amenity in this hall.</summary>
        /// <example>550</example>
        public decimal Price { get; set; }
    }
}
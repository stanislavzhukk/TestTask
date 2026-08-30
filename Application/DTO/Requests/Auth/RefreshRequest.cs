namespace Application.DTO.Requests.Auth
{
    /// <summary>Request to obtain a new access token using a valid refresh token.</summary>
    public class RefreshRequest
    {
        public required string RefreshToken { get; set; }
    }
}
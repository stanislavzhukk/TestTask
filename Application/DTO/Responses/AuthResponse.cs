namespace Application.DTO.Responses
{
    /// <summary>Result of a successful login — access and refresh tokens plus the user's data.</summary>
    public class AuthResponse
    {
        /// <summary>JWT access token. Send as "Bearer {token}" in the Authorization header.</summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>Long-lived token used to obtain a new access token without re-authenticating.</summary>
        public string RefreshToken { get; set; } = string.Empty;

        public required UserDataResponse User { get; set; }
    }
}
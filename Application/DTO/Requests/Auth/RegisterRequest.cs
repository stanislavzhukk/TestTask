namespace Application.DTO.Requests.Auth
{
    /// <summary>Request to register a new user account.</summary>
    public class RegisterRequest
    {
        /// <example>user@example.com</example>
        public required string Email { get; set; }

        /// <example>John</example>
        public required string Name { get; set; }

        /// <example>Doe</example>
        public string? Surname { get; set; }

        public required string Password { get; set; }
    }
}
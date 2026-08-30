namespace Application.DTO.Responses
{
    /// <summary>
    /// Public data about a user.
    /// TODO: clarify — earlier UserDataResponse (in our very first auth-module draft) had a Role
    /// field ("User"/"Admin"), used for [Authorize(Roles = "Admin")] on hall management endpoints.
    /// This version doesn't have it — was Role intentionally dropped, or should it be added back here?
    /// </summary>
    public class UserDataResponse
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }
}
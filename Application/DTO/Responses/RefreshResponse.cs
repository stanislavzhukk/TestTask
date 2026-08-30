using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    /// <summary>
    /// Result of refreshing a session — a new access token and the user's data.
    /// TODO: clarify — no new RefreshToken here. Is the refresh token reused (same one
    /// stays valid) or rotated on each refresh? If rotated, this response is missing the new one.
    /// </summary>
    public class RefreshResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public required UserDataResponse User { get; set; }
    }
}
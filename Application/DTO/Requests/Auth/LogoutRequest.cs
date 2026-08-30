using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Requests.Auth
{
    /// <summary>Request to invalidate a refresh token, ending the user's session.</summary>
    public class LogoutRequest
    {
        /// <summary>The refresh token to invalidate.</summary>
        public required string RefreshToken { get; set; } = string.Empty;
    }
}
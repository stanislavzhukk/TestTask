using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Requests.Auth
{
    /// <summary>Credentials for authenticating an existing user.</summary>
    public class LoginRequest
    {
        /// <example>user@example.com</example>
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
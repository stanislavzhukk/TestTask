using Microsoft.AspNetCore.Mvc;
using Application.DTO.Requests.Auth;
using Application.DTO.Responses;
using Application.Interfaces;

namespace API.Controllers
{
    /// <summary>Registration, login, and session management endpoints.</summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Registers a new user account.</summary>
        /// <response code="409">A user with this email already exists.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            return Accepted(response);
        }

        /// <summary>Authenticates a user and returns an access/refresh token pair.</summary>
        /// <response code="401">Invalid email or password.</response>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        /// <summary>Exchanges a valid refresh token for a new access token.</summary>
        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshResponse>> Refresh([FromBody] RefreshRequest request)
        {
            var response = await _authService.RefreshAccessTokenAsync(request);
            return Ok(response);
        }

        /// <summary>Invalidates a refresh token, ending the session.</summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            await _authService.LogoutAsync(request);
            return Ok();
        }
    }
}
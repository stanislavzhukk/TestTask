using Application.DTO.Requests.Auth;
using Application.DTO.Responses;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly ITokenService _jwtService;
        private readonly IHashService _hashService;

        public AuthService(
            UserManager<User> userManager,
            IRefreshTokensRepository refreshTokens,
            ITokenService jwtService,
            IHashService hashService
            )
        {
            _userManager = userManager;
            _refreshTokensRepository = refreshTokens;
            _jwtService = jwtService;
            _hashService = hashService;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedException("Auth_InvalidCredentials");
            }

            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isValid)
            {
                throw new UnauthorizedException("Auth_InvalidCredentials");
            }

            if (user.EmailConfirmed != true)
            {
                throw new ForbiddenException("Auth_EmailNotConfirmed");
            }

            var accessToken = await _jwtService.GenerateAccessTokenAsync(user);

            var refreshTokenEntity = await _jwtService.GenerateRefreshTokenAsync(user);
            var rawRefreshToken = refreshTokenEntity.Token;
            refreshTokenEntity.Token = _hashService.ComputeHash(refreshTokenEntity.Token);

            await _refreshTokensRepository.AddRefreshTokenAsync(refreshTokenEntity);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = rawRefreshToken,
                User = new UserDataResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email!
                }
            };
        }

        public async Task<RefreshResponse> RefreshAccessTokenAsync(RefreshRequest request)
        {
            var hashedToken = _hashService.ComputeHash(request.RefreshToken);
            var tokenEntity = await _refreshTokensRepository.GetRefreshTokenAsync(hashedToken);
            if (tokenEntity == null)
            {
                throw new UnauthorizedException("Auth_InvalidRefreshToken");
            }

            if (!tokenEntity.IsActive)
            {
                if (tokenEntity.Revoked == null)
                {
                    tokenEntity.Revoked = DateTime.UtcNow;
                    await _refreshTokensRepository.UpdateAsync(tokenEntity);
                }
                throw new RefreshTokenException("Auth_ExpiredRefreshToken");
            }

            if (tokenEntity.Expires < DateTime.UtcNow.AddHours(12))
            {
                tokenEntity.Expires = DateTime.UtcNow.AddDays(7);
                await _refreshTokensRepository.UpdateAsync(tokenEntity);
            }

            var user = await _userManager.FindByIdAsync(tokenEntity.UserId.ToString());
            if (user == null)
            {
                throw new UnauthorizedException("User_NotFound");
            }

            var newAccessToken = await _jwtService.GenerateAccessTokenAsync(user);

            return new RefreshResponse
            {
                AccessToken = newAccessToken,
                User = new UserDataResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email!
                }
            };
        }

        public async Task LogoutAsync(LogoutRequest request)
        {
            var hashedToken = _hashService.ComputeHash(request.RefreshToken);
            var tokenEntity = await _refreshTokensRepository.GetRefreshTokenAsync(hashedToken);
            if (tokenEntity == null || !tokenEntity.IsActive)
            {
                throw new UnauthorizedException("Auth_InvalidRefreshToken");
            }

            await _refreshTokensRepository.RevokeTokenAsync(tokenEntity);
        }

        public async Task<UserDataResponse> RegisterAsync(RegisterRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                throw new BadRequestException("Auth_EmailIsAlreadyTaken");
            }

            var userId = Guid.NewGuid();

            var userRecord = new User
            {
                Id = userId,
                Name = request.Name,
                Surname = request.Surname ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow,
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                UserName = userId.ToString(),
                NormalizedUserName = userId.ToString().ToUpper(),
                EmailConfirmed = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var response = await _userManager.CreateAsync(userRecord, request.Password);
            if (!response.Succeeded)
            {
                var errors = response.Errors.Select(e => e.Code).ToList();

                throw new BadRequestException("", errors);
            }

            await _userManager.AddToRoleAsync(userRecord, "User");

            return new UserDataResponse
            {
                Id = userRecord.Id,
                Name = userRecord.Name,
                Email = userRecord.Email!,
                Surname = userRecord.Surname,
            };
        }
    }
}
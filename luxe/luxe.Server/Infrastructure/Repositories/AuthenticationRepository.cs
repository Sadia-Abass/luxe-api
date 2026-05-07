using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.AuthenticationDTOs;
using luxe.Server.Application.Repositories;
using luxe.Server.Application.Services;
using luxe.Server.Domain.Entities;
using luxe.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace luxe.Server.Infrastructure.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly HttpContext httpContext;

        public AuthenticationRepository(AppDbContext appDbContext, UserManager<AppUser> userManager, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            httpContext = httpContextAccessor.HttpContext;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<AppUser>> RegisterAsync(RegistrationRequestDTO registerRequestDto)
        {
            var user = new AppUser
            {
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.LastName,
                Email = registerRequestDto.Email,
                UserName = registerRequestDto.Email,
                Datejoined = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                LastLoginedInDate = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
            if (!result.Succeeded)
            {
                return new ApiResponse<AppUser>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = result.Errors.Select(e => e.Description).ToList(),
                    Data = null
                };
            }

            return new ApiResponse<AppUser>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                ErrorMessages = new List<string> { "User registered successfully." },
                Data = user
            };
        }

        public async Task<ApiResponse<TokenResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDto)
        {
            var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.Email == loginRequestDto.Email);
            //var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid email or password." },
                    Data = null 
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateAccessTokenAsync(user, roles);

            var refreshToken = _tokenService.CreateRefreshToken(GetIpAddress());

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new ApiResponse<TokenResponseDTO>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                ErrorMessages = new List<string>(),
                Data = new TokenResponseDTO { AccessToken = token, RefreshToken = refreshToken.Token }
            };
        }

        public async Task<ApiResponse<TokenResponseDTO>> RefreshTokenAsync(TokenRequestDTO tokenRequestDto)
        {
            var refreshToken = tokenRequestDto.RefreshToken;
            var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
            {
                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid refresh token." },
                    Data = null
                };
            }

            var existingRefreshToken = user.RefreshTokens.Single(t => t.Token == refreshToken);
            if (!existingRefreshToken.IsActive)
            {
                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Refresh token is no longer active." },
                    Data = null
                };
            }

            existingRefreshToken.RevokedDate = DateTime.UtcNow;
            existingRefreshToken.RevokedByIp = GetIpAddress();
            
            var newRefreshToken = _tokenService.CreateRefreshToken(GetIpAddress());
            existingRefreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(user);

            // Generate new access token
            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenService.CreateAccessTokenAsync(user, roles);

            return new ApiResponse<TokenResponseDTO>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                ErrorMessages = new List<string>(),
                Data = new TokenResponseDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken.Token }
            };
        }

        public async Task<ApiResponse<string>> RevokeTokenAsync(TokenRequestDTO tokenRequestDto)
        {
            var token = tokenRequestDto.RefreshToken;
            var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Token not found." },
                    Data = null
                };
            }

            var existingToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!existingToken.IsActive)
            {
                return new ApiResponse<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Token is already revoked." },
                    Data = null
                };
            }

            existingToken.RevokedDate = DateTime.UtcNow;
            existingToken.RevokedByIp = GetIpAddress();

            await _userManager.UpdateAsync(user);
            return new ApiResponse<string>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                ErrorMessages = new List<string>() { "Refresh token revoked successfully." },
                Data = null
            };
        }

        private string GetIpAddress()
        {
            if(httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return httpContext.Request.Headers["X-Forwarded-For"].ToString();
            }

            return httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }
}

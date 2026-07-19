using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.AuthenticationDTOs;
using luxe.Server.Application.Repositories;
using luxe.Server.Application.Services;
using luxe.Server.Domain.Entities;
using luxe.Server.Infrastructure.Configurations;
using luxe.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;

namespace luxe.Server.Infrastructure.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IFileUploaderService _fileUploaderService;
        private readonly IEmailService _emailService;
        private readonly ClientSettings _clientSettings;

        public AuthenticationRepository(AppDbContext appDbContext, UserManager<AppUser> userManager, ITokenService tokenService, IUserRepository userRepository, IFileUploaderService fileUploaderService, IEmailService emailService, IOptions<ClientSettings> clientSettings)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;      
            _tokenService = tokenService;
            _userRepository = userRepository;
            _fileUploaderService = fileUploaderService;
            _emailService = emailService;
            _clientSettings = clientSettings.Value;
        }

        public async Task<ApiResponse<TokenResponseDTO>> RegisterAsync(RegistrationRequestDTO registerRequestDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerRequestDto.Email);
                if (existingUser != null)
                {
                    return new ApiResponse<TokenResponseDTO>
                    {
                        StatusCode = HttpStatusCode.Conflict,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "A user with this email already exists." },
                        Data = null
                    };
                }

                var imageUrl = await _fileUploaderService.UploadFileAsync(registerRequestDto.File, "profile-images");

                var user = new AppUser
                {
                    FirstName = registerRequestDto.FirstName,
                    LastName = registerRequestDto.LastName,
                    Email = registerRequestDto.Email,
                    UserName = registerRequestDto.Email,
                    ImageUrl = imageUrl.SecureUrl.ToString(),
                    Datejoined = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, registerRequestDto?.Password ?? string.Empty);

                if (!result.Succeeded)
                {
                    return new ApiResponse<TokenResponseDTO>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = result.Errors.Select(e => e.Description).ToList(),
                        Data = null
                    };
                }

                await _userManager.AddToRoleAsync(user, "Customer");

                //var tokenResponse = _tokenService.CreateAccessTokenAsync(user, new List<string> { "Customer" });

                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedEmailToken = Uri.EscapeDataString(emailToken);

                var confirmationLink = $"{_clientSettings.BaseUrl}/confirm-email?userId={user.Id}&token={encodedEmailToken}";

                var emailBody = $"<h1>Welcome, {user.FirstName}!</h1>" +
                                $"<p>Thank you for registering with us. Please confirm your email address by clicking the link below:</p>" +
                                $"<p><a href='{confirmationLink}'>Confirm Email</a></p>" +
                                $"<p>If you did not register, please ignore this email.</p>" +
                                $"<p>Best regards,<br/>The Luxe Team</p>";

                await _emailService.SendEmailAsync(user.Email, "Confirm Your Email", emailBody);

                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    ErrorMessages = new List<string> { "Registration successful. Please check your email to confirm your account." },
                    Data = null
                };
            }
            catch (Exception ex) 
            {
                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An error occurred during registration.", ex.Message },
                    Data = null
                };
            }
            
        }

        // Implementation for login logic
        // This method should validate the user's credentials, generate a token, and return it in the response.
        public async Task<ApiResponse<TokenResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDto)
        {
            //Check if the user exists
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
            if (user == null)
            {
                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid email or password." },
                    Data = null 
                };
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "User account is locked due to multiple failed attempts. Try again later." },
                    Data = null
                };
            }

            if(!await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                // increments the lockout counter
                await _userManager.AccessFailedAsync(user);
                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid email or password." },
                    Data = null
                };
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Email is not confirmed. Please confirm your email before logging in." },
                    Data = null
                };
            }

            // successful login clears the failed login counter
            await _userManager.ResetAccessFailedCountAsync(user);


            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenService.CreateAccessTokenAsync(user, roles);
            var refreshToken = _tokenService.CreateRefreshToken(user.Id);


            user.RefreshTokens.Add(refreshToken);
            await _userRepository.SaveRefreshTokenAsync(refreshToken);

            return new ApiResponse<TokenResponseDTO>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                ErrorMessages = new List<string>(),
                Data = new TokenResponseDTO 
                { 
                    AccessToken = accessToken, 
                    RefreshToken = refreshToken.Token ?? string.Empty, 
                    AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15) 
                }
            };
        }

        public async Task<ApiResponse<TokenResponseDTO>> RefreshTokenAsync(TokenRequestDTO tokenRequestDto)
        {
            ClaimsPrincipal principal;

            try
            {
                principal = _tokenService.GetPrincipalFromExpiredToken(tokenRequestDto.AccessToken ?? string.Empty)!;
            }
            catch (SecurityTokenException)
            {
                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid access token." },
                    Data = null
                };
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var storedRefreshToken = await _userRepository.GetRefreshTokenAsync(tokenRequestDto.RefreshToken ?? string.Empty);


            if (storedRefreshToken == null || storedRefreshToken.UserId != userId)
            {
                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid refresh token." },
                    Data = null
                };
            }

           

            if (!storedRefreshToken.IsActive)
            {
                if (tokenRequestDto.RefreshToken != null) 
                {
                    await RevokeAllUserTokenAsync(storedRefreshToken.UserId);
                }

                return new ApiResponse<TokenResponseDTO>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Refresh token is no longer valid. Please log in again." },
                    Data = null
                };
            }

            var user = storedRefreshToken.User;
            var roles = await _userManager.GetRolesAsync(user);

            var newRefreshToken = _tokenService.CreateRefreshToken(user.Id);
            await _userRepository.RevokeRefreshTokenAsync(storedRefreshToken, newRefreshToken.Token);
            await _userRepository.SaveRefreshTokenAsync(newRefreshToken);

            var newAccessToken = _tokenService.CreateAccessTokenAsync(user, roles);

            return new ApiResponse<TokenResponseDTO>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                ErrorMessages = new List<string>(),
                Data = new TokenResponseDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken.Token, AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15) }
            };
        }

        public async Task<ApiResponse<string>> RevokeTokenAsync(RevokeTokenDTO revokeTokenDto)
        {
            var storedRefreshToken = await _userRepository.GetRefreshTokenAsync(revokeTokenDto.RefreshToken);
            var user = await _userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == storedRefreshToken.Token));


            if(storedRefreshToken == null)
            {
                return new ApiResponse<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Refresh token not found." },
                    Data = null
                };
            }

            var currentUserId = user?.Id;
            if (storedRefreshToken.UserId != currentUserId)
            {
                return new ApiResponse<string>
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "You are not authorized to revoke this token." },
                    Data = null
                };
            }

            if(!storedRefreshToken.IsActive)
            {
                return new ApiResponse<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Refresh token is already inactive." },
                    Data = null
                };
            }

            await _userRepository.RevokeRefreshTokenAsync(storedRefreshToken);

            return new ApiResponse<string>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                ErrorMessages = new List<string> { "Logged out successfully." },
                Data = null
            };        
        }


        private async Task RevokeAllUserTokenAsync(string userId)
        {
            var user = await _userRepository.GetUserWithRefreshTokenAsync(userId);
            if (user == null) return;

            
            foreach (var token in user.RefreshTokens.Where(t => t.IsActive))
            {
                await _userRepository.RevokeRefreshTokenAsync(token);
            }
        }

        public async Task<ApiResponse<string>> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return new ApiResponse<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "User not found." },
                    Data = null
                };
            }

            var decodedToke = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToke);

            if(!result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Email confirmation failed. The link may have expired." },
                    Data = null
                };
            }

            return new ApiResponse<string>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                ErrorMessages = new List<string> { "Email confirmed successfully. You can now log in." },
                Data = null
            };
        }



        //private string GetIpAddress()
        //{
        //    if(_httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        //    {
        //        return _httpContext.Request.Headers["X-Forwarded-For"].ToString();
        //    }

        //    return _httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        //}


        //var existingRefreshToken = user.RefreshTokens.Single(t => t.Token == refreshToken);
        //if (!existingRefreshToken.IsActive)
        //{
        //    return new ApiResponse<TokenResponseDTO>
        //    {
        //        StatusCode = System.Net.HttpStatusCode.Unauthorized,
        //        IsSuccess = false,
        //        ErrorMessages = new List<string> { "Refresh token is no longer active." },
        //        Data = null
        //    };
        //}

        // existingRefreshToken.RevokedDate = DateTime.UtcNow;
        //existingRefreshToken.RevokedByIp = GetIpAddress();


        //existingRefreshToken.ReplacedByToken = newRefreshToken.Token;
        // user.RefreshTokens.Add(newRefreshToken);

        // Generate new access token
        // var roles = await _userManager.GetRolesAsync(user);
        //var newAccessToken = _tokenService.CreateAccessTokenAsync(user, roles);

        //await _userManager.UpdateAsync(user);

        //existingToken.RevokedByIp = GetIpAddress();

        //private readonly HttpContext _httpContext; , IHttpContextAccessor httpContextAccessor    //_httpContext = httpContextAccessor.HttpContext;

        //var refreshToken = tokenRequestDto.RefreshToken;
        //var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));


        //var token = revokeTokenDto.RefreshToken;
        //var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
        //if (user == null)
        //{
        //    return new ApiResponse<string>
        //    {
        //        StatusCode = System.Net.HttpStatusCode.NotFound,
        //        IsSuccess = false,
        //        ErrorMessages = new List<string> { "Token not found." },
        //        Data = null
        //    };
        //}

        //var existingToken = user.RefreshTokens.Single(t => t.Token == token);
        //if (!existingToken.IsActive)
        //{
        //    return new ApiResponse<string>
        //    {
        //        StatusCode = System.Net.HttpStatusCode.BadRequest,
        //        IsSuccess = false,
        //        ErrorMessages = new List<string> { "Token is already revoked." },
        //        Data = null
        //    };
        //}

        //existingToken.RevokedDate = DateTime.UtcNow;


        //await _userManager.UpdateAsync(user);
        //return new ApiResponse<string>
        //{
        //    StatusCode = System.Net.HttpStatusCode.OK,
        //    IsSuccess = true,
        //    ErrorMessages = new List<string>() { "Refresh token revoked successfully." },
        //    Data = null
        //};
    }
}

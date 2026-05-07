using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.AuthenticationDTOs;
using luxe.Server.Domain.Entities;

namespace luxe.Server.Application.Services
{
    public interface ITokenService
    {
        // Task<ApiResponse<>> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
        Task<string> CreateAccessTokenAsync(AppUser user, IList<string> roles);
        Task<RefreshToken> CreateRefreshToken(string ipAddress);   
    }
}

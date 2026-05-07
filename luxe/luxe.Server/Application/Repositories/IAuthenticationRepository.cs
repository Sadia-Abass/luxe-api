using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.AuthenticationDTOs;
using luxe.Server.Domain.Entities;

namespace luxe.Server.Application.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<ApiResponse<AppUser>> RegisterAsync(RegistrationRequestDTO registerRequestDto);
        Task<ApiResponse<TokenResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDto);
        Task<ApiResponse<TokenResponseDTO>> RefreshTokenAsync(TokenRequestDTO tokenRequestDto);
        Task<ApiResponse<string>> RevokeTokenAsync(TokenRequestDTO tokenRequestDto);
    }
}

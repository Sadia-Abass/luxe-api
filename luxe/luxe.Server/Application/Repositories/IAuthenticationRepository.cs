using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.AuthenticationDTOs;
using luxe.Server.Domain.Entities;

namespace luxe.Server.Application.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<ApiResponse<TokenResponseDTO>> RegisterAsync(RegistrationRequestDTO registerRequestDto);
        Task<ApiResponse<TokenResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDto);
        Task<ApiResponse<TokenResponseDTO>> RefreshTokenAsync(TokenRequestDTO tokenRequestDto);
        Task<ApiResponse<string>> RevokeTokenAsync(RevokeTokenDTO revokeTokenDto);
        Task<ApiResponse<string>> ConfirmEmail(string userId, string token);
        Task<ApiResponse<string>> ForgotPassword(ForgotPasswordDTO forgotPasswordDto);
        Task<ApiResponse<string>> ResetPassword(ResetPasswordDTO resetPasswordDto);
    }
}

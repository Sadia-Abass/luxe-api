using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.AuthenticationDTOs;

namespace luxe.Server.Application.Services
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<>> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
    }
}

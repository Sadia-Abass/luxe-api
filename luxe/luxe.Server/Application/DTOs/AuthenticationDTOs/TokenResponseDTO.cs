using luxe.Server.Domain.Entities;

namespace luxe.Server.Application.DTOs.AuthenticationDTOs
{
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public AppUserDTO User { get; set; } = new AppUserDTO();
    }
}

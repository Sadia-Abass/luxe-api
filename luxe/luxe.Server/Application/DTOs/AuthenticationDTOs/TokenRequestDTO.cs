namespace luxe.Server.Application.DTOs.AuthenticationDTOs
{
    public class TokenRequestDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}

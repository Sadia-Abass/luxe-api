namespace luxe.Server.Application.DTOs.AuthenticationDTOs
{
    public class ResetPasswordDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}

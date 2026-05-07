namespace luxe.Server.Infrastructure.Configurations
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int MinutesToExpiration { get; set; }
        public double RefreshTokenExpirationDays { get; set; }
    }
}

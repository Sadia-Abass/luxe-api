namespace luxe.Server.Infrastructure.Configurations
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int MinutesToExpiration { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}

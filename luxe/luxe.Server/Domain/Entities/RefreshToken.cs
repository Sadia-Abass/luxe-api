namespace luxe.Server.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByIp { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => RevokedDate == null && !IsExpired;

        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;

    }
}

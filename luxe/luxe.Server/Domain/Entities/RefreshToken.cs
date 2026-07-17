namespace luxe.Server.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedDate { get; set; }
        public string? ReplacedByToken { get; set; }

        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;

        // Helper properties - computed, not stored in DB
        public bool IsExpired => DateTime.UtcNow >= ExpiresDate;
        public bool IsActive => RevokedDate == null && !IsExpired;
    }
}

namespace luxe.Server.Application.DTOs.AuthenticationDTOs
{
    public class RoleDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}

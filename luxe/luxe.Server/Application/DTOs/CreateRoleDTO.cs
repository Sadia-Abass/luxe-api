namespace luxe.Server.Application.DTOs
{
    public class CreateRoleDTO
    {
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}

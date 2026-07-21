using luxe.Server.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace luxe.Server.Application.DTOs.Users
{
    public class UserResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public DateTime Datejoined { get; set; }
        public bool IsActive { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}

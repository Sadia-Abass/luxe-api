using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace luxe.Server.Application.DTOs.Users
{
    public class AddUserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public IFormFile ImageUrl { get; set; } = null!;
    }
}

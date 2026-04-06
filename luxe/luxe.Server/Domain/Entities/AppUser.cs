using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace luxe.Server.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        [PersonalData]
        [StringLength(100)]
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [PersonalData]
        [StringLength(100)]
        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [PersonalData]
        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [PersonalData]
        [Display(Name = "Profile Picture URL")]
        public string ImageUrl { get; set; } = string.Empty;

        public DateTime Datejoined { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime LastLoginedInDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}

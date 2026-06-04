using System.ComponentModel.DataAnnotations;

namespace luxe.Server.Application.DTOs.Category
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}

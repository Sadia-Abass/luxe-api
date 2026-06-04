using System.ComponentModel.DataAnnotations;

namespace luxe.Server.Application.DTOs.Subcategory
{
    public class CreateSubcategoryDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}

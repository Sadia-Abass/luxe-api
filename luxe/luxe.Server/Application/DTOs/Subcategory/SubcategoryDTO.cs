using System.ComponentModel.DataAnnotations;

namespace luxe.Server.Application.DTOs.Subcategory
{
    public class SubcategoryDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}

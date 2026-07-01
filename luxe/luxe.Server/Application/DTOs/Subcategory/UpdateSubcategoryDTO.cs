using System.ComponentModel.DataAnnotations;

namespace luxe.Server.Application.DTOs.Subcategory
{
    public class UpdateSubcategoryDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}

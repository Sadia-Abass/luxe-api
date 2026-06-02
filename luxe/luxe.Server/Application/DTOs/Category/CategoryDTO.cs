using System.ComponentModel.DataAnnotations;

namespace luxe.Server.Application.DTOs.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}

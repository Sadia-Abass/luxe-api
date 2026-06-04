using System.ComponentModel.DataAnnotations;

namespace luxe.Server.Domain.Entities
{
    public class Category: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Subcategory> Subcategory { get; set; } = new List<Subcategory>();
    }
}

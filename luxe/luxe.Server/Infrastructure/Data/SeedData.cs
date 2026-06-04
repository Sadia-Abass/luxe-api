using luxe.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace luxe.Server.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Makeup", CreatedDate = new DateTime(2026,6,4) },
                new Category { Id = 2, Name = "Skin", CreatedDate = new DateTime(2026,6,4) },
                new Category { Id = 3, Name = "Hair", CreatedDate = new DateTime(2026,6,4) },
                new Category { Id = 4, Name = "Perfume", CreatedDate = new DateTime(2026,6,4) },
                new Category { Id = 5, Name = "Toiletries", CreatedDate = new DateTime(2026,6,4) },
                new Category { Id = 6, Name = "Men's", CreatedDate = new DateTime(2026,6,4) },
                new Category { Id = 7, Name = "Baby & Children", CreatedDate = new DateTime(2026,6,4) },
                new Category { Id = 8, Name = "Wellbeing", CreatedDate = new DateTime(2026,6,4) }
            );

            modelBuilder.Entity<Subcategory>().HasData(
                new Subcategory { Id = 1, Name = "Face", CategoryId = 1, CreatedDate = new DateTime(2026,6,4) },
                new Subcategory { Id = 2, Name = "Lips", CategoryId = 1, CreatedDate = new DateTime(2026,6,4) },
                new Subcategory { Id = 3, Name = "Eyes", CategoryId = 1, CreatedDate = new DateTime(2026,6,4) },
                new Subcategory { Id = 4, Name = "Nail", CategoryId = 1, CreatedDate = new DateTime(2026,6,4) },
                new Subcategory { Id = 5, Name = "Brushes", CategoryId = 1, CreatedDate = new DateTime(2026,6,4) },
                new Subcategory { Id = 6, Name = "Tools & Accessories", CategoryId = 1, CreatedDate = new DateTime(2026,6,4) },
                new Subcategory { Id = 7, Name = "Makeup Remover", CategoryId = 1, CreatedDate = new DateTime(2026,6,4) },
                new Subcategory { Id = 8, Name = "Suncare", CategoryId = 2, CreatedDate = new DateTime(2026,6,4) }
            );
        }
    }
        
}

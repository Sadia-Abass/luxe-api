using luxe.Server.Domain.Entities;
using luxe.Server.Infrastructure.Data;
using luxe.Server.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace luxe.tests.Infrastructure.Repository
{
    public class SubcategoryRepositoryTests
    {
        private AppDbContext GetInMemoryDbCntext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            context.Subcategory.AddRange(
                new Subcategory { Id = 1, Name = "Face", CategoryId = 1, Category = new Category { Name = "Makeup" } },
                new Subcategory { Id = 2, Name = "Lips", CategoryId = 1, Category = new Category { Name = "Makeup" } },
                new Subcategory { Id = 3, Name = "Eyes", CategoryId = 1, Category = new Category { Name = "Makeup" } },
                new Subcategory { Id = 4, Name = "Nails", CategoryId = 1, Category = new Category { Name = "Makeup" } },
                new Subcategory { Id = 5, Name = "Hair", CategoryId = 1, Category = new Category { Name = "Makeup" } }
                );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetAllSubcategoriesAsync_ReturnsAllSubcategories()
        {
            // Arrange
            var context = GetInMemoryDbCntext();
            var repository = new SubcategoryRepostory(context);
            // Act
            var result = await repository.GetAllSubcategoriesAsync();
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(5, result.Data.Count());
        }

        [Fact]
        public async Task GetAllSubcategoriesAsync_ReturnsEmptyList_WhenNoSubcategiesFound()
        {
            // Arrage
            var context = GetInMemoryDbCntext();

            context.Subcategory.RemoveRange(context.Subcategory);
            context.SaveChanges();
            var repository = new SubcategoryRepostory(context);

            // Act
            var result = await repository.GetAllSubcategoriesAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("No subcategories found.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task GetSubcategoryByIdAsync_ReturnsSubcategory_WhenSubcategoryExists()
        {
            // Arrange
            var context = GetInMemoryDbCntext();
            var repository = new SubcategoryRepostory(context);
            int subcategoryId = 1;
            // Act
            var result = await repository.GetSubcategoryByIdAsync(subcategoryId);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(subcategoryId, result.Data.Id);
            Assert.Equal("Face", result.Data.Name);
            Assert.Equal("Makeup", result.Data.CategoryName);
        }

        [Fact]
        public async Task GetSubcategoryByIdAsync_ReturnsError_WhenSubcategoryDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbCntext();
            var repository = new SubcategoryRepostory(context);
            int subcategoryId = 999; // Non-existent subcategory ID
            // Act
            var result = await repository.GetSubcategoryByIdAsync(subcategoryId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal($"Subcategory with ID {subcategoryId} not found.", result.ErrorMessages.FirstOrDefault());
        }


    }
}

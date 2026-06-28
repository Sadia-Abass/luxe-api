using luxe.Server.Application.DTOs.Category;
using luxe.Server.Domain.Entities;
using luxe.Server.Infrastructure.Data;
using luxe.Server.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace luxe.tests.Infrastructure.Repository
{
    public class CategoryRepositoryTests
    {
        // Helper method that creates a fresh, isolated ApplicationDbContext using EF Core InMemory provider
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database name for isolation
                .Options;

            // Create a new ApplicationDbContext instance with the above options
            var context = new AppDbContext(options);

            // Seed initial product data into the in-memory database for testing
            context.Category.AddRange(
                new Category { Id = 1, Name = "Makeup" },
                new Category { Id = 2, Name = "Skin" },
                new Category { Id = 3, Name = "Hair" },
                new Category { Id = 4, Name = "Perfume" }
                );

            // Persist seeded data immediately to the in-memory store
            context.SaveChanges();

            // Return the prepared in-memory context for use in test
            return context;
        }

        // Test method to verify GetCategoryByIdAsync returns a category when the category exists in the database
        [Fact]
        public async Task GetCategoryByIdAsync_ReturnsCategory_WhenCategoryExists()
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test with an existing category ID
            var category = await repository.GetCategoryByIdAsync(1);

            // Assert: Verify that the result is successful and contains the expected category data
            Assert.True(category.IsSuccess);
            Assert.Equal(1, category.Data.Id);
            Assert.Equal("Makeup", category.Data.Name);
        }

        // Test method to verify GetCategoryByIdAsync returns null when the category does not exist
        [Fact]
        public async Task GetCategoryByIdAsync_ReturnsNull_WhenCategoryDoesNotExist()
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test with a non-existing category ID
            var category = await repository.GetCategoryByIdAsync(99);

            // Assert: Verify that the result is not successful and contains null data
            Assert.False(category.IsSuccess);
            //Assert.Equal(0, category.Data.Id);
            Assert.Null(category.Data);
        }

        // Test method to verify GetAllCategoriesAsync returns all categories
        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsAllCategories()
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test to retrieve all categories
            var categories = await repository.GetAllCategoriesAsync();

            // Assert: Verify that the result is successful and contains the expected number of categories
            Assert.True(categories.IsSuccess);
            Assert.Equal(4, categories.Data.Count());
        }

        // Test method to verify GetAllCategoriesAsync returns an empty list when there are no categories
        [Theory]
        [InlineData(0)]
        public async Task GetAllCategoriesAsync_ReturnsEmptyList_WhenNoCategories(int categoryCount)
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();

            // Remove all categories from the in-memory database to simulate no categories
            context.Category.RemoveRange(context.Category);
            context.SaveChanges();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test to retrieve all categories
            var categories = await repository.GetAllCategoriesAsync();

            // Assert: Verify that the result is successful and contains an empty list
            Assert.True(categories.IsSuccess);
            Assert.Equal(categoryCount, categories.Data.Count());
            Assert.Empty(categories.Data);
        }

        [Fact]
        public async Task CreateCategoryAsync_CategoryIsCreatedSuccessfully()
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test to create a new category
            var newCategory = new CreateCategoryDTO { Name = "New Category" };
            await repository.CreateCategoryAsync(newCategory);

            // Assert: Verify that the category was created successfully in the database
            var createdCategory = await context.Category.FirstOrDefaultAsync(c => c.Name == "New Category");
            Assert.NotNull(createdCategory);
            Assert.Equal("New Category", createdCategory.Name);
        }

        [Fact]
        public async Task CreateCategoryAsync_CategoryWithDuplates_ReturnsBadRequest()
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test to create a new category with a duplicate name
            var newCategory = new CreateCategoryDTO { Name = "Makeup" };
            var result = await repository.CreateCategoryAsync(newCategory);

            // Assert: Verify that the result indicates failure due to duplicate category name
            Assert.False(result.IsSuccess);
            Assert.Equal($"Category with the name '{newCategory.Name}' already exists.", result.ErrorMessages.First());
        }

        [Fact]
        public async Task CreateCategoryAsync_CategoryWithEmptyName_ReturnsBadRequest()
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test to create a new category with an empty name
            var newCategory = new CreateCategoryDTO { Name = "" };
            var result = await repository.CreateCategoryAsync(newCategory);

            // Assert: Verify that the result indicates failure due to empty category name
            Assert.False(result.IsSuccess);
            Assert.Equal("Category name cannot be empty.", result.ErrorMessages.First());
        }

        [Fact]
        public async Task UpdateCategoryAsync_CategoryIsUpdatedSuccessfully()
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test to update an existing category
            var updatedCategory = new UpdateCategoryDTO { Id = 1, Name = "Updated Makeup" };
            var result = await repository.UpdateCategoryAsync(updatedCategory);

            // Assert: Verify that the category was updated successfully in the database
            Assert.True(result.IsSuccess);
            var categoryInDb = await context.Category.FindAsync(1);
            Assert.Equal("Updated Makeup", categoryInDb.Name);
        }

        [Fact]
        public async Task UpdateCategoryAsync_CategoryDoesNotExist_ReturnsNotFound()
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test to update a non-existing category
            var updatedCategory = new UpdateCategoryDTO { Id = 99, Name = "Non-Existent Category" };
            var result = await repository.UpdateCategoryAsync(updatedCategory);

            // Assert: Verify that the result indicates failure due to category not found
            Assert.False(result.IsSuccess);
            Assert.Equal($"Category with id '{updatedCategory.Id}' not found.", result.ErrorMessages.First());
        }

        public async Task UpdateCategoryAsync_CategoryWithDuplicateName_ReturnsBadRequest()
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test to create a new category with a duplicate name
            var newCategory = new CreateCategoryDTO { Name = "Makeup" };
            var result = await repository.CreateCategoryAsync(newCategory);

            // Assert: Verify that the result indicates failure due to duplicate category name
            Assert.False(result.IsSuccess);
            Assert.Equal($"Category with the name '{newCategory.Name}' already exists.", result.ErrorMessages.First());
        }

        [Fact]
        public async Task DeleteCategoryAsync_CategoryIsDeletedSuccessfully()
        {
            // Arrange: Create a fresh in-memory context and repository for testing
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act: Call the method under test to delete an existing category
            var deleteCategory = new Category{ Id = 1};
            var result = await repository.DeleteCategoryAsync(deleteCategory.Id);

            // Assert: Verify that the category was deleted successfully from the database
            Assert.True(result.IsSuccess);
            var categoryInDb = await context.Category.FindAsync(1);
            Assert.True(categoryInDb.IsDeleted);
        }
    }
}

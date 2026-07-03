using Moq;
using Xunit;
using FluentAssertions;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using luxe.Server.API.Controllers;
using luxe.Server.Application.DTOs;
using luxe.Server.Infrastructure.Repositories;
using luxe.Server.Application.DTOs.Category;
using luxe.Server.Application.Repositories;

namespace luxe.tests.API.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CategoryController _categoryController;
        private readonly Fixture _fixture;

        public CategoryControllerTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _categoryController = new CategoryController(_mockCategoryRepository.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetCategoryById_ReturnsOkResult_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;
            var categoryDTO = new CategoryDTO { Id = categoryId, Name = "Test Category" };
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(categoryId))
                .ReturnsAsync(new ApiResponse<CategoryDTO> { Data = categoryDTO });

            // Act
            var result = await _categoryController.GetCategoryById(categoryId);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            //Assert.Equal(categoryDTO, okResult.Value);
            var apiResponse = Assert.IsType<ApiResponse<CategoryDTO>>(okResult.Value);
            Assert.Equal(categoryDTO.Id, apiResponse.Data.Id);
            Assert.Equal(categoryDTO.Name, apiResponse.Data.Name);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange: setup the mock to return null for any integer argument (simulate not found)
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((ApiResponse<CategoryDTO>?)null);

            // Act: call the GetCategoryById method with an ID that does not exist
            var result = await _categoryController.GetCategoryById(90);

            // Assert: check that the result is a NotFoundResult (HTTP 404)
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult_WhenCategoriesExist()
        {
            // Arrange
            var categories = new List<CategoryDTO>
            {
                new CategoryDTO { Id = 1, Name = "Makeup" },
                new CategoryDTO { Id = 2, Name = "Skincare" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetAllCategoriesAsync())
                .ReturnsAsync(new ApiResponse<IEnumerable<CategoryDTO>> { Data = categories });

            // Act
            var result = await _categoryController.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<CategoryDTO>>>(okResult.Value);
            Assert.Equal(categories.Count, apiResponse.Data.Count());
        }

        [Fact]
        public async Task GetAllCategories_ReturnsNotFound_WhenNoCategoriesExist()
        {
            // Arrange: setup the mock to return null (simulate no categories found)
            _mockCategoryRepository.Setup(repo => repo.GetAllCategoriesAsync())
                .ReturnsAsync((ApiResponse<IEnumerable<CategoryDTO>>?)null);

            // Act: call the GetAllCategories method
            var result = await _categoryController.GetAllCategories();

            // Assert: check that the result is a NotFoundResult (HTTP 404)
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCategory_ReturnsOkResult_WhenCategoryIsCreated()
        {
            // Arrange
            var createCategoryDTO = new CreateCategoryDTO { Name = "Hair Care" };
            var createdCategoryResponse = new ApiResponse<CategoryDTO> { Data = new CategoryDTO { Id = 1, Name = createCategoryDTO.Name } };
            _mockCategoryRepository.Setup(repo => repo.CreateCategoryAsync(createCategoryDTO))
                .ReturnsAsync(createdCategoryResponse);

            // Act
            var result = await _categoryController.CreateCategory(createCategoryDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<CategoryDTO>>(okResult.Value);
            Assert.Equal(createCategoryDTO.Name, apiResponse.Data.Name);
        }

        [Fact]
        public async Task CreateCategory_ReturnsBadRequest_WhenCategoryCreationFails()
        {
            // Arrange: setup the mock to return an ApiResponse with error messages (simulate creation failure)
            _categoryController.ModelState.AddModelError("Name", "The Name field is required.");

            // Act: call the CreateCategory method
            var result = await _categoryController.CreateCategory(new CreateCategoryDTO());

            // Assert: check that the result is a BadRequestObjectResult (HTTP 400)
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);

        }

        [Fact]
        public async Task UpdateCategory_ReturnsOkResult_WhenCategoryIsUpdated()
        {
            // Arrange
            var updateCategoryDTO = new UpdateCategoryDTO { Id = 1, Name = "Updated Category" };
            var updatedCategoryResponse = new ApiResponse<CategoryDTO> { Data = new CategoryDTO { Id = updateCategoryDTO.Id, Name = updateCategoryDTO.Name } };
            _mockCategoryRepository.Setup(repo => repo.UpdateCategoryAsync(updateCategoryDTO))
                .ReturnsAsync(updatedCategoryResponse);
            // Act
            var result = await _categoryController.UpdateCategory(updateCategoryDTO);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<CategoryDTO>>(okResult.Value);
            Assert.Equal(updateCategoryDTO.Name, apiResponse.Data.Name);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange: setup the mock to return null (simulate category not found)
            var updateCategoryDTO = new UpdateCategoryDTO { Id = 99, Name = "Non-existent Category" };
            _mockCategoryRepository.Setup(repo => repo.UpdateCategoryAsync(updateCategoryDTO))
                .ReturnsAsync((ApiResponse<CategoryDTO>?)null);
            // Act: call the UpdateCategory method
            var result = await _categoryController.UpdateCategory(updateCategoryDTO);
            // Assert: check that the result is a NotFoundResult (HTTP 404)
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsOkResult_WhenCategoryIsDeleted()
        {
            // Arrange
            int categoryId = 1;
            var deletedCategoryResponse = new ApiResponse<bool> { IsSuccess = true };
            _mockCategoryRepository.Setup(repo => repo.DeleteCategoryAsync(categoryId))
                .ReturnsAsync(deletedCategoryResponse);
            // Act
            var result = await _categoryController.DeleteCategory(categoryId);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(apiResponse.IsSuccess);
        }
    }
}

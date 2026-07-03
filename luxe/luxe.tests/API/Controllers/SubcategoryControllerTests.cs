using AutoFixture;
using luxe.Server.API.Controllers;
using luxe.Server.Application.Repositories;
using luxe.Server.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using luxe.Server.Domain.Entities;
using luxe.Server.Application.DTOs.Subcategory;
using luxe.Server.Application.DTOs;


namespace luxe.tests.API.Controllers
{
    public class SubcategoryControllerTests
    {
       // private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ISubcategoryRepository> _mockSubcategoryRepository;
        private readonly SubcategoryController _subcategoryController;
        private readonly Fixture _fixture;

        public SubcategoryControllerTests()
        {
           // _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mockSubcategoryRepository = new Mock<ISubcategoryRepository>();
            _subcategoryController = new SubcategoryController(_mockSubcategoryRepository.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetSubcategoryById_ReturnsOkResult_WhenSubcategoryExists()
        {
            // Arrange
            var subcategoryId = 1;
            var expectedSubcategory = _fixture.Create<SubcategoryDTO>();
            _mockSubcategoryRepository.Setup(repo => repo.GetSubcategoryByIdAsync(subcategoryId))
                .ReturnsAsync(new ApiResponse<SubcategoryDTO> { IsSuccess = true, Data = expectedSubcategory });
            // Act
            var result = await _subcategoryController.GetSubcategoryById(subcategoryId);
            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(new ApiResponse<SubcategoryDTO> { IsSuccess = true, Data = expectedSubcategory });
        }

        [Fact]
        public async Task GetSubcategoryById_ReturnsNotFoundResult_WhenSubcategoryDoesNotExist()
        {
            // Arrange
            var subcategoryId = 1;
            _mockSubcategoryRepository.Setup(repo => repo.GetSubcategoryByIdAsync(subcategoryId))
                .ReturnsAsync((ApiResponse<SubcategoryDTO>)null);
            // Act
            var result = await _subcategoryController.GetSubcategoryById(subcategoryId);
            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetAllSubcategories_ReturnsOkResult_WhenSubcategoriesExist()
        {
            // Arrange
            var expectedSubcategories = _fixture.CreateMany<SubcategoryDTO>(3);
            _mockSubcategoryRepository.Setup(repo => repo.GetAllSubcategoriesAsync())
                .ReturnsAsync(new ApiResponse<IEnumerable<SubcategoryDTO>> { IsSuccess = true, Data = expectedSubcategories });
            // Act
            var result = await _subcategoryController.GetAllSubcategories();
            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(new ApiResponse<IEnumerable<SubcategoryDTO>> { IsSuccess = true, Data = expectedSubcategories });
        }

        [Fact]
        public async Task GetAllSubcategories_ReturnsNotFoundResult_WhenNoSubcategoriesExist()
        {
            // Arrange
            _mockSubcategoryRepository.Setup(repo => repo.GetAllSubcategoriesAsync())
                .ReturnsAsync((ApiResponse<IEnumerable<SubcategoryDTO>>)null);
            // Act
            var result = await _subcategoryController.GetAllSubcategories();
            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task CreateSubcategory_ReturnsOK_Result_WhenSubcategoryIsCreated()
        {
            // Arrange
            var newSubcategory = _fixture.Create<CreateSubcategoryDTO>();
            var createdSubcategory = _fixture.Create<SubcategoryDTO>();
            _mockSubcategoryRepository.Setup(repo => repo.CreateSubcategoryAsync(newSubcategory))
                .ReturnsAsync(new ApiResponse<SubcategoryDTO> { IsSuccess = true, Data = createdSubcategory });
            // Act
            var result = await _subcategoryController.CreateSubcategory(newSubcategory);
            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(new ApiResponse<SubcategoryDTO> { IsSuccess = true, Data = createdSubcategory });
        }

        [Fact]
        public async Task CreateSubcategory_ReturnsBadRequestResult_WhenInvalidDataIsProvided()
        {
            // Arrange
            var newSubcategory = _fixture.Create<CreateSubcategoryDTO>();
            _mockSubcategoryRepository.Setup(repo => repo.CreateSubcategoryAsync(newSubcategory))
                .ReturnsAsync((ApiResponse<SubcategoryDTO>)null);
            // Act
            var result = await _subcategoryController.CreateSubcategory(newSubcategory);
            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateSubcategory_ReturnsOKResult_WhenSubcategoryIsUpdated()
        {
            // Arrange
            var subcategoryId = 1;
            var updateSubcategory = _fixture.Create<UpdateSubcategoryDTO>();
            var updatedSubcategory = _fixture.Create<SubcategoryDTO>();
            _mockSubcategoryRepository.Setup(repo => repo.UpdateSubcategoryAsync(updateSubcategory))
                .ReturnsAsync(new ApiResponse<SubcategoryDTO> { IsSuccess = true, Data = updatedSubcategory });
            // Act
            var result = await _subcategoryController.UpdateSubcategory(updateSubcategory);
            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(new ApiResponse<SubcategoryDTO> { IsSuccess = true, Data = updatedSubcategory });
        }

        //[Fact]
        //public async Task UpdateSubcategory_ReturnsBadRequestResult_WhenInvalidDataIsProvided()
        //{
        //    var updateSubcategory = _fixture.Create<UpdateSubcategoryDTO>();
        //    _mockSubcategoryRepository.Setup(repo => repo.UpdateSubcategoryAsync(updateSubcategory))
        //        .ReturnsAsync((ApiResponse<SubcategoryDTO>)null);
        //    // Act
        //    var result = await _subcategoryController.UpdateSubcategory(updateSubcategory);
        //    // Assert
        //    var badRequestResult = result.Result as BadRequestObjectResult;
        //    badRequestResult.Should().BeNull();
        //    badRequestResult.StatusCode.Should().Be(400);
        //}

        [Fact]
        public async Task DeleteSubcategory_ReturnsOKResult_WhenSubcategoryIsDeleted()
        {
            // Arrange
            var subcategoryId = 1;
            _mockSubcategoryRepository.Setup(repo => repo.DeleteSubcategoryAsync(subcategoryId))
                .ReturnsAsync(new ApiResponse<bool> { IsSuccess = true, Data = true });
            // Act
            var result = await _subcategoryController.DeleteSubcategory(subcategoryId);
            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(new ApiResponse<bool> { IsSuccess = true, Data = true });
        }
    }
}

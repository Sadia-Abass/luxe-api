using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.Category;
using luxe.Server.Application.Repositories;
using luxe.Server.Infrastructure.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;
using luxe.Server.Domain.Entities;

namespace luxe.Server.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<ApiResponse<CategoryDTO>> GetCategoryByIdAsync(int id) 
        { 
            throw new NotImplementedException(); 
        }

        public Task<ApiResponse<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync() 
        { 
            throw new NotImplementedException(); 
        }

        public async Task<ApiResponse<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO createCategoryDTO) 
        {
            try
            {
                if (await _appDbContext.Category.AnyAsync(c => c.Name == createCategoryDTO.Name))
                {
                    return new ApiResponse<CategoryDTO>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"{createCategoryDTO.Name} already exists." }
                    };
                }

                var category = new Category
                {
                    Name = createCategoryDTO.Name,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                _appDbContext.Category.Add(category);
                await _appDbContext.SaveChangesAsync();

                return new ApiResponse<CategoryDTO>
                {
                    StatusCode = HttpStatusCode.Created,
                    IsSuccess = true,
                    Data = new CategoryDTO
                    {
                        Id = category.Id,
                        Name = category.Name,
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.Message }
                };
            }
        }

        public Task<ApiResponse<CategoryDTO>> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO) 
        { 
            throw new NotImplementedException(); 
        }

        public Task<ApiResponse<bool>> DeleteCategoryAsync(int id) 
        { 
            throw new NotImplementedException(); 
        }
    }
}

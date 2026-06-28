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
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<ApiResponse<CategoryDTO>> GetCategoryByIdAsync(int id) 
        {
            try
            {
                var categoty = await _appDbContext.Category.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

                if(categoty == null)
                {
                    return new ApiResponse<CategoryDTO>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"Category with id {id} not found." },
                        Data = null
                    };
                }

                var categoryResponse = new CategoryDTO
                {
                    Id = categoty.Id,
                    Name = categoty.Name,
                };

                return new ApiResponse<CategoryDTO>
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = categoryResponse
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

        public async Task<ApiResponse<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _appDbContext.Category.AsNoTracking().ToListAsync();
                var categoryDTOs = categories.Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

                return new ApiResponse<IEnumerable<CategoryDTO>>
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = categoryDTOs
                };
            }
            catch (Exception ex) 
            {
                return new ApiResponse<IEnumerable<CategoryDTO>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.Message }
                };
            }
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
                        ErrorMessages = new List<string> { $"Category with the name '{createCategoryDTO.Name}' already exists." }
                    };
                }

                if(string.IsNullOrWhiteSpace(createCategoryDTO.Name))
                {
                    return new ApiResponse<CategoryDTO>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Category name cannot be empty." }
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
                    ErrorMessages = new List<string> { $"{createCategoryDTO.Name} created successfully." },
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

        public async Task<ApiResponse<CategoryDTO>> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO) 
        {
            try
            {
                var category = await _appDbContext.Category.FindAsync(updateCategoryDTO.Id);
                if(category == null)
                {
                    return new ApiResponse<CategoryDTO>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"Category with id '{updateCategoryDTO.Id}' not found." }
                    };
                }

                if(await _appDbContext.Category.AnyAsync(c => c.Name == updateCategoryDTO.Name && c.Id != updateCategoryDTO.Id))
                {
                    return new ApiResponse<CategoryDTO>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"Category with the name '{updateCategoryDTO.Name}' already exists." }
                    };
                }

                category.Name = updateCategoryDTO.Name;
                await _appDbContext.SaveChangesAsync();

                return new ApiResponse<CategoryDTO>
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = new CategoryDTO
                    {
                        Id = category.Id,
                        Name = category.Name
                    }
                };
            }
            catch (Exception ex) 
            {
                return new ApiResponse<CategoryDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { $"An unexpected error occurred while processing your request, Error: {ex.Message}" }
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _appDbContext.Category.FindAsync(id);
                if (category == null)
                {
                    return new ApiResponse<bool>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"Category with id '{id}' not found." }
                    };
                }

                category.IsDeleted = true;
                await _appDbContext.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    ErrorMessages = new List<string> { $"Category with id '{id}' has been deleted successfully." }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { $"An unexpected error occurred while processing your request, Error: {ex.Message}" }
                };
            }          
        }
    }
}

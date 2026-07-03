using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.Category;
using luxe.Server.Application.DTOs.Subcategory;
using luxe.Server.Application.Repositories;
using luxe.Server.Domain.Entities;
using luxe.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace luxe.Server.Infrastructure.Repositories
{
    public class SubcategoryRepostory : ISubcategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public SubcategoryRepostory(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<ApiResponse<IEnumerable<SubcategoryDTO>>> GetAllSubcategoriesAsync()
        {
            try
            {
                var subcategories = await _appDbContext.Subcategory.AsNoTracking().Include(s => s.Category).ToListAsync();
                if(subcategories == null || !subcategories.Any())
                {
                    return new ApiResponse<IEnumerable<SubcategoryDTO>>
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "No subcategories found." },
                        Data = null
                    };
                };

                var subcategoryList = subcategories.Select(s => new SubcategoryDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category.Name  
                }).ToList();

                return new ApiResponse<IEnumerable<SubcategoryDTO>>
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = subcategoryList
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SubcategoryDTO>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An error occurred while fetching subcategories." },
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<SubcategoryDTO>> GetSubcategoryByIdAsync(int id)
        {
            try 
            {
                var subcategory = await _appDbContext.Subcategory
                    .AsNoTracking()
                    .Include(s => s.Category)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (subcategory == null)
                {
                    return new ApiResponse<SubcategoryDTO>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"Subcategory with ID {id} not found." },
                        Data = null
                    };
                }

                var subcategoryDTO = new SubcategoryDTO
                {
                    Id = subcategory.Id,
                    Name = subcategory.Name,
                    CategoryId = subcategory.CategoryId,
                    CategoryName = subcategory.Category.Name,
                    IsDeleted = false
                };

                return new ApiResponse<SubcategoryDTO>
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = subcategoryDTO
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SubcategoryDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An error occurred while fetching the subcategory." },
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<SubcategoryDTO>> CreateSubcategoryAsync(CreateSubcategoryDTO createSubcategoryDTO)
        {
            try
            {
                if(await _appDbContext.Subcategory.AnyAsync(s => s.Name.ToLower() == createSubcategoryDTO.Name.ToLower()))
                {
                    return new ApiResponse<SubcategoryDTO>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"A subcategory with the name '{createSubcategoryDTO.Name}' already exists." },
                        Data = null
                    };
                }

                if(!await _appDbContext.Category.AnyAsync(c => c.Id == createSubcategoryDTO.CategoryId))
                {
                    return new ApiResponse<SubcategoryDTO>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"Category with ID {createSubcategoryDTO.CategoryId} does not exist." },
                        Data = null
                    };
                }

                var subcategory = new Subcategory
                {
                    Name = createSubcategoryDTO.Name,
                    CategoryId = createSubcategoryDTO.CategoryId
                };

                _appDbContext.Subcategory.Add(subcategory);
                await _appDbContext.SaveChangesAsync();

                var subcategoryResponse = new SubcategoryDTO
                {
                    Id = subcategory.Id,
                    Name = createSubcategoryDTO.Name,
                    CategoryId = createSubcategoryDTO.CategoryId
                };

                return new ApiResponse<SubcategoryDTO>
                {
                    StatusCode = HttpStatusCode.Created,
                    IsSuccess = true,
                    Data = subcategoryResponse
                };

            }
            catch (Exception ex) 
            {
                return new ApiResponse<SubcategoryDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An error occurred while creating the subcategory." },
                    Data = null
                };
            }
        }


        public async Task<ApiResponse<SubcategoryDTO>> UpdateSubcategoryAsync(UpdateSubcategoryDTO updateSubcategoryDTO)
        {
            try
            {
                var subcategory = await _appDbContext.Subcategory.FindAsync(updateSubcategoryDTO.Id);
                if(subcategory == null)
                {
                    return new ApiResponse<SubcategoryDTO>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"Subcategory with ID {updateSubcategoryDTO.Id} not found." },
                        Data = null
                    };
                }

                if(await _appDbContext.Subcategory.AnyAsync(s => s.Name.ToLower() == updateSubcategoryDTO.Name.ToLower() && s.Id == updateSubcategoryDTO.Id))
                {
                    return new ApiResponse<SubcategoryDTO>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"A subcategory with the name '{updateSubcategoryDTO.Name}' already exists." },
                        Data = null
                    };
                }

                subcategory.Name = updateSubcategoryDTO.Name;
                subcategory.CategoryId = updateSubcategoryDTO.CategoryId;
                await _appDbContext.SaveChangesAsync();

                return new ApiResponse<SubcategoryDTO>
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    ErrorMessages = new List<string> { $"Subcategory with ID {updateSubcategoryDTO.Id} updated successfully." },
                    Data = new SubcategoryDTO
                    {
                        Id = subcategory.Id,
                        Name = subcategory.Name,
                        CategoryId = subcategory.CategoryId
                    }
                };
            }
            catch (Exception ex) 
            {
                return new ApiResponse<SubcategoryDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An error occurred while updating the subcategory." },
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteSubcategoryAsync(int id)
        {
            try
            {
                var subcategory = await _appDbContext.Subcategory
                    .Include(s => s.Category)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if(subcategory == null)
                {
                    return new ApiResponse<bool>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"Subcategory with ID {id} not found." }
                    };
                }

                subcategory.IsDeleted = true;
                await _appDbContext.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    ErrorMessages = new List<string> { $"Subcategory with ID {id} deleted successfully." },
                    Data = true
                };
            }
            catch (Exception ex) 
            {
                return new ApiResponse<bool>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An error occurred while deleting the subcategory." },
                    Data = false
                };
            }
        }
    }
}

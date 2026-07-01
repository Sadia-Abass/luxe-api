using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.Subcategory;
using luxe.Server.Application.Repositories;
using luxe.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
                var subcategories = await _appDbContext.Subcategory.AsNoTracking().ToListAsync();
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
                    CategoryName = s.Category.Name  
                }).ToList();

                return new ApiResponse<IEnumerable<SubcategoryDTO>>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    IsSuccess = true,
                    Data = subcategoryList
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SubcategoryDTO>>
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An error occurred while fetching subcategories." },
                    Data = null
                };
            }
        }

        public Task<ApiResponse<SubcategoryDTO>> GetSubcategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<SubcategoryDTO>> CreateSubcategoryAsync(CreateSubcategoryDTO createSubcategoryDTO)
        {
            throw new NotImplementedException();
        }


        public Task<ApiResponse<SubcategoryDTO>> UpdateSubcategoryAsync(UpdateSubcategoryDTO updateSubcategoryDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> DeleteSubcategoryAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

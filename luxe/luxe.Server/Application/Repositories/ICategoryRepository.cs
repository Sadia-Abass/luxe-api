using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.Category;

namespace luxe.Server.Application.Repositories
{
    public interface ICategoryRepository
    {
        Task<ApiResponse<CategoryDTO>> GetCategoryByIdAsync(int id);
        Task<ApiResponse<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync();
        Task<ApiResponse<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO createCategoryDTO);
        Task<ApiResponse<CategoryDTO>> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO);
        Task<ApiResponse<bool>> DeleteCategoryAsync(int id);
    }
}

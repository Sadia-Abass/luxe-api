using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.Subcategory;

namespace luxe.Server.Application.Repositories
{
    public interface ISubcategoryRepository
    {
        Task<ApiResponse<SubcategoryDTO>> GetSubcategoryByIdAsync(int id);
        Task<ApiResponse<IEnumerable<SubcategoryDTO>>> GetAllSubcategoriesAsync();
        Task<ApiResponse<SubcategoryDTO>> CreateSubcategoryAsync(CreateSubcategoryDTO createSubcategoryDTO);
        Task<ApiResponse<SubcategoryDTO>> UpdateSubcategoryAsync(UpdateSubcategoryDTO updateSubcategoryDTO);
        Task<ApiResponse<bool>> DeleteSubcategoryAsync(int id);
    }
}

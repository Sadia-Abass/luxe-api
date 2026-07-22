using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.Users;
using luxe.Server.Domain.Entities;

namespace luxe.Server.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        Task<AppUser?> GetUserWithRefreshTokenAsync(string userId);
        Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(RefreshToken refreshToken, string? replaceToken = null);

        Task<ApiResponse<PagedResultDTO<UserResponseDTO>>> GetAllUsersAsync(int pageNumber, int pageSize, string? search);
        Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(string id);
        Task<ApiResponse<UserResponseDTO>> UpdateUserAsync(string userId, UpdateUserDTO updateUserDto);
        Task<ApiResponse<UserResponseDTO>> AddNewUserAsync(AddUserDTO addUserDTO);
        Task<ApiResponse<string>> UpdateProfileImageAsync(string userId, IFormFile file);
        Task<ApiResponse<string>> DeleteUserAsync(string userId);
        Task<ApiResponse<string>> AssignRoleAsync(string userId, AssignRoleDTO assignRoleDTO);
        Task<ApiResponse<string>> RemoveRoleAsync(string userId, string role);
    }
}

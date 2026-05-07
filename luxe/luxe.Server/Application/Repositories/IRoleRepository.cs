using CloudinaryDotNet.Actions;
using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.AuthenticationDTOs;

namespace luxe.Server.Application.Repositories
{
    public interface IRoleRepository
    {
        Task<ApiResponse<RoleDTO>> CreateRoleAsync(string name);
        Task<ApiResponse<RoleDTO>> AssignRoleAsync(AssignRoleDTO assignRoleDTO); 
    }
}

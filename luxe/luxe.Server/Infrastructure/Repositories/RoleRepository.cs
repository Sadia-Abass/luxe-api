using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.AuthenticationDTOs;
using luxe.Server.Application.Repositories;
using luxe.Server.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace luxe.Server.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleRepository(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ApiResponse<RoleDTO>> CreateRoleAsync(CreateRoleDTO createRoleDTO)
        {
            var roleName = createRoleDTO.RoleName;
            if (await _roleManager.RoleExistsAsync(createRoleDTO.RoleName))
            {
                return new ApiResponse<RoleDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { $"{createRoleDTO.RoleName} already exists." },
                    Data = null
                };
            }

            var result = await _roleManager.CreateAsync(new AppRole { Name = createRoleDTO.RoleName, Description = createRoleDTO.Description, DateCreated = DateTime.UtcNow, IsActive = createRoleDTO.IsActive });
            if (!result.Succeeded)
            {
                return new ApiResponse<RoleDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = result.Errors.Select(e => e.Description).ToList(),
                    Data = null
                };
            }

            return new ApiResponse<RoleDTO>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                ErrorMessages = new List<string> { $"Role {createRoleDTO.RoleName} created successfully." },
                Data = new RoleDTO { Name = createRoleDTO.RoleName, Description = createRoleDTO.Description, IsActive = createRoleDTO.IsActive }
            };
        }

        public async Task<ApiResponse<RoleDTO>> AssignRoleAsync(AssignRoleDTO assignRoleDTO)
        {
            var user = await _userManager.FindByEmailAsync(assignRoleDTO.Email);
            if (user == null)
            {
                return new ApiResponse<RoleDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { $"User with email {assignRoleDTO.Email} not found." },
                    Data = null
                };
            }

            if (!await _roleManager.RoleExistsAsync(assignRoleDTO.RoleName))
            {
                return new ApiResponse<RoleDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { $"Role {assignRoleDTO.RoleName} does not exist." },
                    Data = null
                };
            }

            var result = await _userManager.AddToRoleAsync(user, assignRoleDTO.RoleName);
            if (!result.Succeeded)
            {
                return new ApiResponse<RoleDTO>
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = result.Errors.Select(e => e.Description).ToList(),
                    Data = null
                };
            }

            return new ApiResponse<RoleDTO>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                ErrorMessages = new List<string> { $"Role {assignRoleDTO.RoleName} assigned to user successfully." },
                Data = new RoleDTO { Name = assignRoleDTO.RoleName }
            };
        }
    }
}

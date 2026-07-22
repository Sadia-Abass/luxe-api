using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.Users;
using luxe.Server.Application.Repositories;
using luxe.Server.Application.Services;
using luxe.Server.Domain.Entities;
using luxe.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Net;

namespace luxe.Server.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        //private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileUploaderService _fileUploaderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(AppDbContext context, UserManager<AppUser> userManager, IUserRepository userRepository, IFileUploaderService fileUploaderService, IHttpContextAccessor httpContextAccessor) : base(context) 
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _fileUploaderService = fileUploaderService;
            _httpContextAccessor = httpContextAccessor;
        }          

        public async Task<AppUser?> GetUserWithRefreshTokenAsync(string userId)
        {
            return await _context.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken refreshToken, string? replaceToken = null)
        {
            refreshToken.RevokedDate = DateTime.UtcNow;
            if (replaceToken != null)
            {
                refreshToken.ReplacedByToken = replaceToken;
            }
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<ApiResponse<PagedResultDTO<UserResponseDTO>>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10, string? search = null)
        {
            var (items, totalCount) = await _userRepository.GetPagedAsync(pageNumber, pageSize, filter: search == null ? null : u => u.Email!.Contains(search) || u.FirstName.Contains(search) || u.LastName.Contains(search));

            var userDtos = new List<UserResponseDTO>();
            foreach (var user in items)
            {
                userDtos.Add(await MapToDto(user));
            }

            return new ApiResponse<PagedResultDTO<UserResponseDTO>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Data = new PagedResultDTO<UserResponseDTO>
                {
                    Items = userDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                }
            };
        }

        public Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<UserResponseDTO>> AddNewUser(AddUserDTO addUserDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<UserResponseDTO>> UpdateUserAsync(string userId, UpdateUserDTO updateUserDto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> UpdateProfileImageAsync(string userId, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> AssignRole(string userId, AssignRoleDTO assignRoleDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> RemoveRole(string userId, string role)
        {
            throw new NotImplementedException();
        }

        private bool IsOwnerOrAdmin(string targetUserId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return currentUserId == targetUserId || _httpContextAccessor.HttpContext.User.IsInRole("Admin");
        }

        private async Task<UserResponseDTO> MapToDto(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DOB = user.DOB,
                Email = user.Email ?? string.Empty,
                ImageUrl = user.ImageUrl,
                EmailConfirmed = user.EmailConfirmed,
                Datejoined = user.Datejoined,
                Roles = roles,
                IsActive = user.IsActive,
            };
        }

        private static string? ExtractPublicIdFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                var segments = uri.AbsolutePath.Split('/');
                var uploadIndex = Array.IndexOf(segments, "upload");
                if(uploadIndex == -1)
                {
                    return null;
                }

                var publicIdParts = segments.Skip(uploadIndex + 2);
                var publicIdWithExtension = string.Join("/", publicIdParts);

                return publicIdWithExtension[..publicIdWithExtension.LastIndexOf('.')];
            }
            catch
            {
                return null;
            }
        }
    }
}

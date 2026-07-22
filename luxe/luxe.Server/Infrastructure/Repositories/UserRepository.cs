using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.Users;
using luxe.Server.Application.Repositories;
using luxe.Server.Domain.Entities;
using luxe.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace luxe.Server.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        //private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context) { }          


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

        public Task<ApiResponse<PagedResultDTO<UserResponseDTO>>> GetAllUsersAsync(int pageNumber, int pageSize, string? search)
        {
            throw new NotImplementedException();
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
    }
}

using luxe.Server.Domain.Entities;

namespace luxe.Server.Application.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser?> GetUserWithRefreshTokenAsync(string userId);
        Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(RefreshToken refreshToken, string? replaceToken = null);
    }
}

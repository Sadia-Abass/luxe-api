using luxe.Server.Domain.Entities;

namespace luxe.Server.Application.Repositories
{
    public interface IUserRepository
    {
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
    }
}

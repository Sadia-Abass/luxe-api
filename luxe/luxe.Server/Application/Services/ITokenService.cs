using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.AuthenticationDTOs;
using luxe.Server.Domain.Entities;
using System.Security.Claims;

namespace luxe.Server.Application.Services
{
    public interface ITokenService
    {
        string CreateAccessTokenAsync(AppUser user, IList<string> roles);
        RefreshToken CreateRefreshToken(string ipAddress);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}

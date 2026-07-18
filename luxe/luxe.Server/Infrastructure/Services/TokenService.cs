using luxe.Server.Application.Services;
using luxe.Server.Domain.Entities;
using luxe.Server.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace luxe.Server.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string CreateAccessTokenAsync(AppUser user, IList<string> roles)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings?.SecretKey ?? string.Empty));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256); 

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // unique token ID
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            // Add each role as its own claim 
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings?.Issuer ?? string.Empty,
                audience: _jwtSettings?.Audience ?? string.Empty,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings?.MinutesToExpiration ?? 0),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken CreateRefreshToken(string userId)
        {
            // Cryptographically random bytes - NOT a JWT, just a secure random string
            var randomBytes = new byte[64];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(randomBytes);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpireDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                UserId = userId,
                CreatedDate = DateTime.UtcNow,
            };
        }

        // This defends against a known JWT attack where someone crafts a token using the alg: none header, tricking a naive validator into skipping signature verification entirely. Checking the algorithm explicitly closes that hole.
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = false, // we want to get claims from expired token
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out var securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecret;
        private readonly string _refreshSecret;

        public TokenService(string jwtSecret, string refreshSecret)
        {
            _jwtSecret = jwtSecret;
            _refreshSecret = refreshSecret;
        }

        public async Task<TokenResponse> GenerateTokens(Guid userId, string role)
        {
            var accessExpires = DateTime.UtcNow.AddMinutes(1);
            var refreshExpires = DateTime.UtcNow.AddDays(7);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = Encoding.UTF8.GetBytes(_jwtSecret);
            var refreshKey = Encoding.UTF8.GetBytes(_refreshSecret);

            //AccessToken
            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            }),
                Expires = accessExpires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "HITS",
                Audience = "HITS"
            };
            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
            var accessTokenString = tokenHandler.WriteToken(accessToken);

            //RefreshToken
            var refreshExpiresInMinutes = (int)(refreshExpires - DateTime.UtcNow).TotalMinutes;
            var refreshDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, userId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim("TokenType", "Refresh")
            }),
                Expires = refreshExpires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(refreshKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "HITS",
                Audience = "HITS"
            };
            var refreshToken = tokenHandler.CreateToken(refreshDescriptor);
            var refreshTokenString = tokenHandler.WriteToken(refreshToken);

            return new TokenResponse
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshTokenString
            };
        }
    }
}


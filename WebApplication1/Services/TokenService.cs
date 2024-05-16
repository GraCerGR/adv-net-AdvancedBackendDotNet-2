using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_Service.Context;
using User_Service.Models;
using User_Service.Services.Interfaces;

namespace User_Service.Services
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

        public async Task<TokenModel> GenerateAccessToken(Guid userId, string role)
        {
            var accessExpires = DateTime.UtcNow.AddMinutes(5);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = Encoding.UTF8.GetBytes(_jwtSecret);

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

            TokenModel token = new TokenModel
            {
                Token = accessTokenString,
                Expires = accessExpires
            };

            return token;
        }

        public async Task<TokenModel> GenerateRefreshToken(Guid userId, string role)
        {
            var refreshExpires = DateTime.UtcNow.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            var refreshKey = Encoding.UTF8.GetBytes(_refreshSecret);

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

            TokenModel token = new TokenModel
            {
                Token = refreshTokenString,
                Expires = refreshExpires
            };

            return token;
        }

        public async Task<string> RefreshAccessToken(string refreshToken)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(refreshToken);
            string role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "HITS",
                ValidateAudience = true,
                ValidAudience = "HITS",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_refreshSecret))
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out validatedToken);
            var userId = principal.FindFirst(ClaimTypes.Name)?.Value;

            // Генерация нового access токена
            var newAccessToken = await GenerateAccessToken(new Guid(userId), role);

            return newAccessToken.Token;
        }
    }
}


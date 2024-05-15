using User_Service.Models;

namespace User_Service.Services.Interfaces
{
    public interface ITokenService
    {
        Task<TokenModel> GenerateAccessToken(Guid userId, string role);

        Task<TokenModel> GenerateRefreshToken(Guid userId, string role);

        Task<string> RefreshAccessToken(string refreshToken);
    }
}

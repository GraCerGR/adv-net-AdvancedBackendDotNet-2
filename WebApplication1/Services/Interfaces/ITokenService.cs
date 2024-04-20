using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface ITokenService
    {
        Task<TokenModel> GenerateAccessToken(Guid userId, string role);

        Task<TokenModel> GenerateRefreshToken(Guid userId, string role);

/*        Task<TokenModel> RefreshAccessToken(string refreshToken);*/
    }
}

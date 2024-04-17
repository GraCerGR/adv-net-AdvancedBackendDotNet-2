using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateTokens(Guid userId, string role);
    }
}

using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Services.Interfaces
{
    public interface IUserService
    {
        Task<TokenResponse> RegistrationUser(UserRegisterModel userRegisterModel);

        Task<TokenResponse> LoginUser(LoginCredentials credentials);


 //       Task LogoutUser(string token);
        Task<UserDto> EditProfile(EditUserModel editUserModel, string userId);

        Task<UserDto> GetProfile(string guid);

        Task<string> GetUserIdFromToken(string token);

        Task<string> ChangePassword(string guid, EditPasswordModel editPasswordModel);

        Task SendCode(string code, string userId);
    }
}

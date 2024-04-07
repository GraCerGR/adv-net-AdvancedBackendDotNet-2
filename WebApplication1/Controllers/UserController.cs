using System.IdentityModel.Tokens.Jwt;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.Models.DTO;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        
        [HttpPost("register")]
        public async Task<TokenResponse> RegisterUser(UserRegisterModel userRegisterModel)
        {
            return await _userService.RegistrationUser(userRegisterModel);
        }

        [HttpPost("login")]

        public async Task<TokenResponse> LoginUser(LoginCredentials credentials)
        {
            return await _userService.LoginUser(credentials);
        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<UserDto> GetProfile()
        {
                string authorizationHeader = Request.Headers["Authorization"];
                // Извлекаем токен Bearer из значения заголовка
                string bearerToken = authorizationHeader.Substring("Bearer ".Length);

                //string bearerToken = HttpContext.Session.GetString("Token");

                var userId = await _userService.GetUserIdFromToken(bearerToken);

                return await _userService.GetProfile(userId);
        }


        [HttpGet("profile/{userApplicantId}")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<UserDto> GetProfileById(string userApplicantId)
        {
/*            try
            {*/
                // Получаем значение заголовка "Authorization"
                string authorizationHeader = Request.Headers["Authorization"];
                // Извлекаем токен Bearer из значения заголовка
                string bearerToken = authorizationHeader.Substring("Bearer ".Length);
                //string bearerToken = HttpContext.Session.GetString("Token");

                var userId = await _userService.GetUserIdFromToken(bearerToken);

                var userProfile = await _userService.GetProfile(userId); // - Существует такой пользователь или нет

                //Проверка: UserId менеджер абитуриента userApplicantId



                return await _userService.GetProfile(userApplicantId);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<UserDto> PutProfile(EditUserModel editUserModel)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var userId = await _userService.GetUserIdFromToken(bearerToken);

            return await _userService.EditProfile(editUserModel, userId);
        }

        [HttpPut("profile/{userApplicantId}")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<UserDto> PutProfileById(string userApplicantId, EditUserModel editUserModel)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var userId = await _userService.GetUserIdFromToken(bearerToken);

            var userProfile = await _userService.GetProfile(userId);


            //Проверка: UserId менеджер абитуриента userApplicantId


            return await _userService.EditProfile(editUserModel, userApplicantId);
        }
    }
}

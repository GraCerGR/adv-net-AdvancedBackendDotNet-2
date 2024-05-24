﻿using System.IdentityModel.Tokens.Jwt;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Service.Context;
using User_Service.Models;
using User_Service.Models.DTO;
using User_Service.Services;
using User_Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using User_Service.Migrations;


namespace WebApplication1.Controllers
{
    [Route("user/[controller]")]
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

        [HttpPost("logout")]
        [Authorize]

        public async Task<string> LogoutUser()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);
            var userId = await _userService.GetUserIdFromToken(bearerToken);

            string refreshTokenHeader = Request.Headers["Refresh-token"];
            if (refreshTokenHeader != null)
            {
                string refreshToken = refreshTokenHeader.Substring("".Length);
                await _userService.LogoutUser(Guid.Parse(userId), bearerToken, refreshToken);
                return "The refresh token has been detected. Logged out of this session";
            }
            else
            {
                await _userService.LogoutUser(Guid.Parse(userId), bearerToken, null);
                return "The refresh token was not detected. Logged out of all sessions";
            }

        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<UserDto> GetProfile()
        {
                string authorizationHeader = Request.Headers["Authorization"];
                string bearerToken = authorizationHeader.Substring("Bearer ".Length);

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

                //Проверка: UserId менеджер 



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

        [HttpPut("profile/changePassword")]
        [Authorize]
        public async Task<string> ChangePassword(EditPasswordModel editPasswordModel)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);
            var userId = await _userService.GetUserIdFromToken(bearerToken);

            return await _userService.ChangePassword(userId, editPasswordModel);
        }

        [Authorize]
        [HttpPost("profile/code")]
        public async Task CheckCode(string code)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);
            var userId = await _userService.GetUserIdFromToken(bearerToken);

            await _userService.SendCode(code, userId);
        }
    }
}

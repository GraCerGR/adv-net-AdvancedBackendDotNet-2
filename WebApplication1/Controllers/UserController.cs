using System.IdentityModel.Tokens.Jwt;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Context;
using MVC.Models;
using MVC.Models.DTO;
using MVC.Services;
using MVC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace MVC.Controllers
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
/*            if (ModelState.IsValid)
            {
                try
                {
                    var tokenResponse = await _userService.LoginUser(credentials);
                    if (tokenResponse != null)
                    {
                        //HttpContext.Session.SetString("Token", tokenResponse.Token);
                        return Ok(tokenResponse);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }
            }
            return BadRequest();*/
        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<UserDto> GetProfile()
        {
/*            try
            {*/
                // Получаем значение заголовка "Authorization"
                string authorizationHeader = Request.Headers["Authorization"];
                // Извлекаем токен Bearer из значения заголовка
                string bearerToken = authorizationHeader.Substring("Bearer ".Length);

                //string bearerToken = HttpContext.Session.GetString("Token");

                var userId = await _userService.GetUserIdFromToken(bearerToken);

                return await _userService.GetProfile(userId);

/*                if (userProfile == null)
                {
                    return NotFound();
                }

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }*/
        }


        [HttpGet("profile/{userApplicantId}")]
        [Authorize]
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

                var userProfile = await _userService.GetProfile(userId);

                //Проверка: UserId менеджер абитуриента userApplicantId



                return await _userService.GetProfile(userApplicantId);

/*                if (userProfile == null || userApplicantProfile == null)
                {
                    return NotFound();
                }*/

                //Проверка: UserId менеджер абитуриента userApplicantId

/*                return Ok(userApplicantProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }*/
        }


/*        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }*/
    }
}

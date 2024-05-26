using System.Net;
using User_Service.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using User_Service.Services.Interfaces;
using User_Service.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using User_Service.Migrations;

namespace User_Service.Services
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        /*private readonly ITokenService _tokenService;*/

        public TokenMiddleware(RequestDelegate next/*, ITokenService tokenService*/)
        {
            _next = next;
/*            _tokenService = tokenService;*/
        }

        public async Task Invoke(HttpContext context)
        {
            string httpMethod = context.Request.Method;

            // Получить путь запроса
            string path = context.Request.Path;

            // Получить заголовки запроса
            var headers = context.Request.Headers;

            // Получить параметры запроса
            var queryParameters = context.Request.Query;

            string accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split("").Last().Substring("Bearer".Length).Split(" ").Last();
            if (accessToken == "")
            {
                accessToken = null;
            }
            
            //string accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last().Substring("Bearer ".Length);
            /*            string authorizationHeader = context.Request.Headers["Authorization"];
                        string accessToken = authorizationHeader.Substring("Bearer ".Length);*/
            /*token != nulltoken.ValidTo.Subtract(DateTime.UtcNow) < TimeSpan.FromSeconds(10)*/

            if (accessToken != null)
            {
                using (var scope = context.RequestServices.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<ITokenService>();

                    ApplicationContext _context = context.RequestServices.GetRequiredService<ApplicationContext>();

                    var revorkedToke = await _context.AccessTokensRevoked.FirstOrDefaultAsync(u => u.AccessToken == accessToken);

                    if (revorkedToke != null)
                    {
                        var ex = new Exception();
                        ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(), "The token is invalid");
                        throw ex;
                    }

                    var token = new JwtSecurityTokenHandler().ReadToken(accessToken) as JwtSecurityToken;
                    if (token.ValidTo.Subtract(DateTime.UtcNow) < TimeSpan.FromSeconds(0))
                    {
                        string refreshToken = context.Request.Headers["Refresh-Token"];

                        var user = await _context.RefreshTokens.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

                        if (user == null || user.Expires <= DateTime.UtcNow)
                        {
                            var ex = new Exception();
                            ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(), "The token is invalid");
                            throw ex;
                        }

                        var existingUser = _context.Users.FirstOrDefaultAsync(u => u.Id == user.UserId);

                        if (existingUser == null)
                        {
                            var ex = new Exception();
                            ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(), "The user was not found");
                            throw ex;
                        }

                        string role = "Applicant";

                        var manager = await _context.Managers.FirstOrDefaultAsync(x => x.UserId == user.UserId);
                        var admin = await _context.Admins.FirstOrDefaultAsync(x => x.UserId == user.UserId);

                        if (manager != null)
                        {
                            if (manager.MainManager == false)
                            {
                                role = "Manager";
                            }
                            else
                            {
                                role = "MainManager";
                            }
                        }

                        if (admin != null)
                        {
                            role = "Admin";
                        }

                        var newToken = await userService.RefreshAccessToken(refreshToken, role);
                        context.Response.Headers.Add("Authorization", "Bearer " + newToken);
                    }
/*                    try
                    {
                        string newAccessToken = await _tokenService.RefreshAccessToken(refreshToken);
                        context.Response.Headers.Add("Access-Token", newAccessToken);
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add(StatusCodes.Status400BadRequest.ToString(), "Bad Request");
                        throw ex;
                    }*/

                }
            }

            await _next(context);
        }
    }
}
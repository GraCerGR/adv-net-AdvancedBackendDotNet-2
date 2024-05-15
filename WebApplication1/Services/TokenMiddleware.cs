using System.Net;
using User_Service.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using User_Service.Services.Interfaces;

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
            string accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split("").Last().Substring("Bearer ".Length);
            /*            string authorizationHeader = context.Request.Headers["Authorization"];
                        string accessToken = authorizationHeader.Substring("Bearer ".Length);*/
            /*token != nulltoken.ValidTo.Subtract(DateTime.UtcNow) < TimeSpan.FromSeconds(10)*/

            if (accessToken != null)
            {
                var token = new JwtSecurityTokenHandler().ReadToken(accessToken) as JwtSecurityToken;
                if (token != null)
                {
                    string refreshToken = context.Request.Headers["Refresh-Token"];


                    using (var scope = context.RequestServices.CreateScope())
                    {
                        var userService = scope.ServiceProvider.GetRequiredService<ITokenService>();

                        try
                        {
                            var newToken = await userService.RefreshAccessToken(refreshToken);
                            context.Response.Headers.Add("Authorization", "Bearer " + newToken);
                        }
                        catch (Exception ex)
                        {
                            ex.Data.Add(StatusCodes.Status400BadRequest.ToString(), "Bad Request");
                            throw ex;
                        }
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
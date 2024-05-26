using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        public async Task<IActionResult> Login()
        {
            return View();
        }

        public async Task<IActionResult> EditUserInfo()
        {
            return View();
        }

        public async Task<IActionResult> EditUserPaasword()
        {
            return View();
        }

        public async Task<IActionResult> SendCodeView()
        {
            return View();
        }

        public async Task<IActionResult> UserInfo()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7101");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                var response = await client.GetAsync("/user/User/profile");

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.GetAsync("/user/User/profile");
                }

                if (response.IsSuccessStatusCode)
                {
                    var profileContent = await response.Content.ReadAsStringAsync();

                    var profileContentResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<UserModel>(profileContent);

                    UserDto user = new UserDto
                    {
                        Id = profileContentResponse.Id,
                        Name = profileContentResponse.Name,
                        Email = profileContentResponse.Email,
                        Birthdate = profileContentResponse.Birthdate,
                        Gender = profileContentResponse.Gender,
                        Citizenship = profileContentResponse.Citizenship,
                        PhoneNumber = profileContentResponse.PhoneNumber
                    };

                    return View("UserInfo", user);
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
        }

        [HttpPost]
        async public Task<IActionResult> Login(string email, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7101");
                client.DefaultRequestHeaders.Add("accept", "text/plain");

                var requestBody = new
                {
                    email = email,
                    password = password
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/user/User/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);

                    var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                    Console.WriteLine(tokenResponse.refreshToken);
                    

                    Response.Cookies.Append("accessToken", tokenResponse.accessToken);
                    Response.Cookies.Append("refreshToken", tokenResponse.refreshToken);
                    //HttpContext.Session.SetString("refreshToken", tokenResponse.refreshToken);

                    return RedirectToAction("UserInfo");
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
        }

        async public Task<IActionResult> SaveUserInfo(EditUserModel updatedUser)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7101/");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                var json = JsonConvert.SerializeObject(updatedUser);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync("user/User/profile", content);

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.PutAsync("user/User/profile", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("UserInfo");
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
        }

        async public Task<IActionResult> SaveUserPassword(EditPasswordModel updatePassword)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7101/");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                var json = JsonConvert.SerializeObject(updatePassword);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync("user/User/profile/changePassword", content);

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.PutAsync("user/User/profile/changePassword", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SendCodeView");
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
        }

        async public Task<IActionResult> SendCode(string code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7101/");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

/*                var json = JsonConvert.SerializeObject(code);
                var content = new StringContent(json, Encoding.UTF8, "application/json");*/

                HttpResponseMessage response = await client.PostAsync($"user/User/profile/code?code={code}", null) ;

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.PostAsync($"user/User/profile/code?code={code}", null);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("UserInfo");
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
        }

        async public Task<IActionResult> Logout()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7101/");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                HttpResponseMessage response = await client.PostAsync($"user/User/logout", null);

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.PostAsync($"user/User/logout", null);
                }

                if (response.IsSuccessStatusCode)
                {

                    Response.Cookies.Delete("accessToken");
                    Response.Cookies.Delete("refreshToken");
                    //HttpContext.Session.Remove("refreshToken");

                    return RedirectToAction("Login");
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
        }
    }
}

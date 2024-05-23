using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult EditUserInfo()
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
                string refreshToken = HttpContext.Session.GetString("refreshToken");

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                var response = await client.GetAsync("/user/User/profile");

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

                    Response.Cookies.Append("accessToken", tokenResponse.accessToken);
                    HttpContext.Session.SetString("refreshToken", tokenResponse.refreshToken);

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
                string refreshToken = HttpContext.Session.GetString("refreshToken");

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                var json = JsonConvert.SerializeObject(updatedUser);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync("user/User/profile", content);

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
    }
}

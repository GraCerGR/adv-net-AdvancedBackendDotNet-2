using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class ManagerController : Controller
    {
        public async Task<IActionResult> GetManager()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7101/");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                HttpResponseMessage response = await client.GetAsync($"/api/Manager/manager");

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    // Повторное выполнение запроса с новым accessToken
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.GetAsync($"/api/Manager/manager");
                }

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var contentResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ManagerDto>>(content);
                    return View("GetManager", contentResponse);
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

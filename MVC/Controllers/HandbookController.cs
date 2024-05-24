using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.Handbook;

namespace MVC.Controllers
{
    public class HandbookController : Controller
    {
        public IActionResult Import()
        {
            return View();
        }

        public async Task<IActionResult> EducationLevels()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7023");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = HttpContext.Session.GetString("refreshToken");

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                var response = await client.GetAsync("/api/Handbook/EducationLevels");

                if (response.IsSuccessStatusCode)
                {
                    var Content = await response.Content.ReadAsStringAsync();

                    var ContentResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EducationLevelModel>>(Content);


                    return PartialView("_EducationLevelsPartial", ContentResponse);
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

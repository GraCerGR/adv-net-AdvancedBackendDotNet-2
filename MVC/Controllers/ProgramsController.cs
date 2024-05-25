using Microsoft.AspNetCore.Mvc;
using MVC.Models.Handbook;
using MVC.Models.Programs;

namespace MVC.Controllers
{
    public class ProgramsController : Controller
    {
        public IActionResult Programs()
        {
            return View();
        }

        public async Task<IActionResult> SearchPrograms(ProgramSearchModel searchModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7122");
                client.DefaultRequestHeaders.Add("accept", "text/plain");

                string url = $"/api/Programs/programs?Faculty={searchModel.Faculty}&EducationLevel={searchModel.EducationLevel}&EducationForm={searchModel.EducationForm}&Language={searchModel.Language}&Code={searchModel.Code}&Name={searchModel.Name}&Page={searchModel.Page}&Size={searchModel.Size}";

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var contentResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ProgramPagedListModel>(content);
                    return PartialView("_SearchPrograms", contentResponse);
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

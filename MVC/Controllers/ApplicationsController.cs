using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Handbook;
using MVC.Models.Programs;

namespace MVC.Controllers
{
    public class ApplicationsController : Controller
    {
        public IActionResult Applications()
        {
            return View();
        }

        public async Task<IActionResult> SearchApplication(ApplicationSearchModel searchModel)
        {
            searchModel.faculty = await ConvertStringToList(searchModel.faculty[0]);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7122");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = HttpContext.Session.GetString("refreshToken");

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                string queryString = "";
                if (searchModel.faculty != null && searchModel.faculty.Count > 0)
                {
                    for (int i = 0; i < searchModel.faculty.Count; i++)
                    {
                        queryString += $"faculty={searchModel.faculty[i]}&";
                    }
                }

                string url = $"/api/Applications/applications?Name={searchModel.Name}&ProgramId={searchModel.ProgramId}&{queryString}AdmissionStatus={searchModel.AdmissionStatus}&hasNotManager={searchModel.hasNotManager}&myApplicants={searchModel.myApplicants}&sortByDate={searchModel.sortByDate}&Page={searchModel.Page}&Size={searchModel.Size}";

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var contentResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationPagedListModel>(content);
                    return PartialView("SearchApplications", contentResponse);
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }

            return View(searchModel);
        }

        public async Task<List<string>> ConvertStringToList(string input)
        {
            List<string> result = new List<string>();

            if (!string.IsNullOrEmpty(input))
            {
                string[] splitItems = input.Split(',');
                foreach (string item in splitItems)
                {
                    result.Add(item.Trim());
                }
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}

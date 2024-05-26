using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Handbook;
using MVC.Models.Programs;
using Newtonsoft.Json;

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

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.GetAsync(url);
                }

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var contentResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ProgramPagedListModel>(content);
                    return PartialView("SearchPrograms", contentResponse);
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
        }

        public async Task<IActionResult> CreateQuery(List<Guid> programs, Guid userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7122/");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                var json = JsonConvert.SerializeObject(programs);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string url;

                if (userId == Guid.Empty)
                {
                    url ="/api/Programs/queue";
                }
                else
                {
                    url = $"/api/Programs/queue/{userId}";
                }

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.PostAsync(url, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    var contentR = await response.Content.ReadAsStringAsync();
                    return PartialView("CreateQuery", "Success");
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

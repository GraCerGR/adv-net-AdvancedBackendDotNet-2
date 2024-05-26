using System;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.Handbook;
using MVC.Models.Programs;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class ApplicationsController : Controller
    {
        public IActionResult Applications()
        {
            return View();
        }

        public async Task<IActionResult> DeleteManager(Guid ApplicationId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7122/");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                HttpResponseMessage response = await client.DeleteAsync($"/api/Applications/{ApplicationId}/assign-manager");

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    // Повторное выполнение запроса с новым accessToken
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.DeleteAsync($"/api/Applications/{ApplicationId}/assign-manager");
                }

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return PartialView("AssignManager", content.ToString());
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
        }

        public async Task<IActionResult> SetStatus(Guid ApplicationId, Status status)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7122/");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-Token", refreshToken);

                HttpResponseMessage response = await client.PostAsync($"/api/Applications/{ApplicationId}/status?status={status}", null);

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.PostAsync($"/api/Applications/{ApplicationId}/status?status={status}", null);
                }

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return PartialView("AssignManager", content.ToString());
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
        }

        public async Task<IActionResult> DeleteApplication(Guid userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7122/");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-Token", refreshToken);

                string url;

                if (userId == Guid.Empty)
                {
                    url = $"/api/Applications/";
                }
                else
                {
                    url = $"/api/Applications/{userId}";
                }

                HttpResponseMessage response = await client.DeleteAsync(url);

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.DeleteAsync(url);
                }

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return PartialView("AssignManager", content.ToString());
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> SearchApplication(ApplicationSearchModel searchModel)
        {
            searchModel.faculty = await ConvertStringToList(searchModel.faculty[0]);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7122");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

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
        }

        public async Task<IActionResult> AssignManager(Guid userId, Guid ApplicationId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7122");
                client.DefaultRequestHeaders.Add("accept", "text/plain");
                string accessToken = Request.Cookies["accessToken"];
                string refreshToken = Request.Cookies["refreshToken"];

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Refresh-token", refreshToken);

                var json = JsonConvert.SerializeObject(userId);
                var contentM = new StringContent(json, Encoding.UTF8, "application/json");

                string url;

                if (userId == Guid.Empty)
                {
                    url = $"/api/Applications/{ApplicationId}/assign-manager";
                }
                else
                {
                    url = $"/api/Applications/{ApplicationId}/assign-manager-by?managerId={userId}";
                }

                HttpResponseMessage response = await client.PostAsync(url, null);

                if (!response.IsSuccessStatusCode && response.Headers.Contains("Authorization"))
                {
                    string newAccessToken = response.Headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);
                    Response.Cookies.Append("accessToken", newAccessToken);
                    response = await client.PostAsync(url, null);
                }

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return PartialView("AssignManager", content.ToString());
                }
                else
                {
                    Console.WriteLine("Ошибка: " + response.StatusCode);
                    ViewData["ErrorMessage"] = response.ToString();
                    return View("Error");
                }
            }
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

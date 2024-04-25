using Handbook_Service.Context;
using Handbook_Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Handbook_Service.Models;
using System.Text.Json;

namespace WebApplication1.Services
{
    public class HandbookService : IHandbookService
    {
        private readonly ApplicationContext _context;

        public HandbookService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<string> GetFaculties()
        {
            var faculties = await FetchEducationLevels("faculties");
            return faculties;
        }

        public async Task<string> GetEducationLevels()
        {
            var educationLevels = await FetchEducationLevels("education_levels");
            return educationLevels;
        }

        public async Task<string> FetchEducationLevels(string endpoint)
        {
            string url = $"https://1c-mockup.kreosoft.space/api/dictionary/{endpoint}";
            string token = "Basic c3R1ZGVudDpueTZnUW55bjRlY2JCclA5bDFGeg==";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", token);
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    throw new Exception("Failed to get data. Status code: " + response.StatusCode);
                }
            }
        }
    }
}

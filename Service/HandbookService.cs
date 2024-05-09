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
using Newtonsoft.Json;
using System.Drawing;
using System.Diagnostics.Metrics;

namespace WebApplication1.Services
{
    public class HandbookService : IHandbookService
    {
        private readonly ApplicationContext _context;

        public HandbookService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<FacultyModel>> GetFaculties()
        {
            var faculties = await FetchImport("faculties");
            var facultiesList = JsonConvert.DeserializeObject<List<FacultyModel>>(faculties);

            _context.Faculties.RemoveRange(_context.Faculties);
            await _context.SaveChangesAsync();

            foreach (var faculty in facultiesList)
            {
                _context.Faculties.Add(new FacultyModel
                {
                    Id = faculty.Id,
                    CreateTime = faculty.CreateTime.ToUniversalTime(),
                    Name = faculty.Name
                });
            }

            await _context.SaveChangesAsync();
            return facultiesList;
        }

        public async Task<List<EducationLevelModel>> GetEducationLevels()
        {
            var educationLevels = await FetchImport("education_levels");
            var educationLevelsList = JsonConvert.DeserializeObject<List<EducationLevelModel>>(educationLevels);

            _context.EducationLevels.RemoveRange(_context.EducationLevels);
            await _context.SaveChangesAsync();

            foreach (var educationLevel in educationLevelsList)
            {
                _context.EducationLevels.Add(new EducationLevelModel
                {
                    Id = educationLevel.Id,
                    Name = educationLevel.Name
                });
            }

            await _context.SaveChangesAsync();

            return educationLevelsList;
        }

        public async Task<List<EducationProgramModel>> GetPrograms()
        {
            int count = 0;
            int page = 1;
            int size = 1;
            int skip = 0;
            ProgramPagedListModel programPagedList;

            _context.EducationPrograms.RemoveRange(_context.EducationPrograms);
            await _context.SaveChangesAsync();

            do
            {
                var response = await FetchImport($"programs?page={page}&size={size}");
                programPagedList = JsonConvert.DeserializeObject<ProgramPagedListModel>(response);

                if (count == 0)
                {
                    count = programPagedList.Pagination.Count;
                }

                foreach (var program in programPagedList.Programs)
                {
                    var existingFaculty = _context.Faculties.FirstOrDefault(f => f.Id == program.Faculty.Id);
                    var existingEducationLevel = _context.EducationLevels.FirstOrDefault(el => el.Id == program.EducationLevel.Id);

                    if (existingFaculty != null && existingEducationLevel != null)
                    {
                        _context.EducationPrograms.Add(new EducationProgramModel
                        {
                            Id = program.Id,
                            CreateTime = program.CreateTime.ToUniversalTime(),
                            Name = program.Name,
                            Code = program.Code,
                            Language = program.Language,
                            EducationForm = program.EducationForm,
                            Faculty = existingFaculty,
                            EducationLevel = existingEducationLevel
                        });
                    }
                    else
                    {
                        if (existingFaculty == null) Console.WriteLine("This faculty is not loaded. Please import the new faculty database");
                        if (existingEducationLevel == null) Console.WriteLine("This level of education is not loaded. Please import a new database of education levels");
                        skip++;
                    }
                }
                Console.WriteLine($"{page} out of {count} were imported");
                await _context.SaveChangesAsync();
                Console.WriteLine($"{skip} were skipped");
                page++;

            } while (page <= count);

            return programPagedList.Programs;
        }

        public async Task<List<EducationDocumentTypeModel>> GetDocumentType()
        {
            var documentType = await FetchImport("document_types");
            var documentTypeList = JsonConvert.DeserializeObject<List<EducationDocumentTypeModel>>(documentType);

            _context.EducationDocumentTypes.RemoveRange(_context.EducationDocumentTypes);
            await _context.SaveChangesAsync();

            foreach (var document in documentTypeList)
            {
/*                var existingEducationLevel = _context.EducationLevels.FirstOrDefault(el => el.Id == document.EducationLevel.Id);
                var sdfs = document.EducationLevel.Id;
                var existingNextEducationLevels = new List<EducationLevelModel>();

                foreach (var nextLevel in document.NextEducationLevels)
                {
                    var existingNextEducationLevel = _context.EducationLevels.FirstOrDefault(el => el.Id == nextLevel.Id);
                    if (existingNextEducationLevel == null)
                    {
                    }
                }*/

                _context.EducationDocumentTypes.Add(new EducationDocumentTypeModel
                {
                    Id = document.Id,
                    CreateTime = document.CreateTime.ToUniversalTime(),
                    Name = document.Name,
                    EducationLevel = document.EducationLevel,
                    //NextEducationLevels = document.NextEducationLevels
                });
            }

            await _context.SaveChangesAsync();

            return documentTypeList;
        }

        public async Task<string> FetchImport(string endpoint)
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

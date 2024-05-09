using Handbook_Service.Models;
using Handbook_Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Handbook_Service.Services.Interfaces
{
    public interface IHandbookService
    {
        Task<List<FacultyModel>> GetFaculties();
        Task<List<EducationLevelModel>> GetEducationLevels();
        Task<List<EducationProgramModel>> GetPrograms();
        Task<List<EducationDocumentTypeModel>> GetDocumentType();
    }
}

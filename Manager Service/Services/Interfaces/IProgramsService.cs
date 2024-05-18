using Handbook_Service.Models;
using Manager_Service.Models;
using Manager_Service.Models.DTO;

namespace Manager_Service.Services.Interfaces
{
    public interface IProgramsService
    {
        Task<Handbook_Service.Models.ProgramPagedListModel> GetPrograms(ProgramSearchModel programSearchModel);
    }
}

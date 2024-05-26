using Manager_Service.Models;
using Manager_Service.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using User_Service.Models.DTO;

namespace Manager_Service.Services.Interfaces
{
    public interface IApplicationsService
    {
        Task<ApplicationModel> CreateApplication(Guid userId);

        Task DeleteApplication(Guid userId, Guid? managerId);

        Task<ApplicationPagedListModel> GetApplication(ApplicationSearchModel applicationSearchModel, Guid userId);

        Task<List<MessageDto>> ManagerApplication(Guid applicationId, Guid managerId);

        Task<List<MessageDto>> DeleteManagerApplication(Guid applicationId, Guid managerId);

        Task<MessageDto> SetStatus(Guid appplicationId, Guid managerId, Status status);
    }
}

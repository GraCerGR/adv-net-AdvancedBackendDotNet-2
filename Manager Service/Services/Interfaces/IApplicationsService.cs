using Manager_Service.Models;
using Manager_Service.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Manager_Service.Services.Interfaces
{
    public interface IApplicationsService
    {
        Task<ApplicationModel> CreateApplication(Guid userId);

        Task DeleteApplication(Guid userId, Guid? managerId);

        Task<ApplicationPagedListModel> GetApplication(ApplicationSearchModel applicationSearchModel, Guid userId);

        Task ManagerApplication(Guid applicationId, Guid managerId);

        Task DeleteManagerApplication(Guid applicationId, Guid managerId);

        Task SetStatus(Guid appplicationId, Guid managerId, Status status);
    }
}

using Manager_Service.Models;
using Manager_Service.Models.DTO;

namespace Manager_Service.Services.Interfaces
{
    public interface IManagerService
    {
        Task<ManagerCreateModel[]> CreateManagers(ManagerCreateModel[] managerCreateModel);
    }
}

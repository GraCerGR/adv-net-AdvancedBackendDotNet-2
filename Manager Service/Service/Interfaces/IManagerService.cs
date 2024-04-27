using Manager_Service.Models;
using Manager_Service.Models.DTO;

namespace WebApplication1.Services.Interfaces
{
    public interface IManagerService
    {
        Task<ManagerCreateModel[]> CreateManagers(ManagerCreateModel[] managerCreateModel);
    }
}

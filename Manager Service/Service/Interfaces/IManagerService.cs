using Manager_Service.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Services.Interfaces
{
    public interface IManagerService
    {
        Task<ManagerCreateModel[]> CreateManagers(ManagerCreateModel[] managerCreateModel);
    }
}

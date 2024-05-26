using User_Service.Models;
using User_Service.Models.DTO;

namespace User_Service.Services.Interfaces
{
    public interface IManagerService
    {
        Task<ManagerCreateModel[]> CreateManagers(ManagerCreateModel[] managerCreateModel);

        Task<List<ManagerDto>> GetManagers();
    }
}

using Document_Service.Models;
using Document_Service.Models.DTO;

namespace Document_Service.Services.Interfaces
{
    public interface IManageFileService
    {
        Task<string> UploadFile(IFormFile _IFormFile, FileTypes type , PassportDto passport, Guid UserId);

        Task<(byte[], string, string)> DownloadFile(string FileName);
    }
}

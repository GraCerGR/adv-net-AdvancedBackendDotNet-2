using Document_Service.Models;
using Document_Service.Models.DTO;

namespace Document_Service.Services.Interfaces
{
    public interface IManageFileService
    {
        Task<string> UploadFile(IFormFile _IFormFile, FileTypes type , Guid UserId, PassportDto? passport, string? educationType);

        Task<(byte[], string, string)> DownloadFile(string FileName);
    }
}

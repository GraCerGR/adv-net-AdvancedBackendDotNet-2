namespace Document_Service.Services.Interfaces
{
    public interface IManageFile
    {
        Task<string> UploadFile(IFormFile _IFormFile);

        Task<(byte[], string, string)> DownloadFile(string FileName);
    }
}

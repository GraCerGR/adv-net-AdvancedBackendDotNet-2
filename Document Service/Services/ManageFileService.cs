using System.Security.AccessControl;
using Document_Service.Services.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using Document_Service.Helper;
using Document_Service.Models.DTO;
using Document_Service.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;

namespace Document_Service.Services
{
    public class ManageFileService: IManageFileService
    {

        private readonly ApplicationContext _context;

        public ManageFileService(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<string> UploadFile(IFormFile _IFormFile, FileTypes fileType, PassportDto passport, Guid UserId)
        {

/*            var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Email == credentials.Email);

            if (userEntity == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(), "User not exists");
                throw ex;
            }*/

             var prevPassport = await _context.PassportFiles.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (prevPassport != null)
            {
                File.Delete(prevPassport.PathToFile);
                _context.PassportFiles.Remove(prevPassport);
                await _context.SaveChangesAsync();
            }
            
            //-------------------------Загрузка файла----------------------
            string FileName = "";
            try
            {
                FileInfo _FileInfo = new FileInfo(_IFormFile.FileName);
                FileName = _IFormFile.FileName + "_" + DateTime.Now.Ticks.ToString() + _FileInfo.Extension;
                var _GetFilePath = Common.GetFilePath(FileName);
                using (var _FileStream = new FileStream(_GetFilePath, FileMode.Create))
                {
                    await _IFormFile.CopyToAsync(_FileStream);
                }

            //--------------------------------------------------------

                if (fileType == FileTypes.Passport)
                {
                    FilePassportModel passportFile = new FilePassportModel
                    {
                        Id = Guid.NewGuid(),
                        PathToFile = _GetFilePath,
                        UserId = UserId,
                        SeriesNumber = passport.SeriesNumber,
                        Birthplace = passport.Birthplace,
                        WhenIssued = passport.WhenIssued,
                        IssuedByWhom = passport.IssuedByWhom
                    };
                    await _context.PassportFiles.AddAsync(passportFile);
                    await _context.SaveChangesAsync();

                }


                return FileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<(byte[], string, string)> DownloadFile(string FileName)
        {
            try
            {
                var _GetFilePath = Common.GetFilePath(FileName);
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(_GetFilePath, out var _ContentType))
                {
                    _ContentType = "application/octet-stream";
                }
                var _ReadAllBytesAsync = await File.ReadAllBytesAsync(_GetFilePath);
                return (_ReadAllBytesAsync, _ContentType, Path.GetFileName(_GetFilePath));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Document_Service.Services.Interfaces;
using Document_Service.Services;
using Document_Service.Models.DTO;
using Document_Service.Models;
using System.ComponentModel.DataAnnotations;

namespace Document_Service.Controllers
{
    [Route("document/[controller]")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        private readonly IManageFileService _iManageFile;
        public FileManagerController(IManageFileService iManageFile)
        {
            _iManageFile = iManageFile;
        }

        [HttpPost]
        [Route("uploadPassport")]
        public async Task<IActionResult> UploadPassportFile(IFormFile _IFormFile, [FromForm] PassportDto passport,[Required] Guid UserId)
        {
            var result = await _iManageFile.UploadFile(_IFormFile, FileTypes.Passport, passport, UserId);
            return Ok(result);
        }

        /*        [HttpPost]
                [Route("uploadEducation")]
                public async Task<IActionResult> UploadEducationFile(IFormFile _IFormFile)
                {
                    var result = await _iManageFile.UploadFile(_IFormFile);
                    return Ok(result);
                }*/

        [HttpGet]
        [Route("downloadfile")]
        public async Task<IActionResult> DownloadFile(string FileName)
        {
            var result = await _iManageFile.DownloadFile(FileName);
            return File(result.Item1, result.Item2, result.Item2);
        }
    }
}

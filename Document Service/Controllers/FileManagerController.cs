using Microsoft.AspNetCore.Mvc;
using Document_Service.Services.Interfaces;
using Document_Service.Services;

namespace Document_Service.Controllers
{
    [Route("document/[controller]")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        private readonly IManageFile _iManageFile;
        public FileManagerController(IManageFile iManageFile)
        {
            _iManageFile = iManageFile;
        }

        [HttpPost]
        [Route("uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile _IFormFile)
        {
            var result = await _iManageFile.UploadFile(_IFormFile);
            return Ok(result);
        }

        [HttpGet]
        [Route("downloadfile")]
        public async Task<IActionResult> DownloadFile(string FileName)
        {
            var result = await _iManageFile.DownloadFile(FileName);
            return File(result.Item1, result.Item2, result.Item2);
        }
    }
}

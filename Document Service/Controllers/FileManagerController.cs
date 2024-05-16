using Microsoft.AspNetCore.Mvc;
using Document_Service.Services.Interfaces;
using Document_Service.Services;
using Document_Service.Models.DTO;
using Document_Service.Models;
using System.ComponentModel.DataAnnotations;
using User_Service.Services;
using User_Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Document_Service.Controllers
{
    [Route("document/[controller]")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        private readonly IManageFileService _iManageFile;
        private readonly IUserService _userService;
        public FileManagerController(IManageFileService iManageFile, IUserService userService)
        {
            _iManageFile = iManageFile;
            _userService = userService;
        }
        

        [HttpPost]
        [Authorize]
        [Route("uploadPassport")]
        public async Task<IActionResult> UploadPassportFile(IFormFile _IFormFile, [FromForm] PassportDto passport,[Required] Guid UserId)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var userId = await _userService.GetUserIdFromToken(bearerToken);

            var result = await _iManageFile.UploadFile(_IFormFile, FileTypes.Passport, UserId, passport, null);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("uploadEducation")]
        public async Task<IActionResult> UploadEducationFile(IFormFile _IFormFile, [Required] Guid UserId, [Required] string type)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var userId = await _userService.GetUserIdFromToken(bearerToken);

            var result = await _iManageFile.UploadFile(_IFormFile, FileTypes.EducationFile, UserId, null, type);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize]
        [Route("uploadEducation")]
        public async Task<IActionResult> DeleteFile(string FileName, [Required] Guid UserId, string type)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var userId = await _userService.GetUserIdFromToken(bearerToken);

            var result = await _iManageFile.DeleteFile(FileName, UserId, type);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("downloadfile")]
        public async Task<IActionResult> DownloadFile(string FileName)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var userId = await _userService.GetUserIdFromToken(bearerToken);

            var result = await _iManageFile.DownloadFile(FileName);
            return File(result.Item1, result.Item2, result.Item2);
        }
    }
}

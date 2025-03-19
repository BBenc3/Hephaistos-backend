using Microsoft.AspNetCore.Mvc;
using ProjectHephaistos.Services;

namespace ProjectHephaistos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FtpController : ControllerBase
    {
        private readonly FtpService _ftpService;

        public FtpController(FtpService ftpService)
        {
            _ftpService = ftpService;
        }

        [HttpGet("list")]
        public IActionResult ListDirectory(string remotePath)
        {
            try
            {
                var filesAndDirectories = _ftpService.ListDirectory(remotePath);
                return Ok(filesAndDirectories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("list-all")]
        public IActionResult ListAllFilesAndDirectories()
        {
            try
            {
                var filesAndDirectories = _ftpService.ListDirectory("");
                return Ok(filesAndDirectories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

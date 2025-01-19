using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class FtpController : ControllerBase
{
    private readonly FtpConfig _ftpConfig;

    public FtpController(IOptions<FtpConfig> ftpConfig)
    {
        _ftpConfig = ftpConfig.Value;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileToFtp([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file was uploaded.");

        if (string.IsNullOrWhiteSpace(_ftpConfig.Url) ||
            string.IsNullOrWhiteSpace(_ftpConfig.Username) ||
            string.IsNullOrWhiteSpace(_ftpConfig.Password))
        {
            return BadRequest("FTP configuration is invalid.");
        }

        try
        {
            string targetFilePath = _ftpConfig.Url + file.FileName;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] fileContents = memoryStream.ToArray();

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(targetFilePath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_ftpConfig.Username, _ftpConfig.Password);
                request.UseBinary = true;
                request.Timeout = 30000;

                using (Stream requestStream = request.GetRequestStream())
                {
                    await requestStream.WriteAsync(fileContents);
                }

                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    return Ok(new { message = "Upload successful", status = response.StatusDescription });
                }
            }
        }
        catch (WebException webEx) when (webEx.Response is FtpWebResponse ftpResponse)
        {
            return StatusCode(500, new { message = "FTP error occurred.", error = ftpResponse.StatusDescription });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }
}


public class FtpConfig
{
    public string Url { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

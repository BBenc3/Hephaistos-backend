using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class BackendController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public BackendController(IWebHostEnvironment env)
    {
        _env = env;
    }
    [HttpPost("Upload")]
    [Authorize]
    public async Task<IActionResult> UploadFileToBackend([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file was uploaded.");

        try
        {
            // Define the folder where the file will be stored
            string subFolder = "files";
            string uploadsPath = Path.Combine(_env.ContentRootPath, subFolder);

            // Ensure the directory exists
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Create the full file path
            string filePath = Path.Combine(uploadsPath, file.FileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream); // Await the asynchronous file copy
            }

            // Return success response
            return Ok(new { message = "File uploaded successfully.", filePath = filePath });
        }
        catch (Exception ex)
        {
            // Log the error if necessary and return a generic error message
            return StatusCode(500, new { message = "An error occurred while uploading the file.", error = ex.Message });
        }
    }
}

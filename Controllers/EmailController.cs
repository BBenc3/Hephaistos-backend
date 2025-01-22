using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize]  // This applies authorization globally to all actions
public class EmailController : ControllerBase
{
    private readonly EmailService _emailService;

    public EmailController(EmailService emailService)
    {
        _emailService = emailService;
    }

    // Endpoint that accepts an email address as a URL parameter
    [HttpGet("send-email/{emailAddress}")]
    public async Task<IActionResult> SendEmail(string emailAddress)
    {
        if (string.IsNullOrEmpty(emailAddress))
        {
            return BadRequest("Email address is required.");
        }

        string subject = "Auto generated email";
        string body = "<h1>Ez egy automatikusan generált email, ne válaszoljon rá.</h1>";

        // Send the email using the provided email address
        await _emailService.SendEmailAsync(User.Claims.FirstOrDefault(c => c.Type == "email")?.Value, subject, body);

        return Ok($"Email sent to {emailAddress} successfully!");
    }
}

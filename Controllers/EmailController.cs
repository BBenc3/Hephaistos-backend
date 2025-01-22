using Microsoft.AspNetCore.Mvc; 
using System.IdentityModel.Tokens.Jwt;
using ProjectHephaistos.Data;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class EmailController : ControllerBase
{
    private readonly HephaistosContext _context;
    private readonly EmailService _emailService;
    private readonly JwtHelper _jwtHelper;
    public EmailController(HephaistosContext context, EmailService emailService, JwtHelper jwtHelper)
    {
        _context = context;
        _emailService = emailService;
        _jwtHelper = jwtHelper;
    }
 
    [HttpGet("send-email")]
    public async Task<IActionResult> SendEmail([FromHeader(Name = "Authorization")] string authorization)
    {
        try
        {
            // Step 1: Extract the token from the Authorization header
            var token = authorization.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing.");
            }

            // Step 2: Extract user ID from token
            var userId = _jwtHelper.ExtractUserIdFromToken(token);

            if (!userId.HasValue)
            {
                return Unauthorized("Invalid or expired token.");
            }

            // Step 3: Retrieve user from database
            var user = _context.Users.FirstOrDefault(c => c.Id == userId.Value);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Step 4: Retrieve user's email
            string userEmail = user.Email;

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("User email not found.");
            }

            // Step 5: Send email
            string subject = "Auto generated email";
            string body = "<h1>This is an automatically generated email. Please do not reply to it.</h1>";

            await _emailService.SendEmailAsync(userEmail, subject, body);
            return Ok($"Email sent to {userEmail} successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while sending the email.");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace AIAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure only authenticated users can access this endpoint
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _env;

        public StudentController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterStudent([FromBody] StudentRegistrationModel model)
        {




            // Step 1: Authenticate and authorize the user
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated.");
            }

            // Step 2: Process payment using Razorpay
            var paymentSuccess = await ProcessPayment(model.PaymentId);
            if (!paymentSuccess)
            {
                return BadRequest("Payment failed.");
            }

            // Step 3: Generate Webex meeting link
            var webexMeetingLink = await GenerateWebexMeetingLink();

            // Step 4: Send email with Webex meeting link
            await SendEmail(model.Email, webexMeetingLink);

            return Ok(new { Message = "Registration successful!", WebexMeetingLink = webexMeetingLink });
        }

        private async Task<bool> ProcessPayment(string paymentId)
        {
            var razorpayKey = _configuration["Razorpay:Key"];
            var razorpaySecret = _configuration["Razorpay:Secret"];

            RazorpayClient client = new RazorpayClient(razorpayKey, razorpaySecret);

            try
            {
                Payment payment = client.Payment.Fetch(paymentId);
                if (payment["status"] == "captured")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Payment processing failed: {ex.Message}");
            }

            return false;
        }

        private async Task<string> GenerateWebexMeetingLink()
        {
            var webexApiKey = _configuration["Webex:ApiKey"];
            var webexRoomId = _configuration["Webex:RoomId"];

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", webexApiKey);

                var requestBody = new
                {
                    title = "Use AI Course - 26 Hours",
                    start = DateTime.UtcNow.AddDays(1).ToString("o"), // Start tomorrow
                    end = DateTime.UtcNow.AddDays(1).AddHours(26).ToString("o"), // 26-hour duration
                    roomId = webexRoomId
                };

                var response = await httpClient.PostAsJsonAsync("https://webexapis.com/v1/meetings", requestBody);
                if (response.IsSuccessStatusCode)
                {
                    var meeting = await response.Content.ReadFromJsonAsync<WebexMeetingResponse>();
                    return meeting.WebLink;
                }
            }

            throw new Exception("Failed to generate Webex meeting link.");
        }

        private async Task SendEmail(string email, string webexMeetingLink)
        {
            var smtpHost = _configuration["Email:SmtpHost"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
            var smtpUsername = _configuration["Email:SmtpUsername"];
            var smtpPassword = _configuration["Email:SmtpPassword"];

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("noreply@usecourse.com"),
                    Subject = "Your Webex Meeting Link for Use AI Course",
                    Body = $"Dear Student,<br><br>Your registration is confirmed. Here is your Webex meeting link: <a href='{webexMeetingLink}'>{webexMeetingLink}</a>.<br><br>Best regards,<br>Use AI Course Team",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }

    public class StudentRegistrationModel
    {
        public string Email { get; set; }
        public string PaymentId { get; set; }
    }

    public class WebexMeetingResponse
    {
        public string WebLink { get; set; }
    }
}
// Controllers/RegistrationController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QRCoder;
//using QuestPDF.Fluent;

//using QuestPDF.Helpers;
//using QuestPDF.Infrastructure;
//using System.Reflection.Metadata;
using System.Text;

using Microsoft.EntityFrameworkCore;
using AIAcademy.Model;
using System.Net.Http;



namespace AIAcademy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly WebinarDbContext _context;
    private readonly ILogger<RegistrationController> _logger;
    private IWebHostEnvironment _env;
   
    public RegistrationController(WebinarDbContext context, ILogger<RegistrationController> logger, IWebHostEnvironment env)
    {
        _context = context;
        _logger = logger;
        _env = env;
      

        // Initialize QuestPDF (only needed once at startup)
        //QuestPDF.Settings.License = LicenseType.Community;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WebinarDto>>> GetWebinars()
    {
        try
        {
            // Read all webinars from CSV
            var allWebinars = WebinarCSVContext.ReadDataFromCSV(Path.Combine(_env.ContentRootPath, "Dataset", "webinar.csv"));

            // Get current UTC date (without time component) for accurate comparison
            var currentUtcDate = DateTime.UtcNow.Date;

            // Filter webinars from yesterday onward and order by date
            var filteredWebinars = allWebinars
                .Where(w => w.Date.Date >= currentUtcDate.AddDays(-1)) // Include webinars from yesterday
                .OrderBy(w => w.Date)
                .Select(w => new WebinarDto
                {
                    Id = w.Id,
                    Topic = w.Topic,
                    Description = w.Description,
                    Date = w.Date,
                    Time = w.Time,
                    Speaker = w.Speaker,
                    VideoUrl = "",
                    WebexUrl = w.WebexUrl,
                    WebexMeetingId = w.WebexMeetingId,
                    WebexPasscode = w.WebexPasscode
                })
                .ToList(); // Materialize the query to execute it now

            return Ok(filteredWebinars);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving webinars");
            return StatusCode(500, "Error retrieving webinar data");
        }
    }
    [HttpPost]
    [Route("webinar-registration")]
    public async Task<IActionResult> RegisterForWebinar([FromBody] Registration registration)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        //var webinar = await _context.Webinars.FindAsync(registration.WebinarId);
        //if (webinar == null)
        //{
        //    return NotFound("Webinar not found");
        //}

        //_context.Registrations.Add(registration);
        //await _context.SaveChangesAsync();
        var webinar = WebinarCSVContext.ReadDataFromCSV(Path.Combine(_env.ContentRootPath, "Dataset", "webinar.csv"));

        _logger.LogInformation($"New registration for webinar {webinar[0].Topic} by {registration.StudentName}");


         var pdfBytes = GenerateWebinarPdf(webinar[0], registration);
        // These IDs come from inspecting the Google Form HTML
        var formFields = new Dictionary<string, string>
        {
            // Map your model properties to Google Form field IDs
            // These are the 'entry.' values from the form HTML
            {"entry.128722308", registration.StudentName},        // Full Name
            {"entry.1531374950", registration.Email},             // Email Address
            {"entry.686306249", registration.Phone ?? ""},        // Phone Number
            {"entry.190390107", ""},                             // Organization/Affiliation (empty in your sample)
            {"entry.1225619309", ""}                             // Professional Experience (empty in your sample)
        };

        // The form action URL from the HTML
        var formUrl = "https://docs.google.com/forms/d/e/1FAIpQLSdN-fx3gE_B1WSuiqwQHWGHB2VC7K4reF8RMyZ9NGC0WeHcYA/viewform?usp=header";
        var content = new FormUrlEncodedContent(formFields);
        HttpClient _httpClient = new HttpClient();   
        try
        {
            var response = await _httpClient.PostAsync(formUrl, content);
            
        }
        catch(Exception ex)
        {
            BadRequest(ex.Message);
        }

        return File(pdfBytes, "text/html", $"WebinarConfirmation_{registration.Id}.html");
    }
    [HttpPost]
    public byte[] GenerateWebinarPdf([FromQuery] Webinar webinar,[FromBody] Registration registration)
    {
        // 1. Generate the HTML content with all styling
        string htmlContent = GenerateWebinarHtml(webinar, registration);

        // 2. Convert HTML to PDF
        //var converter = new BasicConverter(new PdfTools());
        //var doc = new HtmlToPdfDocument()
        //{
        //    GlobalSettings = {
        //        ColorMode = ColorMode.Color,
        //        Orientation = Orientation.Portrait,
        //        PaperSize = PaperKind.A4,
        //        Margins = new MarginSettings() { Top = 20, Bottom = 20, Left = 20, Right = 20 }
        //    },
        //    Objects = {
        //        new ObjectSettings() {
        //            HtmlContent = htmlContent,
        //            WebSettings = { DefaultEncoding = "utf-8" },
        //            HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
        //            FooterSettings = { FontSize = 9, Right = "© " + DateTime.Now.Year + " Webinar Registration System" }
        //        }
        //    }
        //};
        // return converter.Convert(doc);
        return Encoding.UTF8.GetBytes(htmlContent);
    }

    
    private string GenerateWebinarHtml(Webinar webinar, Registration registration)
    {
        // Generate QR code
        var qrGenerator = new QRCodeGenerator();

        string webinarData1 = $"""   
                              AI ACADEMY
                              Topic     :   {webinar.Topic}
                              Date      :   {webinar.Date}
                              Time      :   {webinar.Time}
                              WebexID   :   {webinar.WebexMeetingId}
                              Passcode  :   {webinar.WebexPasscode}
                              WebexUrl  :   {webinar.WebexUrl}
                              Dr. M.
                              """;

        Random random = new Random(1);

       
        string webinarDataFuturistic2 = $"""
                     █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█
                     │  ⚡ AI ACADEMY ⚡  │
                     █══════════════════█
                     │ TOPIC ⚡ {webinar.Topic,-25} │
                     │ DATE  ⚡ {webinar.Date:yyyy-MM-dd} ⌛ {DateTime.Now:HHmm}Z │
                     │ TIME  ⚡ {webinar.Time,-25} │
                     │ ID    ⚡ {webinar.WebexMeetingId,-25} │
                     │ PASS  ⚡ {webinar.WebexPasscode,-25} │
                     │ URL   ⚡ {webinar.WebexUrl,-25} │
                     █▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█
                     │   DR. M.  ⚡  VERIFIED  │
                     ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀
                     [ SCANNED HOLO-ACCESS ]
                     [ EXPIRES NEVER ]
                     """;

        string webinarDataFuturistic3 = $"""
              \u001b[38;2;0;255;255m\u001b[48;2;0;0;80m█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█
              \u001b[38;2;255;255;0m│  ⚡ \u001b[1mAI ACADEMY\u001b[0m\u001b[38;2;255;255;0m ⚡  │
              \u001b[38;2;0;255;255m█══════════════════█
              \u001b[38;2;0;255;0m│ TOPIC ⚡ \u001b[38;2;255;255;255m{webinar.Topic,-25} \u001b[38;2;0;255;0m│
              \u001b[38;2;0;255;0m│ DATE  ⚡ \u001b[38;2;255;255;255m{webinar.Date:yyyy-MM-dd} \u001b[38;2;255;165;0m⌛ {DateTime.Now:HHmm}Z \u001b[38;2;0;255;0m│
              \u001b[38;2;0;255;0m│ TIME  ⚡ \u001b[38;2;255;255;255m{webinar.Time,-25} \u001b[38;2;0;255;0m│
              \u001b[38;2;0;255;0m│ ID    ⚡ \u001b[38;2;255;255;255m{webinar.WebexMeetingId,-25} \u001b[38;2;0;255;0m│
              \u001b[38;2;0;255;0m│ PASS  ⚡ \u001b[38;2;255;255;255m{webinar.WebexPasscode,-25} \u001b[38;2;0;255;0m│
              \u001b[38;2;0;255;0m│ URL   ⚡ \u001b[4m\u001b[38;2;135;206;250m{webinar.WebexUrl,-25}\u001b[0m\u001b[38;2;0;255;0m\u001b[48;2;0;0;80m │
              \u001b[38;2;0;255;255m█▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█
              \u001b[38;2;255;255;255m│   \u001b[38;2;0;255;255mDR. M.  ⚡  \u001b[38;2;0;255;0mVERIFIED  │
              \u001b[38;2;0;255;255m▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀
              \u001b[38;2;200;200;200m[ \u001b[38;2;0;255;255mSCANNED HOLO-ACCESS \u001b[38;2;200;200;200m]
              \u001b[38;2;200;200;200m[ \u001b[38;2;255;50;50mEXPIRES NEVER \u001b[38;2;200;200;200m]
              \u001b[0m
              """;
        var qrData = qrGenerator.CreateQrCode(webinarDataFuturistic3, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new Base64QRCode(qrData);
        var qrCodeImageBase64 = qrCode.GetGraphic(20);

        var html = new StringBuilder();

        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html lang=\"en\">");
        html.AppendLine("<head>");
        html.AppendLine("    <meta charset=\"UTF-8\">");
        html.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        html.AppendLine("    <title>Webinar Registration Confirmation</title>");
        html.AppendLine("    <link href='https://fonts.googleapis.com/css?family=Orbitron:400,500,700|Rajdhani:400,500,600' rel='stylesheet'>");
        html.AppendLine("    <style>");
        html.AppendLine(@"
:root {
    --neon-blue: #0ff0fc;
    --neon-pink: #ff00ff;
    --neon-purple: #bc13fe;
    --neon-green: #00ff88;
    --dark-bg: #0a0a1a;
    --darker-bg: #050510;
    --glow-blue: 0 0 10px rgba(0, 255, 252, 0.7);
    --glow-pink: 0 0 10px rgba(255, 0, 255, 0.7);
    --glow-purple: 0 0 10px rgba(188, 19, 254, 0.7);
}
/* New CSS for sound*/
.sound-visualizer {
    position: fixed;
    bottom: 20px;
    right: 20px;
    width: 60px;
    height: 60px;
    border-radius: 50%;
    background: rgba(10, 10, 26, 0.7);
    border: 1px solid var(--neon-blue);
    display: flex;
    justify-content: center;
    align-items: center;
    cursor: pointer;
    z-index: 100;
    box-shadow: 0 0 20px rgba(0, 255, 252, 0.3);
    transition: all 0.3s ease;
}

.sound-visualizer:hover {
    transform: scale(1.1);
    box-shadow: 0 0 30px rgba(0, 255, 252, 0.5);
}

.sound-visualizer i {
    color: var(--neon-blue);
    font-size: 24px;
}

.sound-visualizer.active {
    background: rgba(0, 255, 252, 0.1);
    box-shadow: 0 0 30px rgba(0, 255, 252, 0.7);
}

.sound-wave {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 40px;
    margin: 10px 0;
    gap: 3px;
}

.sound-bar {
    width: 4px;
    height: 10px;
    background: var(--neon-blue);
    border-radius: 2px;
    animation: sound-wave 1.5s infinite ease-in-out;
    transform-origin: bottom;
}

.sound-bar:nth-child(1) { animation-delay: 0.1s; }
.sound-bar:nth-child(2) { animation-delay: 0.2s; }
.sound-bar:nth-child(3) { animation-delay: 0.3s; }
.sound-bar:nth-child(4) { animation-delay: 0.4s; }
.sound-bar:nth-child(5) { animation-delay: 0.5s; }
.sound-bar:nth-child(6) { animation-delay: 0.6s; }
.sound-bar:nth-child(7) { animation-delay: 0.7s; }
.sound-bar:nth-child(8) { animation-delay: 0.8s; }

@keyframes sound-wave {
    0%, 100% { transform: scaleY(0.3); }
    50% { transform: scaleY(2); }
}


/* Futuristic Holographic Badge */
.holographic-badge {
    width: 200px;
    height: 200px;
    margin: 0 auto 20px;
    position: relative;
    perspective: 1000px;
    transform-style: preserve-3d;
    animation: float 6s ease-in-out infinite;
}

.holo-circle {
    position: absolute;
    width: 100%;
    height: 100%;
    border: 2px solid var(--neon-blue);
    border-radius: 50%;
    animation: rotate 20s linear infinite;
    box-shadow: 0 0 20px rgba(0, 255, 252, 0.3),
                inset 0 0 20px rgba(0, 255, 252, 0.3);
}

.holo-circle-2 {
    border-color: var(--neon-pink);
    animation-delay: -5s;
    animation-direction: reverse;
    box-shadow: 0 0 20px rgba(255, 0, 255, 0.3),
                inset 0 0 20px rgba(255, 0, 255, 0.3);
    transform: rotateX(60deg);
}

.holo-circle-3 {
    border-color: var(--neon-purple);
    animation-delay: -10s;
    box-shadow: 0 0 20px rgba(188, 19, 254, 0.3),
                inset 0 0 20px rgba(188, 19, 254, 0.3);
    transform: rotateY(60deg);
}

@keyframes rotate {
    0% { transform: rotateX(0) rotateY(0) rotateZ(0); }
    100% { transform: rotateX(360deg) rotateY(360deg) rotateZ(360deg); }
}

.qr-code {
    position: relative;
    z-index: 2;
    width: 140px;
    height: 140px;
    margin: 30px auto;
    background: rgba(0, 0, 0, 0.7);
    border: none;
    backdrop-filter: blur(5px);
    box-shadow: 0 0 30px rgba(0, 255, 252, 0.5);
}

.qr-image {
    filter: hue-rotate(0deg) brightness(1.2) drop-shadow(0 0 5px rgba(0, 255, 252, 0.7));
    animation: hue-rotate 10s linear infinite;
}

@keyframes hue-rotate {
    0% { filter: hue-rotate(0deg) brightness(1.2) drop-shadow(0 0 5px rgba(0, 255, 252, 0.7)); }
    100% { filter: hue-rotate(360deg) brightness(1.2) drop-shadow(0 0 5px rgba(255, 0, 255, 0.7)); }
}

.verification-beam {
    position: absolute;
    bottom: -20px;
    left: 50%;
    transform: translateX(-50%);
    width: 2px;
    height: 0;
    background: linear-gradient(to top, rgba(0, 255, 252, 0.8), transparent);
    box-shadow: 0 0 10px var(--neon-blue);
    z-index: 1;
    animation: beam 3s ease-in-out infinite;
}

@keyframes beam {
    0%, 100% { height: 0; opacity: 0; }
    50% { height: 50px; opacity: 1; }
}

.verification-status {
    position: absolute;
    bottom: -40px;
    left: 0;
    width: 100%;
    text-align: center;
    z-index: 3;
}

.verification-text {
    color: var(--neon-green);
    font-size: 14px;
    font-weight: 700;
    letter-spacing: 2px;
    text-transform: uppercase;
    text-shadow: 0 0 10px rgba(0, 255, 136, 0.7);
    display: block;
}

.verification-dots {
    display: flex;
    justify-content: center;
    gap: 5px;
    margin-top: 5px;
}

.dot {
    width: 8px;
    height: 8px;
    border-radius: 50%;
    background: var(--neon-green);
    opacity: 0;
    animation: dot-pulse 1.5s infinite ease-in-out;
    box-shadow: 0 0 5px rgba(0, 255, 136, 0.7);
}

.dot:nth-child(1) { animation-delay: 0s; }
.dot:nth-child(2) { animation-delay: 0.3s; }
.dot:nth-child(3) { animation-delay: 0.6s; }

@keyframes dot-pulse {
    0%, 100% { opacity: 0.2; transform: scale(0.8); }
    50% { opacity: 1; transform: scale(1.2); }
}

.scan-animation {
    width: 200px;
    height: 10px;
    margin: 20px auto 0;
    position: relative;
    overflow: hidden;
    border-radius: 5px;
    background: rgba(0, 255, 252, 0.1);
}

.scan-line {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: linear-gradient(to right, 
                transparent, 
                rgba(0, 255, 252, 0.8), 
                transparent);
    animation: scan 2s linear infinite;
}

@keyframes scan {
    0% { transform: translateX(-100%); }
    100% { transform: translateX(100%); }
}

.qr-instruction {
    font-size: 14px;
    letter-spacing: 3px;
    margin-top: 30px;
    text-transform: uppercase;
    position: relative;
    color: var(--neon-blue);
    text-shadow: 0 0 10px rgba(0, 255, 252, 0.5);
}

.qr-instruction::before,
.qr-instruction::after {
    content: '✧';
    margin: 0 15px;
    color: var(--neon-pink);
}
@keyframes float {
    0%, 100% { transform: translateY(0); }
    50% { transform: translateY(-10px); }
}

@keyframes pulse {
    0%, 100% { opacity: 1; }
    50% { opacity: 0.5; }
}

@keyframes scanline {
    0% { transform: translateY(-100%); }
    100% { transform: translateY(100%); }
}

@keyframes flicker {
    0%, 19%, 21%, 23%, 25%, 54%, 56%, 100% {
        text-shadow: 0 0 5px var(--neon-blue), var(--glow-blue);
    }
    20%, 24%, 55% {
        text-shadow: none;
    }
}

@keyframes particle-move {
    0% { transform: translate(0, 0); opacity: 1; }
    100% { transform: translate(var(--tx), var(--ty)); opacity: 0; }
}

body {
    font-family: 'Orbitron', sans-serif;
    background-color: var(--dark-bg);
    color: #fff;
    overflow-x: hidden;
    background-image: 
        radial-gradient(circle at 10% 20%, rgba(11, 11, 41, 0.8) 0%, transparent 20%),
        radial-gradient(circle at 90% 80%, rgba(92, 12, 92, 0.6) 0%, transparent 20%);
    padding: 20px;
    position: relative;
}

body::before {
    content: '';
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: linear-gradient(
        rgba(188, 19, 254, 0.1) 1px,
        transparent 1px
    );
    background-size: 100% 3px;
    pointer-events: none;
    z-index: -1;
    animation: scanline 8s linear infinite;
}

.particle {
    position: absolute;
    width: 2px;
    height: 2px;
    background: var(--neon-blue);
    border-radius: 50%;
    pointer-events: none;
    opacity: 0;
    animation: particle-move 2s linear forwards;
}

.confirmation-container {
    max-width: 800px;
    margin: 40px auto;
    background: rgba(10, 10, 26, 0.9);
    border-radius: 15px;
    overflow: hidden;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
    border: 1px solid rgba(188, 19, 254, 0.3);
    backdrop-filter: blur(5px);
    position: relative;
    transform: perspective(1000px) rotateX(5deg);
    transition: transform 0.5s ease;
}

.confirmation-container:hover {
    transform: perspective(1000px) rotateX(0deg);
    box-shadow: 0 0 50px rgba(188, 19, 254, 0.5);
}

.confirmation-header {
    background: linear-gradient(90deg, rgba(10, 10, 26, 0.8), rgba(92, 12, 92, 0.6));
    padding: 20px;
    border-bottom: 1px solid rgba(188, 19, 254, 0.3);
    text-align: center;
    position: relative;
    overflow: hidden;
}

.confirmation-header::before {
    content: '';
    position: absolute;
    top: 0;
    left: -100%;
    width: 100%;
    height: 100%;
    background: linear-gradient(
        90deg,
        transparent,
        rgba(255, 255, 255, 0.1),
        transparent
    );
    animation: shine 3s infinite;
}

@keyframes shine {
    100% {
        left: 100%;
    }
}

.confirmation-header h1 {
    color: #0ff0fc;
    font-weight: 700;
    font-size: 28px;
    letter-spacing: 2px;
    margin: 0;
    text-shadow: var(--glow-blue);
    animation: flicker 8s infinite alternate;
    text-transform: uppercase;
    position: relative;
}

.confirmation-header h1::after {
    content: '';
    position: absolute;
    bottom: -5px;
    left: 50%;
    transform: translateX(-50%);
    width: 50%;
    height: 2px;
    background: linear-gradient(90deg, transparent, var(--neon-blue), transparent);
    box-shadow: var(--glow-blue);
}

.confirmation-content {
    padding: 30px;
}

.card {
    background: rgba(20, 20, 40, 0.7);
    border-radius: 10px;
    padding: 20px;
    margin-bottom: 25px;
    border-left: 4px solid var(--neon-purple);
    transition: all 0.4s ease;
    position: relative;
    overflow: hidden;
}

.card::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: linear-gradient(
        45deg,
        transparent 48%,
        rgba(188, 19, 254, 0.1) 50%,
        transparent 52%
    );
    background-size: 5px 5px;
    pointer-events: none;
}

.card:hover {
    transform: translateY(-5px) scale(1.01);
    box-shadow: 0 0 30px rgba(188, 19, 254, 0.5);
    border-left: 4px solid var(--neon-green);
}

.card-title {
    color: #0ff0fc;
    font-weight: 600;
    font-size: 18px;
    letter-spacing: 1.5px;
    margin-bottom: 15px;
    text-transform: uppercase;
    display: flex;
    align-items: center;
}

.card-title::before {
    content: '◈';
    margin-right: 10px;
    color: var(--neon-pink);
    font-size: 12px;
}

.divider {
    height: 1px;
    background: linear-gradient(90deg, transparent, rgba(188, 19, 254, 0.5), transparent);
    margin: 15px 0;
    border: none;
}

.detail-row {
    display: flex;
    margin-bottom: 12px;
    align-items: center;
    transition: all 0.3s ease;
}

.detail-row:hover {
    transform: translateX(5px);
}

.detail-label {
    width: 140px;
    color: rgba(255, 255, 255, 0.7);
    font-size: 14px;
    font-family: 'Rajdhani', sans-serif;
    font-weight: 500;
    position: relative;
}

.detail-label::after {
    content: ':';
    position: absolute;
    right: 10px;
    color: var(--neon-blue);
}

.detail-value {
    flex: 1;
    font-weight: 600;
    color: #fff;
    font-family: 'Rajdhani', sans-serif;
    font-size: 15px;
    letter-spacing: 0.5px;
}

.detail-value a {
    color: var(--neon-green);
    text-decoration: none;
    position: relative;
    transition: all 0.3s ease;
}

.detail-value a::after {
    content: '';
    position: absolute;
    bottom: -2px;
    left: 0;
    width: 0;
    height: 1px;
    background: var(--neon-green);
    transition: width 0.3s ease;
}

.detail-value a:hover {
    text-shadow: 0 0 10px rgba(0, 255, 136, 0.7);
}

.detail-value a:hover::after {
    width: 100%;
}

.webex-card {
    border-left-color: var(--neon-blue);
}

.webex-card .card-title {
    color: var(--neon-blue);
}

.webinar-card {
    border-left-color: var(--neon-pink);
}

.webinar-card .card-title {
    color: var(--neon-pink);
}

.qr-code-container {
    text-align: center;
    margin-top: 40px;
    position: relative;
}

.qr-code-container::before {
    content: 'VERIFICATION';
    position: absolute;
    top: -20px;
    left: 50%;
    transform: translateX(-50%);
    color: rgba(255, 255, 255, 0.3);
    font-size: 12px;
    letter-spacing: 3px;
    text-transform: uppercase;
}

.qr-code {
    width: 160px;
    height: 160px;
    margin: 0 auto;
    background: rgba(255, 255, 255, 0.05);
    border: 1px solid rgba(188, 19, 254, 0.3);
    border-radius: 10px;
    padding: 15px;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    transition: all 0.5s ease;
    animation: float 4s ease-in-out infinite;
}

.qr-code:hover {
    transform: scale(1.05);
    box-shadow: 0 0 30px rgba(0, 255, 252, 0.3);
    border-color: var(--neon-blue);
}

.qr-code::before, .qr-code::after {
    content: '';
    position: absolute;
    width: 20px;
    height: 20px;
    border: 2px solid var(--neon-blue);
    transition: all 0.3s ease;
}

.qr-code::before {
    top: 10px;
    left: 10px;
    border-right: none;
    border-bottom: none;
}

.qr-code::after {
    bottom: 10px;
    right: 10px;
    border-left: none;
    border-top: none;
}

.qr-code:hover::before, .qr-code:hover::after {
    width: 30px;
    height: 30px;
}

.qr-code img {
    max-width: 100%;
    max-height: 100%;
    filter: drop-shadow(0 0 5px rgba(0, 255, 252, 0.5));
}

.qr-instruction {
    color: rgba(255, 255, 255, 0.7);
    font-size: 13px;
    margin-top: 15px;
    letter-spacing: 1px;
    font-family: 'Rajdhani', sans-serif;
    position: relative;
}

.qr-instruction::before {
    content: '»';
    margin-right: 5px;
    color: var(--neon-pink);
}

.confirmation-footer {
    background: linear-gradient(90deg, rgba(10, 10, 26, 0.8), rgba(92, 12, 92, 0.6));
    padding: 15px 20px;
    border-top: 1px solid rgba(188, 19, 254, 0.3);
    font-size: 13px;
    color: rgba(255, 255, 255, 0.7);
    display: flex;
    justify-content: space-between;
    align-items: center;
    position: relative;
}

.confirmation-footer::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 1px;
    background: linear-gradient(90deg, transparent, var(--neon-blue), transparent);
}

.ai-academy-brand {
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 1px;
    background: linear-gradient(90deg, var(--neon-blue), var(--neon-pink));
    -webkit-background-clip: text;
    background-clip: text;
    color: transparent;
    font-size: 1rem;
    position: relative;
    padding-left: 25px;
}

.ai-academy-brand::before {
    content: '✦';
    position: absolute;
    left: 5px;
    top: 50%;
    transform: translateY(-50%);
    color: var(--neon-green);
    font-size: 12px;
    animation: pulse 2s infinite;
}

.whatsapp-contact {
    display: flex;
    align-items: center;
    transition: all 0.3s ease;
}

.whatsapp-contact:hover {
    color: var(--neon-green);
    text-shadow: 0 0 10px rgba(37, 211, 102, 0.5);
}

.whatsapp-contact i {
    margin-right: 8px;
    color: #25D366;
    font-size: 16px;
}

/* Cyberpunk UI elements */
.cyber-button {
    background: transparent;
    border: 1px solid var(--neon-blue);
    color: var(--neon-blue);
    padding: 8px 20px;
    font-family: 'Orbitron', sans-serif;
    text-transform: uppercase;
    letter-spacing: 2px;
    font-size: 12px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
    transition: all 0.3s ease;
    margin-top: 20px;
    display: inline-block;
}

.cyber-button:hover {
    background: rgba(0, 255, 252, 0.1);
    box-shadow: 0 0 15px var(--neon-blue);
    text-shadow: 0 0 5px var(--neon-blue);
}

.cyber-button::before {
    content: '';
    position: absolute;
    top: 0;
    left: -100%;
    width: 100%;
    height: 100%;
    background: linear-gradient(
        90deg,
        transparent,
        rgba(0, 255, 252, 0.4),
        transparent
    );
    transition: all 0.5s ease;
}

.cyber-button:hover::before {
    left: 100%;
}

/* Responsive adjustments */
@media (max-width: 768px) {
    .confirmation-container {
        margin: 20px;
        transform: none;
    }
    
    .detail-row {
        flex-direction: column;
        align-items: flex-start;
    }
    
    .detail-label {
        width: 100%;
        margin-bottom: 5px;
    }
    
    .confirmation-footer {
        flex-direction: column;
        gap: 10px;
        text-align: center;
    }
}");
        html.AppendLine("    </style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        html.AppendLine("    <div class=\"confirmation-container\">");
        html.AppendLine("        <div class=\"confirmation-header\">");
        html.AppendLine("            <h1>WEBINAR REGISTRATION CONFIRMATION</h1>");
        html.AppendLine("        </div>");
        html.AppendLine("        <div class=\"confirmation-content\">");

        // Registration Details Card
        html.AppendLine("            <div class=\"card\">");
        html.AppendLine("                <div class=\"card-title\">REGISTRATION DETAILS</div>");
        html.AppendLine("                <div class=\"divider\"></div>");
        html.AppendLine("                <div class=\"detail-row\">");
        html.AppendLine("                    <div class=\"detail-label\">ID</div>");
        html.AppendLine($"                   <div class=\"detail-value\">{random.Next(int.MaxValue)}</div>");
        html.AppendLine("                </div>");
        html.AppendLine("                <div class=\"detail-row\">");
        html.AppendLine("                    <div class=\"detail-label\">Name</div>");
        html.AppendLine($"                   <div class=\"detail-value\">{registration.StudentName}</div>");
        html.AppendLine("                </div>");
        html.AppendLine("                <div class=\"detail-row\">");
        html.AppendLine("                    <div class=\"detail-label\">Email</div>");
        html.AppendLine($"                   <div class=\"detail-value\">{registration.Email}</div>");
        html.AppendLine("                </div>");
        html.AppendLine("            </div>");

        // Webinar Information Card
        html.AppendLine("            <div class=\"card webinar-card\">");
        html.AppendLine("                <div class=\"card-title\">WEBINAR INFORMATION</div>");
        html.AppendLine("                <div class=\"divider\"></div>");
        html.AppendLine("                <div class=\"detail-row\">");
        html.AppendLine("                    <div class=\"detail-label\">Topic</div>");
        html.AppendLine($"                   <div class=\"detail-value\">{webinar.Topic}</div>");
        html.AppendLine("                </div>");
        html.AppendLine("                <div class=\"detail-row\">");
        html.AppendLine("                    <div class=\"detail-label\">Date</div>");
        html.AppendLine($"                   <div class=\"detail-value\">{webinar.Date.ToShortDateString()}</div>");
        html.AppendLine("                </div>");
        html.AppendLine("                <div class=\"detail-row\">");
        html.AppendLine("                    <div class=\"detail-label\">Time</div>");
        html.AppendLine($"                   <div class=\"detail-value\">{webinar.Time}</div>");
        html.AppendLine("                </div>");
        html.AppendLine("                <div class=\"detail-row\">");
        html.AppendLine("                    <div class=\"detail-label\">Venue</div>");
        html.AppendLine($"                   <div class=\"detail-value\">{webinar.Venue}</div>");
        html.AppendLine("                </div>");
        html.AppendLine("                <div class=\"detail-row\">");
        html.AppendLine("                    <div class=\"detail-label\">Speaker</div>");
        html.AppendLine($"                   <div class=\"detail-value\">{webinar.Speaker}</div>");
        html.AppendLine("                </div>");
        html.AppendLine("            </div>");

        // Webex Meeting Details Card
        html.AppendLine("            <div class=\"card webex-card\">");
        html.AppendLine("                <div class=\"card-title\">WEBEX MEETING DETAILS</div>");
        html.AppendLine("                <div class=\"divider\"></div>");
        html.AppendLine("                <div class=\"detail-row\">");
        html.AppendLine("                    <div class=\"detail-label\">Meeting URL</div>");
        html.AppendLine($"                   <div class=\"detail-value\"><a href=\"{webinar.WebexUrl}\">Join Webex Meeting</a></div>");
        html.AppendLine("                </div>");
        html.AppendLine("                <div class=\"detail-row\">");
        html.AppendLine("                    <div class=\"detail-label\">Meeting ID</div>");
        html.AppendLine($"                   <div class=\"detail-value\">{webinar.WebexMeetingId}</div>");
        html.AppendLine("                </div>");

        if (!string.IsNullOrEmpty(webinar.WebexPasscode))
        {
            html.AppendLine("                <div class=\"detail-row\">");
            html.AppendLine("                    <div class=\"detail-label\">Passcode</div>");
            html.AppendLine($"                   <div class=\"detail-value\">{webinar.WebexPasscode}</div>");
            html.AppendLine("                </div>");
        }

        html.AppendLine("            </div>");

        // QR Code Section
        html.AppendLine("            <div class=\"qr-code-container\">");
        html.AppendLine("                <div class=\"holographic-badge\">");
        html.AppendLine("                    <div class=\"holo-circle\"></div>");
        html.AppendLine("                    <div class=\"holo-circle holo-circle-2\"></div>");
        html.AppendLine("                    <div class=\"holo-circle holo-circle-3\"></div>");
        html.AppendLine("                    <div class=\"qr-code\">");
        html.AppendLine($"                      <img src=\"data:image/png;base64,{qrCodeImageBase64}\" alt=\"QR Code\" class=\"qr-image\">");
        html.AppendLine("                    </div>");
        html.AppendLine("                    <div class=\"verification-beam\"></div>");
        html.AppendLine("                    <div class=\"verification-status\">");
        html.AppendLine("                        <span class=\"verification-text\">GENERATED</span>");
        html.AppendLine("                        <div class=\"verification-dots\">");
        html.AppendLine("                            <span class=\"dot\"></span>");
        html.AppendLine("                            <span class=\"dot\"></span>");
        html.AppendLine("                            <span class=\"dot\"></span>");
        html.AppendLine("                        </div>");
        html.AppendLine("                    </div>");
        html.AppendLine("                </div>");
        html.AppendLine("                <div class=\"qr-instruction\">SCAN FOR MEETING LINKS AND DETAILS</div>");
        html.AppendLine("                <div class=\"scan-animation\">");
        html.AppendLine("                    <div class=\"scan-line\"></div>");
        html.AppendLine("                </div>");
        html.AppendLine("            </div>");


        // Footer
        html.AppendLine("        <div class=\"confirmation-footer\">");
        html.AppendLine("            <div class=\"whatsapp-contact\">");
        html.AppendLine("                <i class=\"fab fa-whatsapp\"></i> +91 9311878122");
        html.AppendLine("            </div>");
        html.AppendLine($"           <div>© {DateTime.Now.Year} WEBINAR REGISTRATION SYSTEM</div>");
        html.AppendLine("            <div class=\"ai-academy-brand\">");
        html.AppendLine("                AI ACADEMY | DR. M");
        html.AppendLine("            </div>");
        html.AppendLine("        </div>");
        html.AppendLine("    </div>");

        // JavaScript for dynamic effects
        html.AppendLine(@"    <script>
        // Create particle effects
        document.addEventListener('DOMContentLoaded', function() {
            const container = document.querySelector('.confirmation-container');
            
            // Create particles on mouse move
            document.addEventListener('mousemove', function(e) {
                for (let i = 0; i < 3; i++) {
                    const particle = document.createElement('div');
                    particle.className = 'particle';
                    
                    // Randomize position slightly around cursor
                    const x = e.clientX + (Math.random() - 0.5) * 20;
                    const y = e.clientY + (Math.random() - 0.5) * 20;
                    
                    // Randomize movement direction and distance
                    const tx = (Math.random() - 0.5) * 200;
                    const ty = (Math.random() - 0.5) * 200;
                    
                    particle.style.left = x + 'px';
                    particle.style.top = y + 'px';
                    particle.style.setProperty('--tx', tx + 'px');
                    particle.style.setProperty('--ty', ty + 'px');
                    
                    // Randomize color
                    const colors = ['#0ff0fc', '#ff00ff', '#bc13fe', '#00ff88'];
                    particle.style.background = colors[Math.floor(Math.random() * colors.length)];
                    
                    document.body.appendChild(particle);
                    
                    // Remove particle after animation
                    setTimeout(() => {
                        particle.remove();
                    }, 2000);
                }
            });
            
            // Add hover effect to cards
            const cards = document.querySelectorAll('.card');
            cards.forEach(card => {
                card.addEventListener('mouseenter', function() {
                    this.style.borderLeftColor = 'var(--neon-green)';
                });
                
                card.addEventListener('mouseleave', function() {
                    if (this.classList.contains('webex-card')) {
                        this.style.borderLeftColor = 'var(--neon-blue)';
                    } else if (this.classList.contains('webinar-card')) {
                        this.style.borderLeftColor = 'var(--neon-pink)';
                    } else {
                        this.style.borderLeftColor = 'var(--neon-purple)';
                    }
                });
            });
        });
// Holographic badge interaction
        const qrCode = document.querySelector('.qr-code');
        const verificationBeam = document.querySelector('.verification-beam');
        const verificationText = document.querySelector('.verification-text');
        
        qrCode.addEventListener('click', function() {
            // Simulate scan interaction
            this.classList.add('scanning');
            verificationBeam.style.animation = 'none';
            verificationBeam.offsetHeight; // Trigger reflow
            verificationBeam.style.animation = 'beam 0.5s ease-in-out';
            
            // Change verification status
            verificationText.textContent = 'SCANNING';
            verificationText.style.color = 'var(--neon-blue)';
            
            setTimeout(() => {
                verificationText.textContent = 'VERIFIED';
                verificationText.style.color = 'var(--neon-green)';
                this.classList.remove('scanning');
                
                // Create a pulse effect
                const pulse = document.createElement('div');
                pulse.className = 'pulse-effect';
                pulse.style.position = 'absolute';
                pulse.style.width = '100%';
                pulse.style.height = '100%';
                pulse.style.borderRadius = '50%';
                pulse.style.border = '2px solid var(--neon-green)';
                pulse.style.top = '0';
                pulse.style.left = '0';
                pulse.style.animation = 'pulse 1s ease-out';
                pulse.style.zIndex = '1';
                qrCode.appendChild(pulse);
                
                // Remove after animation
                setTimeout(() => {
                    pulse.remove();
                }, 1000);
            }, 1500);
        });
        
        // Add hover effect to badge
        const badge = document.querySelector('.holographic-badge');
        badge.addEventListener('mouseenter', function() {
            this.style.animation = 'float 3s ease-in-out infinite';
            document.querySelector('.holo-circle').style.animationDuration = '10s';
            document.querySelector('.holo-circle-2').style.animationDuration = '8s';
            document.querySelector('.holo-circle-3').style.animationDuration = '12s';
        });
        
        badge.addEventListener('mouseleave', function() {
            this.style.animation = 'float 6s ease-in-out infinite';
            document.querySelector('.holo-circle').style.animationDuration = '20s';
            document.querySelector('.holo-circle-2').style.animationDuration = '20s';
            document.querySelector('.holo-circle-3').style.animationDuration = '20s';
        });
    </script>");

        html.AppendLine(@"    <script>
        // Sound effects from SoundJay.com
        const soundToggle = document.getElementById('soundToggle');
        let soundsEnabled = false;
        const sounds = {
            hover: new Audio('https://www.soundjay.com/buttons/sounds/beep-07.mp3'),
            click: new Audio('https://www.soundjay.com/buttons/sounds/button-09.mp3'),
            scan: new Audio('https://www.soundjay.com/mechanical/sounds/radar-01.mp3'),
            success: new Audio('https://www.soundjay.com/buttons/sounds/button-21.mp3'),
            ambient: new Audio('https://www.soundjay.com/communication/sounds/star-trek-communicator-01.mp3'),
            error: new Audio('https://www.soundjay.com/buttons/sounds/beep-05.mp3'),
            whoosh: new Audio('https://www.soundjay.com/mechanical/sounds/sci-fi-whoosh-01.mp3')
        };

        // Set volume levels
        Object.values(sounds).forEach(sound => {
            sound.volume = 0.3;
        });
        sounds.ambient.volume = 0.1;
        sounds.ambient.loop = true;

        // Toggle sound effects
        soundToggle.addEventListener('click', function() {
            soundsEnabled = !soundsEnabled;
            this.classList.toggle('active');
            
            if (soundsEnabled) {
                sounds.ambient.play().catch(e => console.log('Audio play failed:', e));
                sounds.click.play().catch(e => console.log('Audio play failed:', e));
            } else {
                sounds.ambient.pause();
            }
        });

        // Create particle effects with sound
        document.addEventListener('DOMContentLoaded', function() {
            const container = document.querySelector('.confirmation-container');
            
            // Play ambient sound on load if enabled
            if (soundsEnabled) {
                sounds.ambient.play().catch(e => console.log('Audio play failed:', e));
            }
            
            // Create particles on mouse move with sound
            document.addEventListener('mousemove', function(e) {
                if (soundsEnabled) {
                    sounds.hover.currentTime = 0;
                    sounds.hover.play().catch(e => console.log('Audio play failed:', e));
                }
                
                for (let i = 0; i < 3; i++) {
                    const particle = document.createElement('div');
                    particle.className = 'particle';
                    
                    // Randomize position slightly around cursor
                    const x = e.clientX + (Math.random() - 0.5) * 20;
                    const y = e.clientY + (Math.random() - 0.5) * 20;
                    
                    // Randomize movement direction and distance
                    const tx = (Math.random() - 0.5) * 200;
                    const ty = (Math.random() - 0.5) * 200;
                    
                    particle.style.left = x + 'px';
                    particle.style.top = y + 'px';
                    particle.style.setProperty('--tx', tx + 'px');
                    particle.style.setProperty('--ty', ty + 'px');
                    
                    // Randomize color
                    const colors = ['#0ff0fc', '#ff00ff', '#bc13fe', '#00ff88'];
                    particle.style.background = colors[Math.floor(Math.random() * colors.length)];
                    
                    document.body.appendChild(particle);
                    
                    // Remove particle after animation
                    setTimeout(() => {
                        particle.remove();
                    }, 2000);
                }
            });
            
            // Add hover effect to cards with sound
            const cards = document.querySelectorAll('.card');
            cards.forEach(card => {
                card.addEventListener('mouseenter', function() {
                    if (soundsEnabled) {
                        sounds.whoosh.currentTime = 0;
                        sounds.whoosh.play().catch(e => console.log('Audio play failed:', e));
                    }
                    this.style.borderLeftColor = 'var(--neon-green)';
                });
                
                card.addEventListener('mouseleave', function() {
                    if (this.classList.contains('webex-card')) {
                        this.style.borderLeftColor = 'var(--neon-blue)';
                    } else if (this.classList.contains('webinar-card')) {
                        this.style.borderLeftColor = 'var(--neon-pink)';
                    } else {
                        this.style.borderLeftColor = 'var(--neon-purple)';
                    }
                });
            });

            // Add click sound to all interactive elements
            const interactiveElements = document.querySelectorAll('a, .qr-code, .cyber-button, .sound-visualizer');
            interactiveElements.forEach(el => {
                el.addEventListener('click', function() {
                    if (soundsEnabled) {
                        sounds.click.currentTime = 0;
                        sounds.click.play().catch(e => console.log('Audio play failed:', e));
                    }
                });
            });
        });

        // Enhanced holographic badge interaction with sounds
        const qrCode = document.querySelector('.qr-code');
        const verificationBeam = document.querySelector('.verification-beam');
        const verificationText = document.querySelector('.verification-text');
        
        qrCode.addEventListener('click', function() {
            if (soundsEnabled) {
                sounds.scan.currentTime = 0;
                sounds.scan.play().catch(e => console.log('Audio play failed:', e));
            }
            
            // Simulate scan interaction
            this.classList.add('scanning');
            verificationBeam.style.animation = 'none';
            verificationBeam.offsetHeight; // Trigger reflow
            verificationBeam.style.animation = 'beam 0.5s ease-in-out';
            
            // Change verification status
            verificationText.textContent = 'SCANNING';
            verificationText.style.color = 'var(--neon-blue)';
            
            setTimeout(() => {
                if (soundsEnabled) {
                    sounds.success.play().catch(e => console.log('Audio play failed:', e));
                }
                verificationText.textContent = 'VERIFIED';
                verificationText.style.color = 'var(--neon-green)';
                this.classList.remove('scanning');
                
                // Create a pulse effect
                const pulse = document.createElement('div');
                pulse.className = 'pulse-effect';
                pulse.style.position = 'absolute';
                pulse.style.width = '100%';
                pulse.style.height = '100%';
                pulse.style.borderRadius = '50%';
                pulse.style.border = '2px solid var(--neon-green)';
                pulse.style.top = '0';
                pulse.style.left = '0';
                pulse.style.animation = 'pulse 1s ease-out';
                pulse.style.zIndex = '1';
                qrCode.appendChild(pulse);
                
                // Remove after animation
                setTimeout(() => {
                    pulse.remove();
                }, 1000);
            }, 1500);
        });
        
        // Add hover effect to badge with sound
        const badge = document.querySelector('.holographic-badge');
        badge.addEventListener('mouseenter', function() {
            if (soundsEnabled) {
                sounds.hover.currentTime = 0;
                sounds.hover.play().catch(e => console.log('Audio play failed:', e));
            }
            this.style.animation = 'float 3s ease-in-out infinite';
            document.querySelector('.holo-circle').style.animationDuration = '10s';
            document.querySelector('.holo-circle-2').style.animationDuration = '8s';
            document.querySelector('.holo-circle-3').style.animationDuration = '12s';
        });
        
        badge.addEventListener('mouseleave', function() {
            this.style.animation = 'float 6s ease-in-out infinite';
            document.querySelector('.holo-circle').style.animationDuration = '20s';
            document.querySelector('.holo-circle-2').style.animationDuration = '20s';
            document.querySelector('.holo-circle-3').style.animationDuration = '20s';
        });

        // Sound wave visualization
        function createSoundWave() {
            const soundWave = document.createElement('div');
            soundWave.className = 'sound-wave';
            
            for (let i = 0; i < 8; i++) {
                const bar = document.createElement('div');
                bar.className = 'sound-bar';
                soundWave.appendChild(bar);
            }
            
            return soundWave;
        }

        // Add sound wave to sound toggle when enabled
        soundToggle.addEventListener('click', function() {
            if (soundsEnabled) {
                const existingWave = this.querySelector('.sound-wave');
                if (!existingWave) {
                    this.appendChild(createSoundWave());
                }
            } else {
                const wave = this.querySelector('.sound-wave');
                if (wave) {
                    wave.remove();
                }
            }
        });
    </script>");


        html.AppendLine("    <!-- Font Awesome for icons -->");
        html.AppendLine("    <script src=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/js/all.min.js\"></script>");
        html.AppendLine("    <div class=\"sound-visualizer\" id=\"soundToggle\">");
        html.AppendLine("        <i class=\"fas fa-volume-up\"></i>");
        html.AppendLine("    </div>");


        html.AppendLine("</body>");


        html.AppendLine("</html>");

        return html.ToString();
    }
}

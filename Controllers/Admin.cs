// Controllers/WebinarAdminController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AIAcademy.Controllers;
[Route("api/admin/[controller]")]
[ApiController]
public class WebinarAdminController : ControllerBase
{
    private readonly WebinarDbContext _context;
    private readonly ILogger<WebinarAdminController> _logger;

    public WebinarAdminController(WebinarDbContext context, ILogger<WebinarAdminController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWebinar([FromBody] Webinar webinar, [FromHeader] string username, [FromHeader] string password)
    {
        // Authenticate admin
        var admin = await _context.AdminUsers.FirstOrDefaultAsync(a => a.Username == username && a.Password == password);
        if (admin == null)
        {
            return Unauthorized("Invalid admin credentials");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Webinars.Add(webinar);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"New webinar created: {webinar.Topic} by admin {username}");
        return CreatedAtAction(nameof(GetWebinar), new { id = webinar.Id }, webinar);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Webinar>> GetWebinar(int id)
    {
        var webinar = await _context.Webinars.FindAsync(id);
        if (webinar == null)
        {
            return NotFound();
        }
        return webinar;
    }
}
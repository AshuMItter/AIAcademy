// Controllers/WebinarAdminController.cs
using AIAcademy.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AIAcademy.Controllers;
[Route("api/admin/[controller]")]
[ApiController]
public class WebinarAdminController : ControllerBase
{
    private readonly WebinarDbContext _context;
    private readonly ILogger<WebinarAdminController> _logger;
    private IWebHostEnvironment _env;
    public WebinarAdminController(WebinarDbContext context, ILogger<WebinarAdminController> logger, IWebHostEnvironment env)
    {
        _context = context;
        _logger = logger;
        _env = env;
    }

    //[HttpGet]
    //[Route("get-data")]
    //public string DataGet()
    //{
    //    string result = WebinarCSVContext.ReadDataFromCSV(Path.Combine(_env.ContentRootPath, "Dataset", "webinar.csv"));
    //    return result;
    //}

    [HttpPost]
    public async Task<IActionResult> CreateWebinar([FromBody] Webinar webinar, [FromHeader] string username, [FromHeader] string password)
    {

       return Ok("data");
        // Authenticate admin
        // var admin = await _context.AdminUsers.FirstOrDefaultAsync(a => a.Username == username && a.Password == password);
        //if (username == "admin" && password=="admin123") {
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Webinars.Add(webinar);
        //    await _context.SaveChangesAsync();

        //    _logger.LogInformation($"New webinar created: {webinar.Topic} by admin {username}");
        //    return CreatedAtAction(nameof(GetWebinar), new { id = webinar.Id }, webinar);
        //}
        //else {

        //    return Unauthorized("Invalid admin credentials");
        //}
        //if (admin == null)
        //{
        //    return Unauthorized("Invalid admin credentials");
        //}

        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        //_context.Webinars.Add(webinar);
        //await _context.SaveChangesAsync();

        //_logger.LogInformation($"New webinar created: {webinar.Topic} by admin {username}");
        //return CreatedAtAction(nameof(GetWebinar), new { id = webinar.Id }, webinar);
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<List<Webinar>>> GetWebinar(int id)
    //{
       
       

      
    //    return webinars;
    //}
}
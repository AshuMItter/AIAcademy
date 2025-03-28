using Microsoft.AspNetCore.Mvc;
using AIAcademy.Model;
namespace AIAcademy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        IWebHostEnvironment _env;
       
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("sample")]
        public IActionResult GetData()
        {
            string path = Path.Combine(_env.ContentRootPath, "Dataset", "grades.csv");
            List<double> _marksOBtained = null;
           List<double> _totalHours= StudentStudyHoursTotalMarksDataSet.Expose_StudyHoursList(path,out _marksOBtained);

            return Ok("Made by heart from Ashu");
        }
    }
}

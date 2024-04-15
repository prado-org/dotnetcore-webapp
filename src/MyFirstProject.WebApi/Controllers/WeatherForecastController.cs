using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MyFirstProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private WeatherForecast WeatherForecastById(int id)
        {
            try
            {
                WeatherForecast item = null;
                using SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
                connection.OpenAsync();

                string selectCommand = "SELECT * FROM WeatherForecast WHERE id = @id";

                SqlCommand command = new SqlCommand(selectCommand, connection);
                command.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DateTime data = reader.GetDateTime(0);
                    string summary = reader.GetString(1);
                    int temperature = reader.GetInt32(2);

                    item = new WeatherForecast { Date = data, Summary = summary, TemperatureC = temperature };
                }

                return item;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the weather forecast.");
                throw;
            }
        }
    }
}

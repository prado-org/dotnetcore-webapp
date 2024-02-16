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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
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
                using SqlConnection connection = new SqlConnection("Server=localhost;Database=Todo;User Id=sa;Password=Password123;");
                connection.OpenAsync();
                
                string selectCommand = "SELECT * FROM WeatherForecast WHERE id = " + id.ToString();

                SqlCommand command = new SqlCommand(selectCommand, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime data = reader.GetDateTime(0);
                        string summary = reader.GetString(1);
                        int temperature = reader.GetInt32(2);

                        item = new WeatherForecast { Date = data, Summary = summary, TemperatureC = temperature };
                    }
                }

                return item;
            }
            catch(Exception)
            {
                throw;
            }
        }

        private WeatherForecast WeatherForecastByTeste(int id)
        {
            try
            {
                WeatherForecast item = null;
                using SqlConnection connection = new SqlConnection("Server=localhost;Database=Todo;User Id=sa;Password=Password123;");
                connection.OpenAsync();
                
                string selectCommand = "SELECT * FROM WeatherForecast WHERE id = " + id.ToString();

                SqlCommand command = new SqlCommand(selectCommand, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime data = reader.GetDateTime(0);
                        string summary = reader.GetString(1);
                        int temperature = reader.GetInt32(2);

                        item = new WeatherForecast { Date = data, Summary = summary, TemperatureC = temperature };
                    }
                }

                return item;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}

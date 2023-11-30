using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MyFirstProject.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            //outro comentário do treianmento
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, WeatherForecast item)
        {
            try
            {
                _logger.LogInformation("Method - PutTodoItem");
                _logger.LogInformation("Param - Id = " + id);
                _logger.LogInformation("Param - Item = " + item);
                
                if (id != item.TemperatureC)
                {
                    return BadRequest();
                }
            
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError("ERROR: " + ex.ToString());
                throw;
            }
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
    }
}

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

        
        private async Task<WeatherForecast> WeatherForecastById(int id)
        {
            try
            {
                WeatherForecast item = null;
                string connectionString = _configuration.GetConnectionString("MyDbConnection"); // Assuming the connection string is defined in your configuration
        
                using SqlConnection connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
        
                string selectCommand = "SELECT * FROM WeatherForecast WHERE id = @Id";
        
                SqlCommand command = new SqlCommand(selectCommand, connection);
                command.Parameters.Add(new SqlParameter("@Id", id));
        
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
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
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error in WeatherForecastById with id: {id}");
                throw;
            }
        }
    }
}

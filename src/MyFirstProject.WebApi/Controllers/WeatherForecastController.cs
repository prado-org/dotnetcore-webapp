using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Xml;
using MyFirstProject.WebApi.Models;

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

        [HttpGet]
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

        private void ProcessRequest(string employeeName)
        {
            using (XmlWriter writer = XmlWriter.Create("employees.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteRaw("<employee><name>" + employeeName + "</name></employee>");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

                private async Task<WeatherForecast> WeatherForecastByIdAsync(int id)
        {
            try
            {
                WeatherForecast item = null;
                var connectionString = "Server=localhost;Database=Todo;User Id=sa;Password=Password123;";
        
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
        
                    string selectCommand = "SELECT Date, Summary, TemperatureC FROM WeatherForecast WHERE id = @id";
        
                    using (SqlCommand command = new SqlCommand(selectCommand, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
        
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                DateTime data = reader.GetDateTime(0);
                                string summary = reader.GetString(1);
                                int temperature = reader.GetInt32(2);
        
                                item = new WeatherForecast { Date = data, Summary = summary, TemperatureC = temperature };
                            }
                        }
                    }
                }
        
                return item;
            }
            catch (SqlException ex)
            {
                // Log and handle SQL exceptions
                throw new Exception("An error occurred while accessing the database.", ex);
            }
            catch (Exception ex)
            {
                // Log and handle other exceptions
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
    }
}

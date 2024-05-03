using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Xml;

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

        public void ProcessRequest(string employeeName)
        {
            using (XmlWriter writer = XmlWriter.Create("employees.xml"))
            {
                writer.WriteStartDocument();

                // BAD: Insert user input directly into XML
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
                using SqlConnection connection = new SqlConnection("Server=localhost;Database=Todo;User Id=sa;Password=Password123;");
                await connection.OpenAsync();

                string selectCommand = "SELECT * FROM WeatherForecast WHERE id = @id";

                SqlCommand command = new SqlCommand(selectCommand, connection);
                command.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    DateTime data = reader.GetDateTime(0);
                    string summary = reader.GetString(1);
                    int temperature = reader.GetInt32(2);

                    item = new WeatherForecast { Date = data, Summary = summary, TemperatureC = temperature };
                }

                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

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

        private WeatherForecast WeatherForecastById(int id)
        {
            try
            {
                WeatherForecast item = null;
                string connectionString = _configuration.GetConnectionString("TodoDatabase");
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.OpenAsync();
                
                string selectCommand = "SELECT * FROM WeatherForecast WHERE id = " + id.ToString();

                SqlCommand command = new SqlCommand(selectCommand, connection);

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
            catch(Exception)
            {
                throw;
            }
        }
    }
}

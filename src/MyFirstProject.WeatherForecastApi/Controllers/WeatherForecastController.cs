using Microsoft.AspNetCore.Mvc;
using MyFirstProject.WeatherForecastApi.Models;
using Microsoft.Data.SqlClient;
using System.Xml;

namespace MyFirstProject.WeatherForecastApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        // nova alteração Gh4Women
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
            return Enumerable.Range(1, 20).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private void CreateXml(string itemName)
        {
            _logger.LogInformation("Method CreateXml");
            using (XmlWriter writer = XmlWriter.Create("todos.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteRaw("<todo><name>" + itemName + "</name></todo>");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private WeatherForecast GetWeatherForecastById(int id)
        {
            _logger.LogInformation("Method GetTodoItemById");
            try
            {
                WeatherForecast item = null;
                using SqlConnection connection = new SqlConnection("Server=localhost;Database=Todo;User Id=sa;Password=Password123;");
                connection.OpenAsync();

                string selectCommand = "SELECT * FROM WeatherForecast WHERE id = " + id.ToString();

                SqlCommand command = new SqlCommand(selectCommand, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    item = new WeatherForecast
                    {
                        Date = reader.GetDateTime(1),
                        TemperatureC = reader.GetInt32(2),
                        Summary = reader.GetString(3)
                    };
                }

                return item;
            }
            catch (Exception ex)    
            {
                _logger.LogError("Error in GetTodoItemById: " + ex.Message);
                throw ex;
            }
        }
    }
}

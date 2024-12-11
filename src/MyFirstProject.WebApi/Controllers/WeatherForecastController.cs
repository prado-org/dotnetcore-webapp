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

        /// <summary>
        /// Retrieves a WeatherForecast by its ID from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the WeatherForecast to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the WeatherForecast object if found; otherwise, null.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the WeatherForecast.</exception>
        private async Task<WeatherForecast> WeatherForecastByIdAsync(int id)
        {
            try
            {
                // Declara uma variável para armazenar o item WeatherForecast encontrado
                WeatherForecast item = null;
        
                // Obtém a string de conexão do arquivo de configuração
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
        
                // Cria uma nova conexão SQL usando a string de conexão
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abre a conexão de forma assíncrona
                    await connection.OpenAsync();
        
                    // Define o comando SQL para selecionar o WeatherForecast pelo ID
                    string selectCommand = "SELECT Date, Summary, TemperatureC FROM WeatherForecast WHERE id = @id";
        
                    // Cria um comando SQL com a consulta e a conexão
                    using (SqlCommand command = new SqlCommand(selectCommand, connection))
                    {
                        // Adiciona o parâmetro ID ao comando para evitar SQL Injection
                        command.Parameters.AddWithValue("@id", id);
        
                        // Executa o comando e obtém um leitor de dados de forma assíncrona
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            // Verifica se há linhas no resultado da consulta
                            if (await reader.ReadAsync())
                            {
                                // Lê a data da primeira coluna do resultado
                                DateTime data = reader.GetDateTime(0);
        
                                // Lê o resumo da segunda coluna do resultado
                                string summary = reader.GetString(1);
        
                                // Lê a temperatura da terceira coluna do resultado
                                int temperature = reader.GetInt32(2);
        
                                // Cria um novo objeto WeatherForecast com os dados lidos
                                item = new WeatherForecast { Date = data, Summary = summary, TemperatureC = temperature };
                            }
                        }
                    }
                }
        
                // Retorna o item WeatherForecast encontrado ou null se não encontrado
                return item;
            }
            catch (Exception ex)
            {
                // Captura e lança novamente qualquer exceção que ocorra durante a execução
                // Aqui você pode adicionar código para logar a exceção (ex)
                throw;
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyFirstProject.TodoApi.Models;
using System.Xml;

namespace MyFirstProject.TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemController : ControllerBase
    {
        private readonly ILogger<TodoItemController> _logger;
        private readonly TodoItemContext _context;

        public TodoItemController(ILogger<TodoItemController> logger, TodoItemContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            _logger.LogInformation("Method GetTodoItems");
            return await _context.TodoItems.ToListAsync();
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

        private TodoItem GetTodoItemById(int id)
        {
            _logger.LogInformation("Method GetTodoItemById");
            try
            {
                TodoItem item = null;
                using SqlConnection connection = new SqlConnection("Server=localhost;Database=Todo;User Id=sa;Password=Password123;");
                connection.OpenAsync();

                string selectCommand = "SELECT * FROM TodoItem WHERE id = " + id.ToString();

                SqlCommand command = new SqlCommand(selectCommand, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string Name = reader.GetString(1);
                    bool IsComplete = reader.GetBoolean(2);
                    DateTime DueDate = reader.GetDateTime(3);

                    item = new TodoItem { Id = Id, Name = Name, IsComplete = IsComplete, DueDate = DueDate };
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

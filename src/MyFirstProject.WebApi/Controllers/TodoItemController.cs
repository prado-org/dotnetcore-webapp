using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyFirstProject.WebApi.Models;

namespace MyFirstProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemController : ControllerBase
    {
        private readonly ILogger<TodoItemController> _logger;
        private readonly TodoItemContext _context;

        public TodoItemController(ILogger<TodoItemController> logger, TodoItemContext context)
        {
            //testes
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            _logger.LogInformation("Method - GetTodoItems");
            return await _context.TodoItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            _logger.LogInformation($"Method - GetTodoItem: {id}");

            TodoItem item = null;
            using SqlConnection connection = new SqlConnection("Server=localhost;Database=Todo;User Id=sa;Password=Password123;");
            connection.OpenAsync();
            
            string selectCommand = "SELECT * FROM TodoItem WHERE id = " + id.ToString();

            SqlCommand command = new SqlCommand(selectCommand, connection);

            SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                long _id = reader.GetInt16(0);
                string _name = reader.GetString(1);
                bool _complete = reader.GetBoolean(2);

                item = new TodoItem { Id = _id, Name = _name, IsComplete = _complete };
            }

            return item;
        }


    }
}

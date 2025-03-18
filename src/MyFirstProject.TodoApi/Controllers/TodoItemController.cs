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

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            _logger.LogInformation("Method GetTodoItem");
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _logger.LogInformation("Method PostTodoItem");
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            _logger.LogInformation("Method PutTodoItem");
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            _logger.LogInformation("Method DeleteTodoItem");
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            _logger.LogInformation("Method TodoItemExists");
            return _context.TodoItems.Any(e => e.Id == id);
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

        private TodoItem GetTodoItemById(long id)
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

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
            //teste
            _logger.LogInformation("Method - GetTodoItems");
            return await _context.TodoItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            _logger.LogInformation($"Method - GetTodoItem, ID: {id}");
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                _logger.LogWarning($"TodoItem with ID: {id} not found.");
                return NotFound();
            }

            return todoItem;
        }
    }
}

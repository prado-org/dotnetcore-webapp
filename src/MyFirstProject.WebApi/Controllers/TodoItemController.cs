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
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            try
            {
                _logger.LogInformation("Method - GetTodoItems");
                return await _context.TodoItems.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError("ERROR: " + ex.ToString());
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            try
            {
                _logger.LogInformation("Method - PostTodoItem");
                _context.TodoItems.Add(item);
                await _context.SaveChangesAsync();
        
                return CreatedAtAction(nameof(GetTodoItems), new { id = item.Id }, item);
            }
            catch(Exception ex)
            {
                _logger.LogError("ERROR: " + ex.ToString());
                throw;
            }
        }
    }
}

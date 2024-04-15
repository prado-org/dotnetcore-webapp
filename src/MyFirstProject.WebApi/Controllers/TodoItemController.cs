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

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _logger.LogInformation("Method - PostTodoItem");
        
            // Verifica se o nome contém um caractere especial
            if (!Regex.IsMatch(todoItem.Name, @"^[a-zA-Z0-9\s]*$"))
            {
                return BadRequest("O nome não pode conter caracteres especiais.");
            }
        
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
        
            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }


    }
}

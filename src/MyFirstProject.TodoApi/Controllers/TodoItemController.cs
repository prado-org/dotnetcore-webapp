using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstProject.TodoApi.Models;

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

        /// <summary>
        /// Cria um novo TodoItem.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(TodoItem todoItem)
        {
            if (string.IsNullOrWhiteSpace(todoItem.Name) || todoItem.Name.Length > 255 || !IsValidName(todoItem.Name))
            {
                return BadRequest("Nome inválido. O campo Nome é obrigatório, não pode conter caracteres especiais e não pode ter mais de 255 caracteres.");
            }

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItems), new { id = todoItem.Id }, todoItem);
        }

        private bool IsValidName(string name)
        {
            foreach (char c in name)
            {
                if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

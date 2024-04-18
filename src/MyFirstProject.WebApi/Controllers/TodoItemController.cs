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

        /// <summary>
        /// Gets a todo item with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the todo item to get.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            _logger.LogInformation("Method - GetTodoItem");
        
            var todoItem = await _context.TodoItems.FindAsync(id);
        
            if (todoItem == null)
            {
                return NotFound();
            }
        
            return todoItem;
        }

        /// <summary>
        /// Deletes a todo item with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the todo item to delete.</param>
        /// <returns>An <see cref="ActionResult{T}"/> representing the deleted todo item, or <see cref="NotFoundResult"/> if the todo item is not found.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        /// <summary>
        /// Saves a new todo item.
        /// </summary>
        /// <param name="todoItem">The todo item to save.</param>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
        
            // Retorna um status HTTP 201 (Created) e a rota para o novo item de tarefa.
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }


    }
}

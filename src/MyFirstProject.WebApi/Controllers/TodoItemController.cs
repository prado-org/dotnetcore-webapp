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
        public async Task<ActionResult<TodoItem>> SaveTodoItem(TodoItem todoItem)
        {
            // Verifica se o título contém um caractere especial
            if (System.Text.RegularExpressions.Regex.IsMatch(todoItem.Name, @"[^a-zA-Z0-9\s]"))
            {
                return BadRequest("O título não pode conter caracteres especiais.");
            }
        
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
        
            return CreatedAtAction(nameof(GetTodoItems), new { id = todoItem.Id }, todoItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
        
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
        
            return NoContent();
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItem todoItem)
        {
            // Check if the ID provided in the URL matches the ID of the item provided in the request body.
            // If they don't match, return a 400 Bad Request response.
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            // Check if the Name of the item contains any special characters.
            // If it does, return a 400 Bad Request response with a specific error message.
            if (System.Text.RegularExpressions.Regex.IsMatch(todoItem.Name, @"[^a-zA-Z0-9\s]"))
            {
                return BadRequest("O título não pode conter caracteres especiais.");
            }

            // Tell Entity Framework that the item has been modified.
            // This means that when SaveChangesAsync is called, EF will generate a SQL UPDATE statement for this item.
            _context.Entry(todoItem).State = EntityState.Modified;

            // Save the changes to the database asynchronously.
            // This will apply the SQL UPDATE statement generated above.
            await _context.SaveChangesAsync();

            // Return a 204 No Content response.
            // This is a common way to indicate that the operation was successful and there's no additional content to send in the response.
            return NoContent();
        }



    }
}

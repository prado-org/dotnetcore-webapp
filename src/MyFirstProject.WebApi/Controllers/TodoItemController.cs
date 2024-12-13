using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyFirstProject.WebApi.Models;
using System.Text.RegularExpressions;

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
            _logger.LogInformation("Method GetTodoItems");
            return await _context.TodoItems.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(TodoItem todoItem)
        {
            if (string.IsNullOrEmpty(todoItem.Name) || !Regex.IsMatch(todoItem.Name, @"^[a-zA-Z0-9\s]+$") || todoItem.Name.Length > 255)
            {
                return BadRequest("The Name field is required, cannot contain special characters, and must be less than 255 characters.");
            }

            todoItem.IsComplete = false;

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItems), new { id = todoItem.Id }, todoItem);
        }
    }
}

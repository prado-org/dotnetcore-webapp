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

        /// <summary>
        /// Retrieves a list of todo items.
        /// </summary>
        /// <returns>A list of todo items.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            // Log information about the method being called
            _logger.LogInformation("Method - GetTodoItems");

            // Retrieve the list of todo items from the database
            return await _context.TodoItems.ToListAsync();
        }
    }
}

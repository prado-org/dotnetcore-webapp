using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Creates an XML file named "todos.xml" with the provided todo item details.
        /// </summary>
        /// <param name="itemName">The name of the todo item.</param>
        /// <param name="isComplete">A boolean indicating whether the todo item is complete.</param>
        private void CreateXml(string itemName, bool isComplete)
        {
            _logger.LogInformation("Method CreateXml");
            using (XmlWriter writer = XmlWriter.Create("todos.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteRaw("<todo><name>" + itemName + "</name><isComplete>" + isComplete.ToString() + "</isComplete></todo>");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}

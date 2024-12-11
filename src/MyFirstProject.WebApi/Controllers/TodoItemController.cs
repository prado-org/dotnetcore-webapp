using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFirstProject.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Obtém uma lista de itens de tarefas.
        /// </summary>
        /// <returns>Uma lista de itens de tarefas.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoItem>), 200)]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            _logger.LogInformation("Method GetTodoItems");
            return await _context.TodoItems.ToListAsync();
        }

        /// <summary>
        /// Obtém um item de tarefa específico pelo ID.
        /// </summary>
        /// <param name="id">ID do item de tarefa.</param>
        /// <returns>O item de tarefa correspondente ao ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoItem), 200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Cria um novo item de tarefa.
        /// </summary>
        /// <param name="todoItem">O item de tarefa a ser criado.</param>
        /// <returns>O item de tarefa criado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _logger.LogInformation("Method PostTodoItem");
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        /// <summary>
        /// Atualiza um item de tarefa existente.
        /// </summary>
        /// <param name="id">ID do item de tarefa a ser atualizado.</param>
        /// <param name="todoItem">O item de tarefa atualizado.</param>
        /// <returns>Resultado da operação de atualização.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Exclui um item de tarefa pelo ID.
        /// </summary>
        /// <param name="id">ID do item de tarefa a ser excluído.</param>
        /// <returns>Resultado da operação de exclusão.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(TodoItem), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            _logger.LogInformation("Method DeleteTodoItem");
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
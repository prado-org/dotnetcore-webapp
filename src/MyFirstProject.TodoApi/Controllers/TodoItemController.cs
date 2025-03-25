using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstProject.TodoApi.Models;
using System.Xml;
using System.Text.RegularExpressions;

namespace MyFirstProject.TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemController : ControllerBase
    {
        private readonly ILogger<TodoItemController> _logger;
        private readonly TodoItemContext _context;

        /// <summary>
        /// Construtor do controlador TodoItemController.
        /// </summary>
        /// <param name="logger">Instância do logger para registrar logs.</param>
        /// <param name="context">Contexto do Entity Framework para acessar o banco de dados.</param>
        public TodoItemController(ILogger<TodoItemController> logger, TodoItemContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Obtém todos os itens de tarefas.
        /// </summary>
        /// <returns>Uma lista de itens de tarefas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            _logger.LogInformation("Método GetTodoItems");
            return await _context.TodoItems.ToListAsync();
        }

        /// <summary>
        /// Obtém um item de tarefa pelo ID.
        /// </summary>
        /// <param name="id">ID do item de tarefa.</param>
        /// <returns>O item de tarefa correspondente ao ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItemById(long id)
        {
            _logger.LogInformation("Método GetTodoItemById");

            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        /// <summary>
        /// Atualiza um item de tarefa existente.
        /// </summary>
        /// <param name="id">ID do item de tarefa a ser atualizado.</param>
        /// <param name="todoItem">Objeto TodoItem com os dados atualizados.</param>
        /// <returns>Resultado da operação de atualização.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            // Implementa uma regra para verificar se o campo Name possui caracteres especiais, usando regex. Se possuir, retorna um BadRequest com uma mensagem de erro.
            if (Regex.IsMatch(todoItem.Name, @"[!@#$%^&*(),.?\""]"))
            {
                return BadRequest("O campo Name não pode conter caracteres especiais.");
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
        /// Verifica se um item de tarefa existe no banco de dados.
        /// </summary>
        /// <param name="id">ID do item de tarefa.</param>
        /// <returns>True se o item de tarefa existir, caso contrário, False.</returns>
        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        /// <summary>
        /// Cria um novo item de tarefa.
        /// </summary>
        /// <param name="todoItem">Objeto TodoItem a ser criado.</param>
        /// <returns>O item de tarefa criado.</returns>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _logger.LogInformation("Método PostTodoItem");

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItemById), new { id = todoItem.Id }, todoItem);
        }

        /// <summary>
        /// Cria um arquivo XML chamado "todos.xml" com os detalhes do item de tarefa fornecido.
        /// </summary>
        /// <param name="itemName">O nome do item de tarefa.</param>
        /// <param name="isComplete">Um booleano indicando se o item de tarefa está completo.</param>
        private void CreateXml(string itemName, bool isComplete)
        {
            _logger.LogInformation("Método CreateXml");
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
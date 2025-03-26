using Microsoft.AspNetCore.Mvc;
using MyFirstProject.WebApp.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace MyFirstProject.WebApp.Controllers
{
    public class TodoItemController : Controller
    {
        private readonly ILogger<TodoItemController> _logger;
        private IConfiguration _configuration;

        public TodoItemController(ILogger<TodoItemController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Controller:TodoItemController - Method:Index");

                List<TodoItemViewModel> lst = new List<TodoItemViewModel>();
                string _urlApi = string.Empty;

                using (var httpClient = new HttpClient())
                {
                    _urlApi = _configuration.GetSection("Api:Todo").Value + "/api/TodoItem";
                    _logger.LogInformation("URL API = " + _urlApi);

                    using (var response = await httpClient.GetAsync( _urlApi))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        lst = JsonConvert.DeserializeObject<List<TodoItemViewModel>>(apiResponse);
                    }
                }

                _logger.LogInformation("Qtde: " + lst.Count().ToString());

                return View(lst);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: " + ex.Message);
                return Redirect("/Home/Error");
            }
        }

        public async Task<IActionResult> Details(long id)
        {
            try
            {
                _logger.LogInformation("Controller:TodoItemController - Method:Details");

                TodoItemViewModel item = null;
                string _urlApi = _configuration.GetSection("Api:Url").Value + $"/api/TodoItem/{id}";

                using (var httpClient = new HttpClient())
                {
                    _logger.LogInformation("URL API = " + _urlApi);

                    using (var response = await httpClient.GetAsync(_urlApi))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        item = JsonConvert.DeserializeObject<TodoItemViewModel>(apiResponse);
                    }
                }

                if (item == null)
                {
                    return NotFound();
                }

                return View(item);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: " + ex.Message);
                return Redirect("/Home/Error");
            }
        }

        /// <summary>
        /// Exibe o formulário para criar um novo TodoItem.
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Processa a submissão do formulário para criar um novo TodoItem.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string _urlApi = _configuration.GetSection("Api:Todo").Value + "/api/TodoItem";
                    _logger.LogInformation("URL API = " + _urlApi);

                    using (var httpClient = new HttpClient())
                    {
                        var json = JsonConvert.SerializeObject(todoItem);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PostAsync(_urlApi, content))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Erro ao criar o TodoItem.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("ERROR: " + ex.Message);
                    return Redirect("/Home/Error");
                }
            }

            return View(todoItem);
        }
    }
}

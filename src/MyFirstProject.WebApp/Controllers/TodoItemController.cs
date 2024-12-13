using Microsoft.AspNetCore.Mvc;
using MyFirstProject.WebApp.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

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
                    _urlApi = _configuration.GetSection("Api:Url").Value + "/api/TodoItem";
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(todoItem.Name) || !Regex.IsMatch(todoItem.Name, @"^[a-zA-Z0-9\s]+$") || todoItem.Name.Length > 255)
                {
                    ModelState.AddModelError("Name", "The Name field is required, cannot contain special characters, and must be less than 255 characters.");
                    return View(todoItem);
                }

                todoItem.IsComplete = false;

                try
                {
                    string _urlApi = _configuration.GetSection("Api:Url").Value + "/api/TodoItem";
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(_urlApi);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var json = JsonConvert.SerializeObject(todoItem);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync(_urlApi, content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "TodoItem has been successfully created.";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "An error occurred while creating the TodoItem.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("ERROR: " + ex.Message);
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the TodoItem.");
                }
            }

            return View(todoItem);
        }
    }
}

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
                string _urlApi = _configuration.GetSection("Api:Todo").Value + $"/api/TodoItem/{id}";

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItemViewModel item)
        {
            try
            {
                _logger.LogInformation("Controller:TodoItemController - Method:Create");

                if (ModelState.IsValid)
                {
                    string _urlApi = _configuration.GetSection("Api:Todo").Value + "/api/TodoItem";
                    using (var httpClient = new HttpClient())
                    {
                        _logger.LogInformation("URL API = " + _urlApi);
                        var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(_urlApi, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            item = JsonConvert.DeserializeObject<TodoItemViewModel>(apiResponse);
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(item);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: " + ex.Message);
                return Redirect("/Home/Error");
            }
        }

        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                _logger.LogInformation("Controller:TodoItemController - Method:Edit");
                TodoItemViewModel item = null;
                string _urlApi = _configuration.GetSection("Api:Todo").Value + $"/api/TodoItem/{id}";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, TodoItemViewModel item)
        {
            try
            {
                _logger.LogInformation("Controller:TodoItemController - Method:Edit");
                if (id != item.Id)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    string _urlApi = _configuration.GetSection("Api:Todo").Value + $"/api/TodoItem/{id}";
                    using (var httpClient = new HttpClient())
                    {
                        _logger.LogInformation("URL API = " + _urlApi);
                        var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PutAsync(_urlApi, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            item = JsonConvert.DeserializeObject<TodoItemViewModel>(apiResponse);
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(item);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: " + ex.Message);
                return Redirect("/Home/Error");
            }
        }

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                _logger.LogInformation("Controller:TodoItemController - Method:Delete");
                TodoItemViewModel item = null;
                string _urlApi = _configuration.GetSection("Api:Todo").Value + $"/api/TodoItem/{id}";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id, TodoItemViewModel item)
        {
            try
            {
                _logger.LogInformation("Controller:TodoItemController - Method:DeleteConfirmed");
                string _urlApi = _configuration.GetSection("Api:Todo").Value + $"/api/TodoItem/{id}";
                using (var httpClient = new HttpClient())
                {
                    _logger.LogInformation("URL API = " + _urlApi);
                    using (var response = await httpClient.DeleteAsync(_urlApi))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: " + ex.Message);
                return Redirect("/Home/Error");
            }
        }

    }
}
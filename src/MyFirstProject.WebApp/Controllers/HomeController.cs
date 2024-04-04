using Microsoft.AspNetCore.Mvc;
using MyFirstProject.WebApp.Models;
using System.Diagnostics;

namespace MyFirstProject.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;

        private readonly IHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHostEnvironment env)
        {
            //tese
            _logger = logger;
            _configuration = configuration;
            _env = env;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index Page");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Privacy Page");

            ViewBag.VersionInfoNumber = _configuration.GetSection("VersionInfo:Number").Value;
            ViewBag.VersionInfoDate = _configuration.GetSection("VersionInfo:Date").Value;
            ViewBag.Api = _configuration.GetSection("Api:Url").Value;
            ViewBag.EnvironmentName = _env.EnvironmentName;

            var connAdo = "payoihpodpcqhn6xhaxzau3w77fgvrgtk26qgrigitpbl2rnsr4q";
            _logger.LogInformation("Connection ADO = " + connAdo);
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogInformation("Error Page");
            
            var myInfo = "109.326.110-28";
            _logger.LogInformation("My Info = " + myInfo);

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

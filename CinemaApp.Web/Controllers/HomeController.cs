
using System.Diagnostics;

using CinemaApp.Web.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //Два варянат за споделяне на данни от контролера към Вюто:
            //1.Използвайки ViewData / ViewBag
            //2. Pass ViewModel to the View

            ViewData["Title"] = "Home Page";
            ViewData["Message"] = "Welcome to the Cinema Web App!";
            
            return View();
        }

        
    }
}

using Microsoft.AspNetCore.Mvc;
using SalesPredictionProject.Models;
using System.Diagnostics;

namespace SalesPredictionProject.Controllers
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Handles form submission and redirects to ForecastSales action
        [HttpPost]
        public IActionResult SubmitForecastPeriod(int forecastPeriod)
        {
            // Redirect to SalesController's ForecastSales action, passing the forecastPeriod
            return RedirectToAction("ForecastSales", "Sales", new { forecastPeriod });
        }
    }
}

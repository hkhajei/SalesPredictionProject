using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Ensure this is included
using SalesPredictionProject.Data; // Adjust to your actual namespace
using SalesPredictionProject.Models.Service; // Adjust to your actual namespace
using SalesPredictionProject.Models; // Include your models namespace
using System.Threading.Tasks;
using System.Linq;
using SalesPredictionProject.Models.Entity;

namespace SalesPredictionProject.Controllers
{
    public class SalesController : Controller
    {
        private readonly ILogger<SalesController> _logger;
        private readonly MyDbContext _db;
        private readonly ForecastService _forecastService;

        public SalesController(MyDbContext db, ILogger<SalesController> logger, ForecastService forecastService)
        {
            _db = db;
            _logger = logger;
            _forecastService = forecastService;
        }

        // Default action for the Sales page
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ForecastResults(List<ForecastResult> forecastResults)
        {
            return View(forecastResults); // Pass the forecast results to the view
        }

        // Action to upload sales data from a file
        [HttpPost]
        public IActionResult UploadSalesData(IFormFile salesDataFile)
        {
            if (salesDataFile != null && salesDataFile.Length > 0)
            {
                // Logic to parse and save sales data
                // You might want to add error handling here
                // Example: Read the file and save data to the database
            }

            return RedirectToAction("Index");
        }

        // Action to forecast sales based on a selected period
        [HttpGet] // Keep this if calling via GET
        public async Task<IActionResult> ForecastSales(int forecastPeriod)
        {
            var salesData = _db.SalesDataRows.ToList(); // Fetch from DB
            if (salesData == null || !salesData.Any())
            {
                _logger.LogWarning("No sales data found.");
                return View("ForecastResults", new List<ForecastResult>()); // Pass an empty list
            }

            var  forecastResult =await  _forecastService.GetForecast(salesData, forecastPeriod);


            if (forecastResult == null)
            {
                _logger.LogWarning("Forecasting returned no results.");
                return View("ForecastResults", new List<ForecastResult>()); // Pass an empty list
            }

            return View("ForecastResults", forecastResult); // Show results in view
        }
    }
}

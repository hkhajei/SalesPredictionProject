using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Ensure this is included
using SalesPredictionProject.Data; // Adjust to your actual namespace
using SalesPredictionProject.Models.Service; // Adjust to your actual namespace
using SalesPredictionProject.Models; // Include your models namespace
using System.Threading.Tasks;
using System.Linq;
using SalesPredictionProject.Models.Entity;
using Newtonsoft.Json;
using System.Text;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using System.Globalization;

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


        // Action to forecast sales based on a selected period
        [HttpPost]
        public async Task<IActionResult> UploadSalesData(IFormFile salesDataFile)
        {
            if (salesDataFile != null && salesDataFile.Length > 0)
            {
                using (var reader = new StreamReader(salesDataFile.OpenReadStream(), Encoding.UTF8))
                {
                    var extension = Path.GetExtension(salesDataFile.FileName).ToLower();
                    var fileContent = await reader.ReadToEndAsync();

                    List<SalesDataRow> salesData = null;

                    if (extension == ".json")
                    {
                        // Parse as JSON
                        salesData = JsonConvert.DeserializeObject<List<SalesDataRow>>(fileContent);
                    }
                    else if (extension == ".csv")
                    {
                        // Parse as CSV
                        using (var csvReader = new CsvReader(new StringReader(fileContent), CultureInfo.InvariantCulture))
                        {
                            salesData = csvReader.GetRecords<SalesDataRow>().ToList();
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Unsupported file type uploaded.");
                        return RedirectToAction("Index");
                    }

                    if (salesData != null)
                    {
                        HttpContext.Session.SetString("UploadedSalesData", JsonConvert.SerializeObject(salesData));
                    }
                }
            }

            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public async Task<IActionResult> ForecastSales(int forecastPeriod)
        {
            List<SalesDataRow> salesData = null;

            var uploadedJson = HttpContext.Session.GetString("UploadedSalesData");
            if (!string.IsNullOrEmpty(uploadedJson))
            {
                salesData = JsonConvert.DeserializeObject<List<SalesDataRow>>(uploadedJson);
            }
            
            if (salesData == null || !salesData.Any())
            {
                _logger.LogWarning("No sales data found.");
                return View("ForecastResults", new List<ForecastResult>());
            }

            var forecastResult = await _forecastService.GetForecast(salesData, forecastPeriod);

            if (forecastResult == null)
            {
                _logger.LogWarning("Forecasting returned no results.");
                return View("ForecastResults", new List<ForecastResult>());
            }

            return View("ForecastResults", forecastResult);
        }
    }
}

using Newtonsoft.Json;
using SalesPredictionProject.Models.Entity;
using System.Net.Http;
using System.Text;

namespace SalesPredictionProject.Models.Service
{
    public class ForecastService
    {
        private readonly HttpClient _client;

        public ForecastService(HttpClient client)
        {
            _client = client;
        }


        public async Task<List<ForecastResult>> GetForecast(List<SalesData> salesData, int forecastPeriod)
        {
            // Assuming you serialize salesData to send to the Python API
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                sales_data = salesData,
                forecast_period = forecastPeriod
            }), Encoding.UTF8, "application/json");

            // Send request to the Python Flask API
            var response = await _client.PostAsync($"http://127.0.0.1:5000/forecast", content);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize JSON into List<ForecastResult>
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var forecastResults = JsonConvert.DeserializeObject<List<ForecastResult>>(jsonResponse);
                return forecastResults;
            }

            return new List<ForecastResult>(); // Return an empty list if something goes wrong
        }
    }

}

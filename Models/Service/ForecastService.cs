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


        public async Task<List<ForecastResult>> GetForecast(List<SalesDataRow> salesData, int forecastPeriod)
        {
            var payload = new
            {
                forecast_period = forecastPeriod,
                sales_data = salesData       // <- raw list of objects
            };

            var json = JsonConvert.SerializeObject(payload); // Serialize properly

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("http://binno.ir/flaskapp/forecast", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var forecastResults = JsonConvert.DeserializeObject<List<ForecastResult>>(jsonResponse);
                return forecastResults;
            }

            return new List<ForecastResult>();
        }
    }

}

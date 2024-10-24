using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SalesPredictionProject.Models.Entity
{
    public class ForecastResult
    {
        //[JsonConverter(typeof(CustomDateTimeConverter))]
        [JsonProperty("ds")] // Map 'ds' in JSON to 'Date' in C#

        public DateTime Date { get; set; }    // Corresponds to 'ds' from the Python API
        [JsonProperty("yhat")] // Map 'yhat' in JSON to 'PredictedSales' in C#

        public decimal PredictedSales { get; set; }  // Corresponds to 'yhat' (predicted values)
    }

}

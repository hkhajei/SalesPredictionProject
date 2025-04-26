using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SalesPredictionProject.Models.Entity
{
    public class ForecastResult
    {
        public DateTime ds { get; set; }
        public double yhat { get; set; }
    }

}

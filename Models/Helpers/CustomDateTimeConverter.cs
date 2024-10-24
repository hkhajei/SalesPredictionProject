using Newtonsoft.Json;
using System.Globalization;

namespace SalesPredictionProject.Models.Helpers
{
    public class CustomDateTimeConverter : JsonConverter
    {
        private readonly string _format = "ddd, dd MMM yyyy HH:mm:ss 'GMT'"; // Match the date format in the JSON response

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dateStr = reader.Value.ToString();
            if (DateTime.TryParseExact(dateStr, _format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var date))
            {
                return date;
            }
            return DateTime.MinValue; // Handle any parsing failures
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime date = (DateTime)value;
            writer.WriteValue(date.ToString(_format, CultureInfo.InvariantCulture));
        }
    }
}

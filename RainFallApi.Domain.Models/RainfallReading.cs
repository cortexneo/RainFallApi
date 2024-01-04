using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace RainFallApi.Domain.Models
{
    [SwaggerSchema("Details of a rainfall reading", Title = "Rainfall reading")]

    public class RainfallReading
    {
        [JsonProperty("dateTime")]
        public DateTime DateMeasured { get; set; }

        [JsonProperty("value")]
        public decimal AmountMeasured { get; set; }
    }
}

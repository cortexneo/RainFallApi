using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;

namespace RainFallApi.Domain.Models
{
    [SwaggerSchema("Details of a rainfall reading", Title = "Rainfall reading response")]
    public class RainfallReadingResponse
    {
        [JsonProperty("items")]
        public List<RainfallReading> Readings { get; set; }
    }
}

using Swashbuckle.AspNetCore.Annotations;

namespace RainFallApi.Domain.Models
{
    [SwaggerSchema("Details of a rainfall reading", Title = "Error response")]

    public class Error
    {
        public string Message { get; set; }

        public List<ErrorDetail> Detail { get; set; }
    }
}

using Swashbuckle.AspNetCore.Annotations;

namespace RainFallApi.Domain.Models
{
    [SwaggerSchema("Details of invalid request property")]
    public class ErrorDetail
    {
        public string? PropertyName { get; set; }

        public string? Message { get; set; }
    }
}

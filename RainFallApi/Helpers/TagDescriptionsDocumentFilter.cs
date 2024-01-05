using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RainFallApi.Presentation.Helpers
{
    /// <summary>
    /// Tag Descriptions Document Filter
    /// </summary>
    public class TagDescriptionsDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Apply Tag Descriptions
        /// </summary>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<OpenApiTag>
            {
                new() { Name = "Rainfall", Description = "Operations relating to rainfall" }
            };
        }
    }
}

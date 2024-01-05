using Microsoft.AspNetCore.Mvc;
using RainFallApi.Domain;
using RainFallApi.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RainFallApi.Presentation.Controllers
{
    /// <summary>
    /// Controller class for Rain Fall Reader Endpoints
    /// </summary>
    [ApiController]
    [Route("rainfall")]
    public class RainfallController : ControllerBase
    {
        private readonly IRainFallReaderService _rainFallService;

        /// <summary>
        /// Controller class for Rain Fall Reader Endpoints
        /// </summary>
        public RainfallController(IRainFallReaderService rainFallService)
        {
            _rainFallService = rainFallService;
        }

        /// <summary>
        /// Get rainfall readings by station Id
        /// </summary>
        /// <param name="stationId">The id of the reading station</param>
        /// <param name="count">The number of readings to return</param>
        /// <returns>A list of rainfall readings</returns>
        [HttpGet("id/{stationId}/readings")]
        [SwaggerOperation(
            Summary = "Get rainfall readings by station Id",
            Description = "Retrieve the latest readings for the specified stationId",
            OperationId = "get-rainfall"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "A list of rainfall readings successfully retrieved", typeof(RainfallReadingResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request", typeof(Error))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No readings found for the specified stationId", typeof(Error))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(Error))]
        public async Task<IActionResult> GetRainfallReadings(string stationId, [Range(1, 100)] int count = 10)
        {
            try
            {
                var result = await _rainFallService.GetRainfallReadings(stationId, count);

                return result.Readings != null 
                    && result.Readings.Count > 0 
                    ? Ok(result) 
                    : NotFound();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                var errorResponse = new Error
                {
                    Message = "Server Error",
                    Detail = new List<ErrorDetail>
                    {
                        new() { PropertyName = "General", Message = ex.Message }
                    }
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }
}

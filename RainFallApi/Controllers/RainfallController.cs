using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RainFallApi.Domain;
using RainFallApi.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RainFallApi.Presentation.Controllers
{
    [ApiController]
    [Route("rainfall")]
    public class RainfallController : ControllerBase
    {
        private readonly IRainFallReaderService _rainFallService;

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
                if (string.IsNullOrEmpty(stationId))
                {
                    var errorResponse = new Error
                    {
                        Message = "Bad Request",
                        Detail = new List<ErrorDetail>
                        {
                            new() { PropertyName = "stationId", Message = "StationId is required" }
                        }
                    };
                    return BadRequest(errorResponse);
                }

                var response = await _rainFallService.GetRainfallReadings(stationId, count);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var rainfallReadingResponse = JsonConvert.DeserializeObject<RainfallReadingResponse>(responseData);
                    return Ok(rainfallReadingResponse);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
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

                return StatusCode(500, errorResponse);
            }
        }
    }
}

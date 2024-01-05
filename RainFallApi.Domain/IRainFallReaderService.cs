using RainFallApi.Domain.Models;

namespace RainFallApi.Domain
{
    public interface IRainFallReaderService
    {
        Task<RainfallReadingResponse> GetRainfallReadings(string stationId, int count = 10);
    }
}

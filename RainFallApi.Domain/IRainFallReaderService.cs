using RainFallApi.Domain.Models;

namespace RainFallApi.Domain
{
    public interface IRainFallReaderService
    {
        Task<HttpResponseMessage> GetRainfallReadings(string stationId, int count = 10);
    }
}

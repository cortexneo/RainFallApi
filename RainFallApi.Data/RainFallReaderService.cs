using RainFallApi.Domain;

namespace RainFallApi.Data
{
    public class RainFallReaderService : IRainFallReaderService
    {
        private readonly HttpClient _httpClient;

        public RainFallReaderService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("RainFallApiBaseUrl");
        }

        public async Task<HttpResponseMessage> GetRainfallReadings(string stationId, int count = 10)
        {

            //http://environment.data.gov.uk/flood-monitoring/id/stations/4163/readings?parameter=rainfall
            string url = $"id/stations/{stationId}/readings?parameter=rainfall&_limit={count}";
            return await _httpClient.GetAsync(url);
        }
    }
}


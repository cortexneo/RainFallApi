using Newtonsoft.Json;
using RainFallApi.Domain;
using RainFallApi.Domain.Models;
using System.Net;

namespace RainFallApi.Data
{
    public class RainFallReaderService : IRainFallReaderService
    {
        private readonly HttpClient _httpClient;

        public RainFallReaderService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("RainFallApiBaseUrl");
        }

        public async Task<RainfallReadingResponse> GetRainfallReadings(string stationId, int count = 10)
        {
            if (stationId == null)
                throw new ArgumentNullException(nameof(stationId));

            string url = $"id/stations/{stationId}/readings?_sorted&parameter=rainfall&_limit={count}";

            try
            {
                RainfallReadingResponse rainfallReadingResponse = new();
                var response = await _httpClient.GetAsync(url);

                if(response.StatusCode == HttpStatusCode.OK)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    rainfallReadingResponse = JsonConvert.DeserializeObject<RainfallReadingResponse>(responseData) ?? throw new Exception();
                }
                return rainfallReadingResponse;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}


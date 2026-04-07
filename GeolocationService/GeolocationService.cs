using Newtonsoft.Json;
using SorTechTask.ahmedmohamedelameen.Models;

namespace SorTechTask.ahmedmohamedelameen.GeolocationService
{
    public class GeolocationService : IGeolocationService
    {
         readonly HttpClient _httpClient;
        readonly string _apiKey;

        public GeolocationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GeolocationSettings:ApiKey"];
        }

        public async Task<IPLookupResponse> GetCountryInfoAsync(string ipAddress)
        {
      
            var url = $"ipgeo?apiKey={_apiKey}&ip={ipAddress}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IPLookupResponse>(content);
        }
    }
}

using SorTechTask.ahmedmohamedelameen.Models;

namespace SorTechTask.ahmedmohamedelameen.GeolocationService
{
    public interface IGeolocationService
    {
        Task<IPLookupResponse> GetCountryInfoAsync(string ipAddress);
    }
}
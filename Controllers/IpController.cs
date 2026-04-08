using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SorTechTask.ahmedmohamedelameen.GeolocationService;
using SorTechTask.ahmedmohamedelameen.Models;
using SorTechTask.ahmedmohamedelameen.Repositories;

namespace SorTechTask.ahmedmohamedelameen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        readonly IGeolocationService _geoService;
        readonly MemoryStorage _storage;

        public IpController(IGeolocationService geoService, MemoryStorage storage)
        {
            _geoService = geoService;
            _storage = storage;
        }


        [HttpGet("lookup")]
        public async Task<IActionResult> LookupIp([FromQuery] string? ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var data = await _geoService.GetCountryInfoAsync(ipAddress);

            if (data == null)
                return NotFound("Could not retrieve info for this IP.");

            return Ok(data);
        }


        [HttpGet("check-block")]
        public async Task<IActionResult> CheckIfBlocked()
        {
          
            var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (userIp == "::1" || string.IsNullOrEmpty(userIp)) userIp = "156.210.1.1";

            var geoInfo = await _geoService.GetCountryInfoAsync(userIp);
            if (geoInfo == null) return StatusCode(500, "Error verifying IP location.");

            bool isBlocked = _storage.BlockedCountries.ContainsKey(geoInfo.Country_Code);
            var log = new BlockedAttemptLog
            {
                IpAddress = userIp,
                CountryCode = geoInfo.Country_Code,
                Timestamp = DateTime.UtcNow,
                BlockedStatus = isBlocked,
                UserAgent = Request.Headers["User-Agent"].ToString()
            };
            _storage.Logs.Add(log);

            if (isBlocked)
                return StatusCode(403, new { Message = "Access Denied. Your country is blocked.", Details = geoInfo });

            return Ok(new { Message = "Access Granted.", Details = geoInfo });
        }
    }
}

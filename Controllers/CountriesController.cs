using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SorTechTask.ahmedmohamedelameen.DTO;
using SorTechTask.ahmedmohamedelameen.Models;
using SorTechTask.ahmedmohamedelameen.Repositories;

namespace SorTechTask.ahmedmohamedelameen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        readonly MemoryStorage _storage;

        public CountriesController(MemoryStorage storage) => _storage = storage;
        


        [HttpPost("block")]
        public IActionResult BlockedCountry([FromBody] BlockedCountryRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.CountryCode) || request.CountryCode.Length != 2)
                return BadRequest("Invalid country code format. It must be a 2-letter ISO code.");

            var code = request.CountryCode.ToUpper().Trim();
            if (!code.All(char.IsLetter))
                return BadRequest("Country code must contain letters only.");
            if (_storage.BlockedCountries.ContainsKey(code))
            { 
                return Conflict(new { Message = $"Country with code '{code}' is already blocked." });
            }
            var newBlock = new BlockedCountry
            {
                CountryCode = request.CountryCode, 
                BlockedAt = DateTime.UtcNow
            };

            bool added = _storage.BlockedCountries.TryAdd(code, newBlock);

            if (added)
            {
                return Ok(newBlock);
            }

            return StatusCode(500, "An error occurred while adding the country.");
        }

        [HttpDelete("block/{countryCode}")]
        public IActionResult DeleteBlockedCountry(string countryCode)
        {
            if (_storage.BlockedCountries.TryRemove(countryCode, out _))
            {
                return Ok(new { Message = $"Country {countryCode} unblocked successfully." });
            }

            return NotFound(new { Message = "Country is not in the blocked list." });
        }

        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromQuery] string search = null)
        {
            var data = _storage.BlockedCountries.Values.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(c => c.CountryCode.Contains(search, StringComparison.OrdinalIgnoreCase)
                      || c.CountryName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

          
            var result = data
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                TotalCount = data.Count(),
                Page = page,
                PageSize = pageSize,
                Data = result
            });
        }

        [HttpPost("temporal-block")]
        public IActionResult TemporalBlock([FromBody] TemporalBlockRequestDTO request)
        {
            if(string.IsNullOrWhiteSpace(request.CountryCode) || request.CountryCode.Length != 2)
                return BadRequest("Invalid country code format. It must be a 2-letter ISO code.");
            if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                return BadRequest("Duration must be between 1 and 1440 minutes.");

            var code = request.CountryCode.ToUpper();
            if (!code.All(char.IsLetter))
                return BadRequest("Country code must contain letters only.");

            if (_storage.BlockedCountries.ContainsKey(code))
                return Conflict("Country is already blocked.");

            var newBlock = new BlockedCountry
            {
                CountryCode = code,
                BlockedAt = DateTime.UtcNow,
                ExpirationTime = DateTime.UtcNow.AddMinutes(request.DurationMinutes)
            };

            _storage.BlockedCountries.TryAdd(code, newBlock);
            return Ok(newBlock);
        }

       
    }
}

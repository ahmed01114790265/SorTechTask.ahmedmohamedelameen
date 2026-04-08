using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SorTechTask.ahmedmohamedelameen.Repositories;

namespace SorTechTask.ahmedmohamedelameen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
         readonly MemoryStorage _storage;
        public LogsController(MemoryStorage storage) => _storage = storage;

        [HttpGet("blocked-attempts")]
        public IActionResult GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var logs = _storage.Logs
                .OrderByDescending(l => l.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new { TotalCount = _storage.Logs.Count, Page = page, Data = logs });
        }
    }
}

using SorTechTask.ahmedmohamedelameen.Models;
using System.Collections.Concurrent;

namespace SorTechTask.ahmedmohamedelameen.Repositories
{
    public class MemoryStorage
    {
        public ConcurrentDictionary<string, BlockedCountry> BlockedCountries { get; }
        = new ConcurrentDictionary<string, BlockedCountry>(StringComparer.OrdinalIgnoreCase);
        public ConcurrentBag<BlockedAttemptLog> Logs { get; } = new ConcurrentBag<BlockedAttemptLog>(); 
    }
}

using SorTechTask.ahmedmohamedelameen.Repositories;

namespace SorTechTask.ahmedmohamedelameen.BackgroundServices
{
    public class BlockedCountryCleanupService : BackgroundService
    {
       readonly MemoryStorage _storage;

        public BlockedCountryCleanupService(MemoryStorage storage)
        {
            _storage = storage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var expiredCountries = _storage.BlockedCountries
                    .Where(c => c.Value.ExpirationTime.HasValue && c.Value.ExpirationTime <= now)
                    .Select(c => c.Key)
                    .ToList();

                foreach (var code in expiredCountries)
                {
                    _storage.BlockedCountries.TryRemove(code, out _);
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}

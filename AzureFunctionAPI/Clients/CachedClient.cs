using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AzureFunctionAPI.Clients {
	public abstract class CachedClient<ItemType> {
		protected const int CacheSizeLimit = 1024 * 8;
		protected virtual int TimeToLiveMinutes { get; } = 60;
		protected MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions { SizeLimit = CacheSizeLimit });
		protected abstract Task<ItemType> GetFromSource(string key);
		protected abstract ILogger Logger { get; }
		public async Task<ItemType?> GetAsync(string key) {
			return await Cache.GetOrCreateAsync(key, async entry => {
				Logger.LogInformation($"Fetching {typeof(ItemType).Name} from source for {key} (caching for {TimeToLiveMinutes} minutes)");
				var stopwatch = Stopwatch.StartNew();
				var data = await GetFromSource(key);
				Logger.LogInformation($"Fetched {typeof(ItemType).Name} from source for {key} in {stopwatch.Elapsed}");
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(TimeToLiveMinutes);
				entry.Size = 1;
				return data;
			});
		}
	}
}

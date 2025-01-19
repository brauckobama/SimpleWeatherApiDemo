using SimpleWeatherApiDemo.Classes;
using SimpleWeatherApiDemo.Interfaces;
using System.Collections.Concurrent;

namespace SimpleWeatherApiDemo.Services
{
    public class WeatherCacheService : IWeatherCacheService
    {
        private ConcurrentDictionary<object, CacheItem> _cacheDictionary = new();
        public void Add(object key, object value, DateTime? timeToExpire)
        {
            PurgeExpired();
            var cacheItem = new CacheItem() { Value = value, TimeToExpire = timeToExpire };
            _cacheDictionary.TryAdd(key, cacheItem);
        }

        public object? Get(object key)
        {
            PurgeExpired();
            _cacheDictionary.TryGetValue(key, out var cachedValue);
            return cachedValue?.Value;
        }

        private void PurgeExpired()
        {
            var expiredItems = _cacheDictionary.Where(x => x.Value.IsExpired);
            foreach (var item in expiredItems) 
            {
                _cacheDictionary.TryRemove(item);
            }
        }
    }
}

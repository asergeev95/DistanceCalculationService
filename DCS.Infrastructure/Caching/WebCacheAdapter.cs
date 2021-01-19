using System;
using Microsoft.Extensions.Caching.Memory;

namespace DCS.Infrastructure.Caching
{
    public class WebCacheAdapter : ICacheService
    {
        private readonly IMemoryCache _cache;

        public WebCacheAdapter(IMemoryCache cache)
        {
            // Cache will be disposed on application shutdown
            // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.memorycache.dispose?view=netframework-4.8
            _cache = cache;
        }
        
        public void Add(string key, object value, DateTime absoluteExpiration)
        {
            _cache.Set(key, value, new DateTimeOffset(absoluteExpiration));
        }

        public bool TryGetValue<T>(string key, out T value) where T : class
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}
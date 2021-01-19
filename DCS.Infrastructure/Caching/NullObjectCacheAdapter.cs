using System;

namespace DCS.Infrastructure.Caching
{
    public class NullObjectCacheAdapter : ICacheService
    {
        public void Add(string key, object value, DateTime absoluteExpiration)
        {
        }

        public bool TryGetValue<T>(string key, out T value) where T : class
        {
            value = null;
            return false;
        }
    }
}
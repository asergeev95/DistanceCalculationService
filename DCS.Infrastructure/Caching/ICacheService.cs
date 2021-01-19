using System;

namespace DCS.Infrastructure.Caching
{
    public interface ICacheService
    {
        void Add(string key, object value, DateTime absoluteExpiration);
        bool TryGetValue<T>(string key, out T value) where T : class;
    }
}
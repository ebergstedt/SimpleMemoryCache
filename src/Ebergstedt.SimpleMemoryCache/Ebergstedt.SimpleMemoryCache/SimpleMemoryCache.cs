using System;
using System.Linq;
using System.Runtime.Caching;
using Ebergstedt.SimpleMemoryCache.Interfaces;

namespace Ebergstedt.SimpleMemoryCache
{
    public class SimpleMemoryCache : ISimpleMemoryCache
    {
        readonly ObjectCache _cache;
        readonly CacheItemPolicy _policy;

        public SimpleMemoryCache(
                                 CacheItemPolicy policy = null)
        {
            _cache = MemoryCache.Default;

            if (policy == null)
                _policy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
                };
        }

        public T Get<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        public T Get<T>(string key, T dataToAddToCacheIfCacheResultNotFound)
        {
            return Get(key, () => dataToAddToCacheIfCacheResultNotFound);
        }

        public T Get<T>(string key, Func<T> funcResultToAddIfCacheNotFound = null)
        {
            var result = (T)_cache.Get(key);

            if (result == null && funcResultToAddIfCacheNotFound != null)
            {
                result = funcResultToAddIfCacheNotFound();
                _cache.Add(key, result, _policy);
            }

            return result;
        }

        public void Add<T>(string key, T data)
        {
            _cache.Add(key, data, _policy);
        }

        public void Add<T>(string key, Func<T> dataFunc)
        {
            Add(key, dataFunc());
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Clear()
        {
            _cache.ToList().ForEach(a => _cache.Remove(a.Key));
        }
    }
}

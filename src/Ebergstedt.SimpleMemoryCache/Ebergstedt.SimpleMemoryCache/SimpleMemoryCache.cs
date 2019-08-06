using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using Ebergstedt.SimpleMemoryCache.Interfaces;

namespace Ebergstedt.SimpleMemoryCache
{
    public class SimpleMemoryCache : ISimpleMemoryCache
    {
        readonly ObjectCache _cache;
        readonly CacheItemPolicy _policy;
        private static readonly ConcurrentDictionary<string, object> locks = new ConcurrentDictionary<string, object>();

        public SimpleMemoryCache(CacheItemPolicy policy = null)
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

        public T GetOrAdd<T>(string key, T dataToAddToCacheIfCacheResultNotFound)
        {
            return GetOrAdd(key, () => dataToAddToCacheIfCacheResultNotFound);
        }

        public T GetOrAdd<T>(string key, Func<T> funcResultToAddIfCacheNotFound = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key must not be null or empty", nameof(key));
            }

            var result = (T)_cache.Get(key);

            if (result == null && funcResultToAddIfCacheNotFound != null)
            {

                bool lockWasTaken = false;
                var l = locks.GetOrAdd(key, k => new object());
                try
                {
                    //https://blogs.msdn.microsoft.com/ericlippert/2009/03/06/locks-and-exceptions-do-not-mix/
                    Monitor.TryEnter(l, 1000, ref lockWasTaken);
                    if (lockWasTaken)
                    {
                        result = (T)_cache.Get(key);

                        if (result == null)
                        {
                            result = funcResultToAddIfCacheNotFound();
                            _cache.Add(key, result, _policy);
                        }

                    }
                    else
                    {
                        var obj = _cache.Get(key);

                        if (obj == null)
                        {
                            result = (T)funcResultToAddIfCacheNotFound();
                        }
                        else
                        {
                            result = (T) obj;
                        }
                    }
                }
                finally
                {
                    if (lockWasTaken)
                    {
                        locks.TryRemove(key, out l);
                        Monitor.Exit(l);
                    }
                }
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

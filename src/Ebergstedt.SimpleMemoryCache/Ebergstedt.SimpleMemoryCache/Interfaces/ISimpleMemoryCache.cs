using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebergstedt.SimpleMemoryCache.Interfaces
{
    public interface ISimpleMemoryCache
    {
        T Get<T>(string key);

        T GetOrAdd<T>(string key, T dataToAddToCacheIfCacheResultNotFound);

        T GetOrAdd<T>(string key, Func<T> funcResultToAddIfCacheNotFound = null);

        void Add<T>(string key, T data);

        void Add<T>(string key, Func<T> dataFunc);

        void Remove(string key);

        void Clear();
    }
}

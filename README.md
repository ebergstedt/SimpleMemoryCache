# SimpleMemoryCache

Helper library for .NET MemoryCache with generics, easy initialization and deferred methods if key data is not found.

# Method list

```C#
T Get<T>(string key);

T Get<T>(string key, T dataToAddToCacheIfCacheResultNotFound);

T Get<T>(string key, Func<T> funcResultToAddIfCacheNotFound = null);

void Add<T>(string key, T data);

void Add<T>(string key, Func<T> dataFunc);

void Remove(string key);

void Clear();
```

# Todo

Nuget
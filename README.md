# SimpleMemoryCache

Helper library for .NET MemoryCache with generics, easy initialization and deferred methods if key data is not found.

You initialize the cache by instancing *ISimpleMemoryCache*. A *SimpleMemoryCache* implementation is provided with the following constructor:

```C#
public SimpleMemoryCache(
						 CacheItemPolicy policy = null)						 					
```

**[CacheItemPolicy](https://msdn.microsoft.com/en-us/library/system.runtime.caching.cacheitempolicy(v=vs.110).aspx)** will default to a 5 minute absolute expiration caching time.

# Notes

When using a MemoryCache, keep in mind that you are using your RAM with raw data. Caching big objects will take space, so be careful with using it frivolously.

# [Nuget](https://www.nuget.org/packages/Ebergstedt.SimpleMemoryCache)

	PM> Install-Package Ebergstedt.SimpleMemoryCache

# Method list

```C#
T Get<T>(string key);

T GetOrAdd<T>(string key, T dataToAddToCacheIfCacheResultNotFound);

T GetOrAdd<T>(string key, Func<T> funcResultToAddIfCacheNotFound = null);

void Add<T>(string key, T data);

void Add<T>(string key, Func<T> dataFunc);

void Remove(string key);

void Clear();
```

# Sample usage
```C#
[Test]
public void Given_no_cache_Then_add_object_cache_by_Get_Then_get_result()
{
	string key = "Given_no_cache_Then_add_object_cache_by_Get_Then_get_result";

	string data = "data";

	var result = _simpleMemoryCache.GetOrAdd(
		key, 
		data);

	Assert.True(data == result);
}

[Test]
public void Given_no_cache_Then_add_method_cache_by_Get_Then_get_result()
{
	string key = "Given_no_cache_Then_add_method_cache_by_Get_Then_get_result";

	Func<string> getData = () => "data";

	string data = getData();

	var result = _simpleMemoryCache.GetOrAdd(
		key, 
		getData);

	Assert.True(data == result);
}

[Test]
public void Given_no_cache_Then_add_object_cache_by_Add_Then_get_result()
{
	string key = "Given_no_cache_Then_add_object_cache_by_Add_Then_get_result";

	string data = "data";

	_simpleMemoryCache.Add(
		key, 
		data);

	var result = _simpleMemoryCache.Get<string>(
		key);

	Assert.True(data == result);
}

[Test]
public void Given_no_cache_Then_add_cache_Then_clear_it_Then_no_cache_exists()
{
	string key = "Given_no_cache_Then_add_cache_Then_clear_it_Then_no_cache_exists";

	string data = "data";

	_simpleMemoryCache.Add(
		key, 
		data);

	_simpleMemoryCache.Clear();

	var result = _simpleMemoryCache.Get<string>(key);

	Assert.True(string.IsNullOrEmpty(result));
}
```

using System;
using Ebergstedt.SimpleMemoryCache.Interfaces;
using NUnit.Framework;

namespace Ebergstedt.SimpleMemoryCache.Test
{
    [TestFixture]
    public class SimpleMemoryCache_Test 
    {
        ISimpleMemoryCache _simpleMemoryCache = new SimpleMemoryCache();

        [SetUp]
        protected void SetUp()
        {
            _simpleMemoryCache.Clear();
        }

        [TearDown]
        protected void TearDown()
        {
            _simpleMemoryCache.Clear();
        }

        [Test]
        public void Given_no_cache_Then_get_no_result()
        {
            string key = "Given_no_cache_Then_get_no_result";

            var cacheResult = _simpleMemoryCache.GetOrAdd<object>(
                                                             key);

            Assert.Null(cacheResult);
        }

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

            var result = _simpleMemoryCache.GetOrAdd<string>(
                                                        key);

            Assert.True(data == result);
        }

        [Test]
        public void Given_no_cache_Then_add_method_cache_by_Add_Then_get_result()
        {
            string key = "Given_no_cache_Then_add_method_cache_by_Add_Then_get_result";

            Func<string> getData = () => "data";

            string data = getData();

            _simpleMemoryCache.Add(
                                   key, 
                                   getData);

            var result = _simpleMemoryCache.GetOrAdd<string>(
                                                        key);

            Assert.True(data == result);
        }

        [Test]
        public void Given_no_cache_Then_add_cache_Then_clear_it_Then_no_cache_exists()
        {
            string key = "Given_no_cache_Then_add_cache_Then_clear_it_Then_no_cache_exists";

            string data = "data";

            _simpleMemoryCache.Add(key, data);

            _simpleMemoryCache.Clear();

            var result = _simpleMemoryCache.GetOrAdd<string>(key);

            Assert.True(string.IsNullOrEmpty(result));
        }
    }
}

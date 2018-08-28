using System;
using System.Threading.Tasks;
using Xunit;

namespace Talk.Redis.Tests
{
    public class UnitTest1
    {
        public UnitTest1()
        {
            RedisHelper.RedisConfig = "192.168.10.107:6379,allowAdmin=true,password=redispass";
        }
        [Fact]
        public void Test1Async()
        {
            RedisHelper redis = new RedisHelper(1);
            redis.Set("key3", "hahah");
            var value = redis.GetString("key3");
            Assert.True(value == "hahah");
        }

        [Fact]
        public void Test2Async()
        {
            RedisHelper redis = new RedisHelper(3);
            redis.Set("key5", "hahah");
            var value = redis.GetString("key5");
            Assert.True(value == "hahah");
        }
    }
}

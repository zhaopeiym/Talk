using System;
using Xunit;
using Talk.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Talk.Redis.Tests
{
    public class temp
    {
        [Fact]
        public void Set_Test()
        {
            //**** 注意 ****
            //同类型的数据操作 相同的key会覆盖
            //不同类型的数据操作 相同的key会报错
            //var strConfig = "192.168.10.107:6379,allowAdmin=true,password=redispass";

            var testConfig = "192.168.10.178";//开发环境
            RedisManager redis = new RedisManager(testConfig);

            #region 键值对
            ////键值对
            //redis.Set("key22222", "");
            //Assert.True(redis.GetString("key22222") == string.Empty);
            //redis.Set("key22222", "key22222");
            //Assert.True(redis.GetString("key22222") == "key22222");

            //string a1 = null;
            //redis.Set("key_3", a1);
            //Assert.True(redis.GetString("key_3") == null);
            //redis.Set("key_6", "");
            //Assert.True(redis.GetInt("key_6") == null);
            //int? int1 = null;
            //redis.Set("key_7", int1);
            //Assert.True(redis.GetInt("key_7") == null);
            //bool? bool1 = null;
            //redis.Set("key_8", bool1);
            //Assert.True(redis.GetBoolean("key_8") == null);

            //redis.Set("key_4", 1);
            //Assert.True(redis.GetInt("key_4") == 1);

            //redis.Set("key_5", 0);
            //Assert.True(redis.GetInt("key_5") == 0);

            //redis.Set("key_9", false);
            //Assert.True(redis.GetBoolean("key_9") == false);
            //var key9 = redis.GetInt("key_9");
            //var key9s = redis.GetString("key_9");
            //var key8 = redis.GetString("key_8");
            #endregion

            #region set / 集合
            ////set
            //redis.SetAdd("key1", "123");
            //redis.SetAdd("key1", "234");
            //redis.SetAdd("key1", "666");
            //redis.SetAdd("key1", new List<string>() { "777", "888" });
            //var listSet = redis.SetMembers("key1");
            //Assert.Contains("123", listSet);
            //Assert.Contains("666", listSet);
            //Assert.Contains("888", listSet);
            #endregion

            #region hashSet
            ////hashSet
            //redis.HashSet("key2", new Dictionary<string, string>() {
            //    { "123","123"}
            //});
            //redis.HashSet("key2", new Dictionary<string, string>() {
            //    { "123","1233"}, //会覆盖上面的 123， 前部分是key 后部分是value
            //    { "1234","888"},
            //    { "1235","123"},
            //    //{ "1235","12333"}, //**报错，添加了相同的项
            //});
            //var hashList = redis.HashGetAll("key2");
            //Assert.True(hashList["123"] == "1233");
            //Assert.Contains("123", hashList.Keys);
            //Assert.Contains("888", hashList.Values);
            //Assert.True(redis.HashGet("key2", "123") == "1233");

            //redis.HashSet("key2", "123", "22222222");
            //Assert.True(redis.HashGet("key2", "123") == "22222222");
            #endregion
        }

        [Fact]
        public async Task Lock_TestAsync()
        {
            //var testConfig = "192.168.10.178";//开发环境
            //RedisManager redis = new RedisManager(testConfig);
            //List<Task> list = new List<Task>();

            //for (int i = 0; i < 3; i++)
            //{
            //    var task = Task.Run(() =>
            //    {
            //        if (redis.Database.LockTake("test", Environment.MachineName, TimeSpan.FromDays(1)))
            //        {
            //            Thread.Sleep(5000);
            //            redis.Database.LockRelease("test", Environment.MachineName);//释放锁
            //        }
            //    });
            //    list.Add(task);
            //}

            //foreach (var item in list)
            //{
            //    await item;
            //}
        }
    }
}

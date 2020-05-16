using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Talk.Redis
{
    /// <summary>
    /// 【超时问题处理】仅仅把同步读取改为异步即可
    /// syncTimeout=10000   默认是5秒，同步读取超时时间（异步读取不会有读取超时问题）  
    /// https://blog.csdn.net/qq_35633131/article/details/84069184
    /// https://docs.microsoft.com/zh-cn/dotnet/api/system.threading.threadpool.setminthreads?view=netframework-4.7.2
    /// https://www.cnblogs.com/ShaYeBlog/p/11375179.html
    /// </summary>
    public class RedisManager
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string RedisConfig { get; set; }
        /// <summary>
        /// 数据库索引
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private IDatabase database { get; set; }

        private static ConnectionMultiplexer _connection;

        private static ConnectionMultiplexer Connection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RedisConfig))
                    throw new Exception("没有配置redis连接");
                if (_connection == null || !_connection.IsConnected)
                    _connection = ConnectionMultiplexer.Connect(RedisConfig);
                return _connection;
            }
        }

        //public RedisManager(int dbIndex)
        //  : this(dbIndex, null)
        //{
        //}

        public RedisManager(int dbIndex, string config)
        {
            if (!string.IsNullOrWhiteSpace(config))
                RedisConfig = config;
            database = Connection.GetDatabase(dbIndex);
        }


        #region 键值对操作

        public async Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            return await database.StringSetAsync(key, value, expiry);
        }

        public async Task<bool> SetAsync(string key, bool value, TimeSpan? expiry = null)
        {
            return await database.StringSetAsync(key, value, expiry);
        }

        //public bool Set(string key, byte value, TimeSpan? expiry = null)
        //{
        //    return database.StringSet(key, value, expiry);
        //}

        public async Task<bool> SetAsync(string key, int value, TimeSpan? expiry = null)
        {
            return await database.StringSetAsync(key, value, expiry);
        }

        public async Task<bool> SetAsync(string key, long value, TimeSpan? expiry = null)
        {
            return await database.StringSetAsync(key, value, expiry);
        }

        public async Task<bool> SetAsync(string key, float value, TimeSpan? expiry = null)
        {
            return await database.StringSetAsync(key, value, expiry);
        }

        public async Task<bool> SetAsync(string key, double value, TimeSpan? expiry = null)
        {
            return await database.StringSetAsync(key, value, expiry);
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class, new()
        {
            return await database.StringSetAsync(key, JsonConvert.SerializeObject(value), expiry);
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await database.StringGetAsync(key);
        }

        public async Task<int?> GetIntAsync(string key)
        {
            var value = await database.StringGetAsync(key);
            if (!value.HasValue)
                return null;
            return (int)value;
        }

        public async Task<bool?> GetBooleanAsync(string key)
        {
            var value = await database.StringGetAsync(key);
            if (!value.HasValue)
                return null;
            return (bool)value;
        }

        public async Task<long?> GetLongAsync(string key)
        {
            var value = await database.StringGetAsync(key);
            if (!value.HasValue)
                return null;
            return (long)value;
        }

        public async Task<float?> GetFloatAsync(string key)
        {
            var value = await database.StringGetAsync(key);
            if (!value.HasValue)
                return null;
            return (float)value;
        }

        public async Task<double?> GetDoubleAsync(string key)
        {
            var value = await database.StringGetAsync(key);
            if (!value.HasValue)
                return null;
            return (double)value;
        }

        public async Task<object> GetAsync(string key, Type type)
        {
            var value = await database.StringGetAsync(key);
            if (!value.HasValue)
                return null;
            return JsonConvert.DeserializeObject(value, type);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await database.StringGetAsync(key);
            if (!value.HasValue)
            {
                return default(T);
            }
            if (typeof(T) == typeof(string))
            {
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
            }
            return JsonConvert.DeserializeObject<T>(value);
        }
        #endregion

        #region Set操作
        public async Task<bool> SetAddAsync(string key, string value)
        {
            return await database.SetAddAsync(key, value);
        }

        public async Task<bool> MultipleSetAddAsync(string key, RedisValue[] values)
        {
            return (await database.SetAddAsync(key, values)) > 0;
        }

        public async Task<bool> SetContainsAsync(string key, string value)
        {
            return await database.SetContainsAsync(key, value);
        }

        /// <summary>
        /// 获取Set数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string[]> SetMembersAsync(string key)
        {
            return (await database.SetMembersAsync(key)).Select(t => t.ToString()).ToArray();
        }
        #endregion

        #region Hash操作

        public async Task<string> HashGetAsync(string key, string hashField)
        {
            return await database.HashGetAsync(key, hashField);
        }

        public async Task<T> HashGetAsync<T>(string key, string hashField) where T : class, new()
        {
            string value = await database.HashGetAsync(key, hashField);
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<object> HashGetObjAsync(string key, string hashField)
        {
            string value = await database.HashGetAsync(key, hashField);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(value);
        }

        public async Task<Dictionary<string, string>> HashGetAllAsync(string key)
        {
            HashEntry[] hashEntries = await database.HashGetAllAsync(key);
            if (hashEntries == null || hashEntries.Length == 0)
            {
                return new Dictionary<string, string>();
            }
            return hashEntries.ToDictionary<HashEntry, string, string>(hashEntry => hashEntry.Name, hashEntry => hashEntry.Value);
        }

        public async Task<Dictionary<string, object>> HashGetAllObjAsync(string key)
        {
            HashEntry[] hashEntries = await database.HashGetAllAsync(key);
            if (hashEntries == null || hashEntries.Length == 0)
            {
                return new Dictionary<string, object>();
            }
            return hashEntries.ToDictionary<HashEntry, string, object>(hashEntry => hashEntry.Name, hashEntry => JsonConvert.DeserializeObject(hashEntry.Value));
        }

        /// <summary>
        /// 批量设置Hash
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="db"></param>
        public async Task MultiHashSetAsync(Dictionary<string, Dictionary<string, string>> dictionary)
        {
            var batch = database.CreateBatch();
            foreach (var item in dictionary)
            {
                HashEntry[] hashEntries = item.Value.Select(kv => new HashEntry(kv.Key, kv.Value)).ToArray();
                await batch.HashSetAsync(item.Key, hashEntries);
            }
            batch.Execute();
        }

        public async Task HashSetAsync(string key, Dictionary<string, string> dictionary)
        {
            HashEntry[] hashEntries = dictionary.Select(kv => new HashEntry(kv.Key, kv.Value)).ToArray();
            await database.HashSetAsync(key, hashEntries);
        }

        public async Task HashSetAsync(string key, Dictionary<string, object> dictionary)
        {
            HashEntry[] hashEntries = dictionary.Select(kv => new HashEntry(kv.Key, JsonConvert.SerializeObject(kv.Value))).ToArray();
            await database.HashSetAsync(key, hashEntries);
        }

        public async Task<bool> HashSetAsync(string key, string hashField, string value)
        {
            return await database.HashSetAsync(key, hashField, value);
        }

        public async Task<bool> HashSetAsync(string key, string hashField, long value)
        {
            return await database.HashSetAsync(key, hashField, value);
        }

        public async Task<bool> HashSetAsync(string key, string hashField, object value, TimeSpan? expiry = null)
        {
            bool result = await database.HashSetAsync(key, hashField, JsonConvert.SerializeObject(value));
            if (expiry != null)
            {
                await database.KeyExpireAsync(key, expiry);
            }
            return result;
        }

        public async Task<bool> HashSetAsync<T>(string key, string hashField, T value) where T : class, new()
        {
            return await database.HashSetAsync(key, hashField, JsonConvert.SerializeObject(value));
        }

        public async Task<long> HashIncrementAsync(string key, string hashField, long value = 1L)
        {
            return await database.HashIncrementAsync(key, hashField, value);
        }

        public async Task<long> HashDecrementAsync(string key, string hashField, long value = 1L)
        {
            return await database.HashDecrementAsync(key, hashField, value);
        }

        public async Task<bool> HashExistsAsync(string key, string hashField)
        {
            return await database.HashExistsAsync(key, hashField);
        }
        #endregion

        #region List 操作
        public async Task<long> EnqueueAsync(string key, string value)
        {
            return await database.ListLeftPushAsync(key, value);
        }

        public async Task<long> EnqueueAsync<T>(string key, T value)
        {
            return await database.ListLeftPushAsync(key, JsonConvert.SerializeObject(value));
        }

        public async Task<string> DequeueAsync(string key)
        {
            return await database.ListRightPopAsync(key);
        }

        public async Task<T> DequeueAsync<T>(string key)
        {
            string value = await DequeueAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<List<string>> DequeueAllAsync(string key)
        {
            long length = await database.ListLengthAsync(key);
            if (length == 0)
            {
                return new List<string>();
            }
            List<string> list = new List<string>();
            for (int i = 0; i < length; i++)
            {
                list.Add(await DequeueAsync<string>(key));
            }
            return list;
        }

        public async Task<List<T>> DequeueAllAsync<T>(string key)
        {
            long length = await database.ListLengthAsync(key);
            if (length == 0)
            {
                return new List<T>();
            }
            List<T> list = new List<T>();
            for (int i = 0; i < length; i++)
            {
                list.Add(await DequeueAsync<T>(key));
            }
            return list;
        }

        public async Task<List<T>> DequeueListAsync<T>(string key, int count)
        {
            long length = await database.ListLengthAsync(key);
            if (length == 0)
            {
                return new List<T>();
            }
            if (count > length)
            {
                count = (int)length;
            }
            List<T> list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                list.Add(await DequeueAsync<T>(key));
            }
            return list;
        }

        public async Task<long> ListLengthAsync(string key)
        {
            return await database.ListLengthAsync(key);
        }

        public async Task<List<T>> ListRangeAsync<T>(string key, long start = 0L, long stop = -1L)
        {
            RedisValue[] values = await database.ListRangeAsync(key, start, stop);
            if (values == null || values.Length == 0)
            {
                return new List<T>();
            }
            return values.Select(redisValue => JsonConvert.DeserializeObject<T>(redisValue)).ToList();
        }

        public async Task<string> ListGetByIndexAsync(string key, long index)
        {
            return await database.ListGetByIndexAsync(key, index);
        }

        public async Task<T> ListGetByIndexAsync<T>(string key, long index)
        {
            string value = await database.ListGetByIndexAsync(key, index);
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }
        #endregion

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            return await database.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 判断是否存在键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            return await database.KeyExistsAsync(key);
        }

        /// <summary>
        /// 计数器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">只有第一次设置有效期生效</param>
        /// <returns></returns>
        public async Task<long> SetStringIncrAsync(string key, long value, TimeSpan? expiry = null)
        {
            try
            {
                var nubmer = await database.StringIncrementAsync(key, value);
                if (nubmer == 1 && expiry != null)//只有第一次设置有效期（防止覆盖）
                    await database.KeyExpireAsync(key, expiry);//设置有效期
                return nubmer;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// 计数器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<long> SetStringIncrAsync(string key, TimeSpan? expiry = null)
        {
            return await SetStringIncrAsync(key, 1, expiry);
        }

        /// <summary>
        /// 读取计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> GetStringIncrAsync(string key)
        {
            var value = await GetStringAsync(key);
            return string.IsNullOrWhiteSpace(value) ? 0 : long.Parse(value);
        }

        public async Task FlushDbAsync()
        {
            var endPoints = database.Multiplexer.GetEndPoints();
            foreach (var endpoint in endPoints)
            {
                await database.Multiplexer.GetServer(endpoint).FlushDatabaseAsync();
            }
        }

        public async Task FlushAllAsync()
        {
            var endPoints = database.Multiplexer.GetEndPoints();
            foreach (var endpoint in endPoints)
            {
                await database.Multiplexer.GetServer(endpoint).FlushAllDatabasesAsync();
            }
        }

        public async Task SaveAsync()
        {
            SaveType saveType = SaveType.BackgroundSave;
            var endPoints = database.Multiplexer.GetEndPoints();

            foreach (var endpoint in endPoints)
            {
                await database.Multiplexer.GetServer(endpoint).SaveAsync(saveType);
            }
        }

        public async Task<long> IncrementAsync(string key, long value = 1L)
        {
            return await database.StringIncrementAsync(key, value);
        }

        public async Task<bool> KeyExpireAsync(string key, TimeSpan? expiry)
        {
            return await database.KeyExpireAsync(key, expiry);
        }

        public async Task<bool> KeyExpireAsync(string key, DateTime? expiry)
        {
            return await database.KeyExpireAsync(key, expiry);
        }

        public IEnumerable<RedisValue> SetScan(string key)
        {
            return database.SetScan(key);
        }
    }
}

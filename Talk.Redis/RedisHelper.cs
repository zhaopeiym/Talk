using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Talk.Redis
{
    public class RedisHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string RedisConfig { get; set; }
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

        public RedisHelper(int dbIndex)
          : this(dbIndex, null)
        {
        }

        public RedisHelper(int dbIndex, string config)
        {
            if (!string.IsNullOrWhiteSpace(config))
                RedisConfig = config;
            database = Connection.GetDatabase(dbIndex);
        }


        #region 键值对操作

        public bool Set(string key, string value, TimeSpan? expiry = null)
        {
            return database.StringSet(key, value, expiry);
        }

        public bool Set(string key, bool value, TimeSpan? expiry = null)
        {
            return database.StringSet(key, value, expiry);
        }

        public bool Set(string key, byte value, TimeSpan? expiry = null)
        {
            return database.StringSet(key, value, expiry);
        }

        public bool Set(string key, int value, TimeSpan? expiry = null)
        {
            return database.StringSet(key, value, expiry);
        }

        public bool Set(string key, long value, TimeSpan? expiry = null)
        {
            return database.StringSet(key, value, expiry);
        }

        public bool Set(string key, float value, TimeSpan? expiry = null)
        {
            return database.StringSet(key, value, expiry);
        }

        public bool Set(string key, double value, TimeSpan? expiry = null)
        {
            return database.StringSet(key, value, expiry);
        }

        public bool Set<T>(string key, T value, TimeSpan? expiry = null) where T : class, new()
        {
            return database.StringSet(key, JsonConvert.SerializeObject(value), expiry);
        }

        public string GetString(string key)
        {
            return database.StringGet(key);
        }

        public int? GetInt(string key)
        {
            var value = database.StringGet(key);
            if (!value.HasValue)
                return null;
            return (int)value;
        }

        public bool? GetBoolean(string key)
        {
            var value = database.StringGet(key);
            if (!value.HasValue)
                return null;
            return (bool)value;
        }

        public long? GetLong(string key)
        {
            var value = database.StringGet(key);
            if (!value.HasValue)
                return null;
            return (long)value;
        }

        public float? GetFloat(string key)
        {
            var value = database.StringGet(key);
            if (!value.HasValue)
                return null;
            return (float)value;
        }

        public double? GetDouble(string key)
        {
            var value = database.StringGet(key);
            if (!value.HasValue)
                return null;
            return (double)value;
        }

        public object Get(string key, Type type)
        {
            var value = database.StringGet(key);
            if (!value.HasValue)
                return null;
            return JsonConvert.DeserializeObject(value, type);
        }

        public T Get<T>(string key)
        {
            var value = database.StringGet(key);
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
        public bool SetAdd(string key, string value)
        {
            return database.SetAdd(key, value);
        }

        public bool MultipleSetAdd(string key, RedisValue[] values)
        {
            return database.SetAdd(key, values) > 0;
        }

        public bool SetContains(string key, string value)
        {
            return database.SetContains(key, value);
        }

        /// <summary>
        /// 获取Set数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] SetMembers(string key)
        {
            return database.SetMembers(key).Select(t => t.ToString()).ToArray();
        }
        #endregion

        #region Hash操作

        public string HashGet(string key, string hashField)
        {
            return database.HashGet(key, hashField);
        }

        public T HashGet<T>(string key, string hashField) where T : class, new()
        {
            string value = database.HashGet(key, hashField);
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public object HashGetObj(string key, string hashField)
        {
            string value = database.HashGet(key, hashField);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(value);
        }

        public Dictionary<string, string> HashGetAll(string key)
        {
            HashEntry[] hashEntries = database.HashGetAll(key);
            if (hashEntries == null || hashEntries.Length == 0)
            {
                return new Dictionary<string, string>();
            }
            return hashEntries.ToDictionary<HashEntry, string, string>(hashEntry => hashEntry.Name, hashEntry => hashEntry.Value);
        }

        public Dictionary<string, object> HashGetAllObj(string key)
        {
            HashEntry[] hashEntries = database.HashGetAll(key);
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
        public void MultiHashSet(Dictionary<string, Dictionary<string, string>> dictionary)
        {
            var batch = database.CreateBatch();
            foreach (var item in dictionary)
            {
                HashEntry[] hashEntries = item.Value.Select(kv => new HashEntry(kv.Key, kv.Value)).ToArray();
                batch.HashSetAsync(item.Key, hashEntries);
            }
            batch.Execute();
        }

        public void HashSet(string key, Dictionary<string, string> dictionary)
        {
            HashEntry[] hashEntries = dictionary.Select(kv => new HashEntry(kv.Key, kv.Value)).ToArray();
            database.HashSet(key, hashEntries);
        }

        public void HashSet(string key, Dictionary<string, object> dictionary)
        {
            HashEntry[] hashEntries = dictionary.Select(kv => new HashEntry(kv.Key, JsonConvert.SerializeObject(kv.Value))).ToArray();
            database.HashSet(key, hashEntries);
        }

        public bool HashSet(string key, string hashField, string value)
        {
            return database.HashSet(key, hashField, value);
        }

        public bool HashSet(string key, string hashField, long value)
        {
            return database.HashSet(key, hashField, value);
        }

        public bool HashSet(string key, string hashField, object value, TimeSpan? expiry = null)
        {
            bool result = database.HashSet(key, hashField, JsonConvert.SerializeObject(value));
            if (expiry != null)
            {
                database.KeyExpire(key, expiry);
            }
            return result;
        }

        public bool HashSet<T>(string key, string hashField, T value) where T : class, new()
        {
            return database.HashSet(key, hashField, JsonConvert.SerializeObject(value));
        }

        public long HashIncrement(string key, string hashField, long value = 1L)
        {
            return database.HashIncrement(key, hashField, value);
        }

        public long HashDecrement(string key, string hashField, long value = 1L)
        {
            return database.HashDecrement(key, hashField, value);
        }

        public bool HashExists(string key, string hashField)
        {
            return database.HashExists(key, hashField);
        }
        #endregion

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return database.KeyDelete(key);
        }

        /// <summary>
        /// 判断是否存在键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return database.KeyExists(key);
        }

        /// <summary>
        /// 计数器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">只有第一次设置有效期生效</param>
        /// <returns></returns>
        public long SetStringIncr(string key, long value, TimeSpan? expiry = null)
        {
            try
            {
                var nubmer = database.StringIncrement(key, value);
                if (nubmer == 1 && expiry != null)//只有第一次设置有效期（防止覆盖）
                    database.KeyExpireAsync(key, expiry);//设置有效期
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
        public long SetStringIncr(string key, TimeSpan? expiry = null)
        {
            return SetStringIncr(key, 1, expiry);
        }

        /// <summary>
        /// 读取计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetStringIncr(string key)
        {
            var value = GetString(key);
            return string.IsNullOrWhiteSpace(value) ? 0 : long.Parse(value);
        }

        public void FlushDb()
        {
            var endPoints = database.Multiplexer.GetEndPoints();
            foreach (var endpoint in endPoints)
            {
                database.Multiplexer.GetServer(endpoint).FlushDatabase();
            }
        }

        public void FlushAll()
        {
            var endPoints = database.Multiplexer.GetEndPoints();
            foreach (var endpoint in endPoints)
            {
                database.Multiplexer.GetServer(endpoint).FlushAllDatabases();
            }
        }

        public void Save()
        {
            SaveType saveType = SaveType.BackgroundSave;
            var endPoints = database.Multiplexer.GetEndPoints();

            foreach (var endpoint in endPoints)
            {
                database.Multiplexer.GetServer(endpoint).Save(saveType);
            }
        }

        public long Increment(string key, long value = 1L)
        {
            return database.StringIncrement(key, value);
        }

        public long Enqueue(string key, string value)
        {
            return database.ListLeftPush(key, value);
        }

        public long Enqueue<T>(string key, T value)
        {
            return database.ListLeftPush(key, JsonConvert.SerializeObject(value));
        }

        public string Dequeue(string key)
        {
            return database.ListRightPop(key);
        }

        public T Dequeue<T>(string key)
        {
            string value = Dequeue(key);
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public List<string> DequeueAll(string key)
        {
            long length = database.ListLength(key);
            if (length == 0)
            {
                return new List<string>();
            }
            List<string> list = new List<string>();
            for (int i = 0; i < length; i++)
            {
                list.Add(Dequeue<string>(key));
            }
            return list;
        }

        public List<T> DequeueAll<T>(string key)
        {
            long length = database.ListLength(key);
            if (length == 0)
            {
                return new List<T>();
            }
            List<T> list = new List<T>();
            for (int i = 0; i < length; i++)
            {
                list.Add(Dequeue<T>(key));
            }
            return list;
        }

        public List<T> DequeueList<T>(string key, int count)
        {
            long length = database.ListLength(key);
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
                list.Add(Dequeue<T>(key));
            }
            return list;
        }

        public long ListLength(string key)
        {
            return database.ListLength(key);
        }

        public List<T> ListRange<T>(string key, long start = 0L, long stop = -1L)
        {
            RedisValue[] values = database.ListRange(key, start, stop);
            if (values == null || values.Length == 0)
            {
                return new List<T>();
            }
            return values.Select(redisValue => JsonConvert.DeserializeObject<T>(redisValue)).ToList();
        }

        public string ListGetByIndex(string key, long index)
        {
            return database.ListGetByIndex(key, index);
        }

        public T ListGetByIndex<T>(string key, long index)
        {
            string value = database.ListGetByIndex(key, index);
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public bool KeyExpire(string key, TimeSpan? expiry)
        {
            return database.KeyExpire(key, expiry);
        }

        public bool KeyExpire(string key, DateTime? expiry)
        {
            return database.KeyExpire(key, expiry);
        }

        public IEnumerable<RedisValue> SetScan(string key)
        {
            return database.SetScan(key);
        }
    }
}

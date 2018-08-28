using System;
using System.Collections.Generic;
using System.Linq;

namespace Talk.Cache
{
    /// <summary>
    /// 缓存对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class CacheData<T>
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// 要存储的数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 简易缓存器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EasyCache<T>
    {
        private readonly static Dictionary<string, CacheData<T>> list = new Dictionary<string, CacheData<T>>();
        public static object lockObj = new object();

        private string _key;
        private TimeSpan _timeSpan;
        public EasyCache(string key, TimeSpan timeSpan)
        {
            _key = key;
            _timeSpan = timeSpan;
        }

        /// <summary>
        /// 添加数据
        /// 不能重复添加，必须在 GetData 为null 才能添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cacheData"></param>
        public bool AddData(T data)
        {
            lock (lockObj)
            {
                if (list.ContainsKey(_key))
                    return false;
                list.Add(_key, new CacheData<T>()
                {
                    Data = data,
                    ExpirationTime = DateTime.Now.Add(_timeSpan)
                });
                return true;
            }
        }

        /// <summary>
        /// 修改 (如果缓存数据过期 则修改失败)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns>返回false代表修改失败</returns>
        internal bool ModifyData(string key, T data)
        {
            //查询出过期数据
            var keys = list.Where(t => t.Value.ExpirationTime <= DateTime.Now).Select(t => t.Key).ToArray();
            lock (lockObj)
            {
                foreach (var k in keys)
                {
                    list.Remove(k);//移除过期数据
                }
                if (!list.ContainsKey(key))
                    return false;
                list[key].Data = data;
                return true;
            }
        }

        /// <summary>
        ///获取存储对象
        /// 如果没有取到 则返回default(T)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetData()
        {
            var keys = list.Where(t => t.Value.ExpirationTime <= DateTime.Now).Select(t => t.Key).ToArray();
            lock (lockObj)
            {
                foreach (var k in keys)
                {
                    list.Remove(k);
                }
                if (list.ContainsKey(_key))
                    return list[_key].Data;
                return default(T);
            }
        }

        /// <summary>
        /// 如果没有取到 则返回null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal CacheData<T> GetCache()
        {
            var keys = list.Where(t => t.Value.ExpirationTime <= DateTime.Now).Select(t => t.Key).ToArray();
            lock (lockObj)
            {
                foreach (var k in keys)
                {
                    list.Remove(k);
                }
                if (list.ContainsKey(_key))
                    return list[_key];
                return null;
            }
        }
    }

    /// <summary>
    /// 执行计数器
    /// </summary>
    public class ExecuteNum
    {
        private EasyCache<long> easy;
        public string _key;  //缓存对象 key
        private TimeSpan _timeSpan; //过期间隔

        public ExecuteNum(string key, TimeSpan timeSpan)
        {
            _key = key;
            _timeSpan = timeSpan;
            easy = new EasyCache<long>(key, timeSpan);
            easy.AddData(0);
        }

        /// <summary>
        /// 获取（时间内）执行次数
        /// </summary>
        /// <returns></returns>
        public long GetNum()
        {
            //获取缓存对象
            var data = easy.GetCache();
            if (data == null)
            {
                data = new CacheData<long>() { Data = 0 };
                easy.AddData(0);//如果过期了，重新添加缓存对象
            }
            //累计计算
            var num = data.Data + 1;
            easy.ModifyData(_key, num);
            return num;
        }
    }
}

using Microsoft.Azure;
using Moe.Lib;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jinyinmao.Daemon.Services
{
    public static class RedisCacheHelper
    {
        private static Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(GetConfigurationOptions(CloudConfigurationManager.GetSetting("RedisContextConnection"))));

        //private static readonly Lazy<ConnectionMultiplexer> LazyConnection;
        public static IDatabase GetRedisClient(int dataBase = 4, string strConfiguration = "")
        {
            if (!string.IsNullOrEmpty(strConfiguration))
            {
                LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(GetConfigurationOptions(strConfiguration)));
            }
            return RedisConnectionMultiplexer.GetDatabase(dataBase, new object());
        }

        #region 连接设置

        private static ConnectionMultiplexer RedisConnectionMultiplexer
        {
            get { return LazyConnection.Value; }
        }

        private static ConfigurationOptions GetConfigurationOptions(string bizRedisConnectionString)
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(bizRedisConnectionString, true);
            options.AbortOnConnectFail = false;
            options.AllowAdmin = true;
            options.ConnectRetry = 10;
            options.ConnectTimeout = 20000;
            options.DefaultDatabase = 0;
            options.ResponseTimeout = 20000;
            options.Ssl = false;
            options.SyncTimeout = 20000;
            return options;
        }

        #endregion 连接设置

        #region 清除指定Key的数据

        /// <summary>
        /// 清除指定Key的数据
        /// </summary>
        /// <param name="redisDatabase">The redis database.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ClearDataFromCache(this IDatabase redisDatabase, string cacheName, string cacheId)
        {
            string cacheKey = $"{cacheName}:{cacheId}";
            return redisDatabase.KeyDelete(cacheKey);
        }

        #endregion 清除指定Key的数据

        #region Hash获取指定Key的数据

        /// <summary>
        /// Hash获取指定Key的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisDatabase">The redis database.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        public static IList<T> HashGetAllFromCache<T>(this IDatabase redisDatabase, string cacheName, string cacheId)
        {
            string cacheKey = $"{cacheName}:{cacheId}";
            if (string.IsNullOrEmpty(cacheName))
            {
                cacheKey = cacheId;
            }
            if (string.IsNullOrEmpty(cacheId))
            {
                cacheKey = cacheName;
            }

            try
            {
                var data = redisDatabase.HashGetAll(cacheKey);
               
                return data.Select(item => JsonConvert.DeserializeObject<T>(item.Value)).ToList();
            }
            catch (Exception ex)
            {
                return default(IList<T>);
            }
        }

        #endregion Hash获取指定Key的数据


        



        #region Hash获取指定Key的数据

        /// <summary>
        /// Hash获取指定Key的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisDatabase">The redis database.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <param name="field">The field.</param>
        /// <returns>T.</returns>
        public static T HashGetFromCache<T>(this IDatabase redisDatabase, string cacheName, string cacheId, string field)
        {
            string cacheKey = $"{cacheName}:{cacheId}";

            try
            {
                string data = redisDatabase.HashGet(cacheKey, field);
                return data.IsNotNullOrWhiteSpace() ? JsonConvert.DeserializeObject<T>(data) : default(T);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        #endregion Hash获取指定Key的数据

        #region Hash设定指定Key的数据

        /// <summary>
        /// Hash设定指定Key的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisDatabase">The redis database.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <param name="field">The field.</param>
        /// <param name="data">The data.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool HashSetToCache<T>(this IDatabase redisDatabase, string cacheName, string cacheId, string field, T data, TimeSpan cacheTime)
        {
            string cacheKey = $"{cacheName}:{cacheId}";
            string dataJson = JsonConvert.SerializeObject(data);
            return redisDatabase.HashSet(cacheKey, field, dataJson);
        }

        #endregion Hash设定指定Key的数据

        #region 获取指定Key的数据

        /// <summary>
        /// 获取指定Key的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisDatabase">The redis client.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <returns>T.</returns>
        public static T GetDataFromCache<T>(this IDatabase redisDatabase, string cacheName, string cacheId)
        {
            string cacheKey = $"{cacheName}:{cacheId}";
            try
            {
                string data = redisDatabase.StringGet(cacheKey);
                return data.IsNotNullOrWhiteSpace() ? JsonConvert.DeserializeObject<T>(data) : default(T);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        #endregion 获取指定Key的数据

        #region 设定指定Key的数据

        /// <summary>
        /// 设定指定Key的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisDatabase">The redis client.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns>Task.</returns>
        public static bool SetDataToCache<T>(this IDatabase redisDatabase, string cacheName, string cacheId, T data, TimeSpan cacheTime)
        {
            string cacheKey = $"{cacheName}:{cacheId}";
            string dataJson = JsonConvert.SerializeObject(data);
            return redisDatabase.StringSet(cacheKey, dataJson, cacheTime);
        }

        #endregion 设定指定Key的数据

        #region 多条删除

        /// <summary>
        /// 多条删除
        /// </summary>
        /// <param name="redisDatabase">The redis database.</param>
        /// <param name="redisKey">The redis key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DatectKeys(this IDatabase redisDatabase, RedisKey[] redisKey)
        {
            try
            {
                redisDatabase.KeyDelete(redisKey);
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        #endregion 多条删除
    }
}

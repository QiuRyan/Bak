using Moe.Lib;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DataTransfer
{
    /// <summary>
    ///     Class RedisHelper.
    /// </summary>
    public static class RedisHelper
    {
        /// <summary>
        ///     The grainscount
        /// </summary>
        private const int GRAINSCOUNT = 4;

        /// <summary>
        ///     The connection string0
        /// </summary>
        private static readonly string ConnectionString0 = ConfigurationManager.AppSettings.Get("RedisConnectionString-0");

        /// <summary>
        ///     The connection string1
        /// </summary>
        private static readonly string ConnectionString1 = ConfigurationManager.AppSettings.Get("RedisConnectionString-1");

        /// <summary>
        ///     The connection string2
        /// </summary>
        private static readonly string ConnectionString2 = ConfigurationManager.AppSettings.Get("RedisConnectionString-2");

        /// <summary>
        ///     The connection string3
        /// </summary>
        private static readonly string ConnectionString3 = ConfigurationManager.AppSettings.Get("RedisConnectionString-3");

        /// <summary>
        ///     The lazy connections
        /// </summary>
        private static readonly List<Lazy<ConnectionMultiplexer>> LazyConnections;

        /// <summary>
        ///     The letters
        /// </summary>
        private static readonly string Letters;

        /// <summary>
        ///     The options
        /// </summary>
        private static readonly List<ConfigurationOptions> Options = GetConfigurationOptions(new List<string> { ConnectionString0, ConnectionString1, ConnectionString2, ConnectionString3 });

        //private readonly RetryPolicy retryPolicy;

        /// <summary>
        ///     Initializes static members of the <see cref="RedisHelper" /> class.
        /// </summary>
        static RedisHelper()
        {
            LazyConnections = Options.Select(o => new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(o))).ToList();
            Letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }

        /// <summary>
        ///     Gets the redis connection multiplexers.
        /// </summary>
        /// <value>The redis connection multiplexers.</value>
        private static List<ConnectionMultiplexer> RedisConnectionMultiplexers
        {
            get { return LazyConnections.Select(l => l.Value).ToList(); }
        }

        /// <summary>
        ///     Checks if key exist.
        /// </summary>
        /// <param name="redisDatabase">The redis database.</param>
        /// <param name="key">The key.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> CheckIfKeyExist(this IDatabase redisDatabase, string key)
        {
            return await redisDatabase.KeyExistsAsync(key);
        }

        /// <summary>
        ///     Gets the activity redis client by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IDatabase.</returns>
        public static IDatabase GetActivityRedisClientById(string id)
        {
            return RedisConnectionMultiplexers[GetIndexById(id)].GetDatabase(0, new object());
        }

        /// <summary>
        ///     Gets the client by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ConfigurationOptions.</returns>
        public static ConfigurationOptions GetClientById(string id)
        {
            return Options[GetIndexById(id)];
        }

        /// <summary>
        ///     read data from cache as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisDatabase">The redis database.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static async Task<T> ReadDataFromCacheAsync<T>(this IDatabase redisDatabase, string cacheKey)
        {
            try
            {
                string data = await redisDatabase.StringGetAsync(cacheKey);
                return data.IsNotNullOrWhiteSpace() ? JsonConvert.DeserializeObject<T>(data) : default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        ///     Gets the configuration options.
        /// </summary>
        /// <param name="conStrList">The con string list.</param>
        /// <returns>List&lt;ConfigurationOptions&gt;.</returns>
        private static List<ConfigurationOptions> GetConfigurationOptions(IEnumerable<string> conStrList)
        {
            return conStrList.Select(s => GetConfigurationOptions(s)).ToList();
        }

        /// <summary>
        ///     Gets the configuration options.
        /// </summary>
        /// <param name="redisConnectiongString">The redis connectiong string.</param>
        /// <returns>ConfigurationOptions.</returns>
        private static ConfigurationOptions GetConfigurationOptions(string redisConnectiongString)
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(redisConnectiongString, true);
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

        /// <summary>
        ///     Gets the index by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Int32.</returns>
        private static int GetIndexById(string id)
        {
            char index = char.ToUpperInvariant(id[31]);
            return Letters.IndexOf(index) % GRAINSCOUNT;
        }
    }
}
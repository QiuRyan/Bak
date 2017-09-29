using System;
using StackExchange.Redis;

namespace jinyinmao.Signature.lib
{
    public static class RedisHelper
    {
        private static readonly Lazy<IDatabase> redisDatabase = new Lazy<IDatabase>(() => ConfigManager.GetBizRedisClient());

        public static string GetStringValue(string key)
        {
            RedisValue redisvalue = redisDatabase.Value.StringGet(key);
            return redisvalue.HasValue ? redisvalue.ToString() : string.Empty;
        }

        public static void HashSet(string key, string value)
        {
            redisDatabase.Value.ListLeftPush(key, value);
        }

        public static bool SetStringValue(string key, string value)
        {
            return redisDatabase.Value.StringSet(key, value);
        }
    }
}
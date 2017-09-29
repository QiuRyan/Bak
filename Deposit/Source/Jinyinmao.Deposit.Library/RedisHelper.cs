// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : RedisHelper.cs
// Created          : 2017-08-10  10:46
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:20
// ******************************************************************************************************
// <copyright file="RedisHelper.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using Jinyinmao.Deposit.Config;
using StackExchange.Redis;

namespace Jinyinmao.Deposit.Lib
{
    public class RedisHelper
    {
        private static readonly Lazy<IDatabase> redisDatabase = new Lazy<IDatabase>(() => ConfigManager.GetBizRedisClient());

        public static string GetStringValue(string key)
        {
            try
            {
                RedisValue redisvalue = redisDatabase.Value.StringGet(key);
                return redisvalue.HasValue ? redisvalue.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "查询Redis报错", key, key);
                return string.Empty;
            }
        }

        public static bool KeyExists(string key)
        {
            return redisDatabase.Value.KeyExists(key);
        }

        public static bool SetStringValue(string key, string value, TimeSpan timeSpan)
        {
            return redisDatabase.Value.StringSet(key, value, timeSpan);
        }
    }
}
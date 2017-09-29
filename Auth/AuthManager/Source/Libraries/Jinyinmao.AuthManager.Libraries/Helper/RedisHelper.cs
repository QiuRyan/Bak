// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : RedisHelper.cs
// Created          : 2016-12-14  20:14
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:14
// ***********************************************************************
// <copyright file="RedisHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Moe.Lib.Jinyinmao;
using StackExchange.Redis;
using System;

namespace Jinyinmao.AuthManager.Libraries.Helper
{
    public static class RedisHelper
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static RedisHelper()
        {
            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(App.Configurations.GetConfig<AuthApiConfig>().JYMRedisConnectionString));
        }

        private static ConnectionMultiplexer RedisConnectionMultiplexer
        {
            get { return LazyConnection.Value; }
        }

        /// <summary>
        ///     Gets the xian feng redis client.
        /// </summary>
        /// <returns>IDatabase.</returns>
        public static IDatabase GetUserRedisClient()
        {
            return RedisConnectionMultiplexer.GetDatabase(2, new object());
        }

        private static ConfigurationOptions GetConfigurationOptions(string bizRedisConnectiongString)
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(bizRedisConnectiongString, true);
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
    }
}
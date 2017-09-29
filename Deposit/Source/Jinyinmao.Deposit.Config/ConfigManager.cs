// ******************************************************************************************************
// Project : Jinyinmao.Deposit File : ConfigManager.cs Created : 2017-08-10 11:21
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-08-10 11:39 ******************************************************************************************************
// <copyright file="ConfigManager.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using Exceptionless;
using Exceptionless.Dependency;
using Exceptionless.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using MoeLib.Diagnostics;
using StackExchange.Redis;
using ExponentialRetry = Microsoft.WindowsAzure.Storage.RetryPolicies.ExponentialRetry;

namespace Jinyinmao.Deposit.Config
{
    /// <summary>
    /// Class ConfigManager.
    /// </summary>
    public static class ConfigManager
    {
        private static readonly Lazy<CloudTableClient> CloudTableClient;
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static ConfigManager()
        {
            ConfigurationOptions options = GetConfigurationOptions();
            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
            CloudTableClient = new Lazy<CloudTableClient>(() => InitTableClient());
        }

        /// <summary>
        /// Gets the exceptionless default client.
        /// </summary>
        /// <value>The exceptionless default client.</value>
        public static ExceptionlessClient ExceptionlessDefaultClient
        {
            get
            {
                ExceptionlessClient.Default.Configuration.ServerUrl = ExceptionlessServerUrl;
                ExceptionlessClient.Default.Configuration.ApiKey = ExceptionlessApiKey;
                ExceptionlessClient.Default.Configuration.SubmissionBatchSize = SubmissionBatchSize;
                ExceptionlessClient.Default.Configuration.Resolver.Register<IObjectStorage>(new InMemoryObjectStorage(10000));
                ExceptionlessClient.Default.Configuration.UseSessions(false);

                return ExceptionlessClient.Default;
            }
        }

        /// <summary>
        /// 资产服务SF基地址
        /// </summary>
        /// <value>The asset servicefabric base URL.</value>
        public static string AssetServicefabricBaseUrl => App.Configurations.GetConfig<DepositConfig>().AssetServicefabricBaseUrl;

        /// <summary>
        /// 标的创建回调队列
        /// </summary>
        /// <value>The bid notification queue.</value>
        public static string BidNotificationQueue => App.Configurations.GetConfig<DepositConfig>().BidNotificationQueue;

        /// <summary>
        /// 标的还款队列
        /// </summary>
        /// <value>The bid notification queue.</value>
        public static string BidRepayQueue => App.Configurations.GetConfig<DepositConfig>().BidRepayQueue;

        public static string BizDBConnectionString => App.Configurations.GetConfig<DepositConfig>().BizDBConnectionString;
        public static string BizRedisConnectiongString => App.Configurations.GetConfig<DepositConfig>().BizRedisConnectiongString;
        public static string BookNotificationQueue => App.Configurations.GetConfig<DepositConfig>().BookNotificationQueue;
        /// <summary>
        /// Gets the business queue.
        /// </summary>
        /// <value>The business queue.</value>
        public static string BusinessQueue => App.Configurations.GetConfig<DepositConfig>().BusinessQueue;
        /// <summary>
        /// 资产系统
        /// </summary>
        /// <value>The business role.</value>
        public static string BusinessRole => App.Configurations.GetConfig<DepositConfig>().BusinessRole;
        public static string CouponDBConnectionString => App.Configurations.GetConfig<DepositConfig>().CouponDBConnectionString;
        /// <summary>
        /// Gets the coupon service role.
        /// </summary>
        /// <value>The coupon service role.</value>
        public static string CouponServiceRole => App.Configurations.GetConfig<DepositConfig>().CouponServiceRole;
        /// <summary>
        /// Gets the coupon use queue.
        /// </summary>
        /// <value>The coupon use queue.</value>
        public static string CouponUseQueue => App.Configurations.GetConfig<DepositConfig>().CouponUseQueue;
        /// <summary>
        /// Gets the exceptionless API key.
        /// </summary>
        /// <value>The exceptionless API key.</value>
        private static string ExceptionlessApiKey => App.Configurations.GetConfig<DepositConfig>().ExceptionlessApiKey;
        /// <summary>
        /// Gets the exceptionless server URL.
        /// </summary>
        /// <value>The exceptionless server URL.</value>
        private static string ExceptionlessServerUrl => App.Configurations.GetConfig<DepositConfig>().ExceptionlessServerUrl;
        /// <summary>
        /// Gets the finish create account queue.
        /// </summary>
        /// <value>The finish create account queue.</value>
        public static string FinishCreateAccountQueue => App.Configurations.GetConfig<DepositConfig>().FinishCreateAccountQueue;
        public static string JYMMarketingIdentifier => App.Configurations.GetConfig<DepositConfig>().JYMMarketingIdentifier;
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public static ILogger Logger => App.LogManager.CreateLogger();
        /// <summary>
        /// 队列请求重试次数
        /// </summary>
        /// <value>The maximum retry count.</value>
        public static int MaxRetryCount => App.Configurations.GetConfig<DepositConfig>().MaxRetryCount;
        /// <summary>
        /// 短信队列
        /// </summary>
        /// <value>The message queue.</value>
        public static string MessageQueue => App.Configurations.GetConfig<DepositConfig>().MessageQueue;
        /// <summary>
        /// 短信系统
        /// </summary>
        /// <value>The message service role.</value>
        public static string MessageServiceRole => App.Configurations.GetConfig<DepositConfig>().MessageServiceRole;
        /// <summary>
        /// Gets the notify cloud table.
        /// </summary>
        /// <value>The notify cloud table.</value>
        public static CloudTable NotifyCloudTable => CloudTableClient.Value.GetTableReference("GatewayNotify");
        /// <summary>
        ///返利流水队列名称
        /// </summary>
        /// <value>The bid repay queue.</value>
        public static string RebateTransactionQueue => App.Configurations.GetConfig<DepositConfig>().RebateTransactionQueue;
        /// <summary>
        /// 充值回调队列
        /// </summary>
        /// <value>The recharge notification queue.</value>
        public static string RechargeNotificationQueue => App.Configurations.GetConfig<DepositConfig>().RechargeNotificationQueue;

        /// <summary>
        /// Gets the regular invest notification queue.
        /// </summary>
        /// <value>The regular invest notification queue.</value>
        public static string RegularInvestNotificationQueue => App.Configurations.GetConfig<DepositConfig>().RegularInvestNotificationQueue;

        /// <summary>
        /// Gets the service bus connection string.
        /// </summary>
        /// <value>The service bus connection string.</value>
        public static string ServiceBusConnectionString => App.Configurations.GetConfig<DepositConfig>().ServiceBusConnectionString;

        public static string StorageConnectionString => App.Configurations.GetConfig<DepositConfig>().DataConnectionString;

        private static int SubmissionBatchSize => App.Configurations.GetConfig<DepositConfig>().SubmissionBatchSize;
        public static string TirisfalServiceRole => App.Configurations.GetConfig<DepositConfig>().TirisfalServiceRole;

        public static string WithdrawNotificationQueue => App.Configurations.GetConfig<DepositConfig>().WithdrawNotificationQueue;
        private static ConnectionMultiplexer RedisConnectionMultiplexer => LazyConnection.Value;

        public static IDatabase GetBizRedisClient()
        {
            return RedisConnectionMultiplexer.GetDatabase(2, new object());
        }

        /// <summary>
        /// Gets the configuration options.
        /// </summary>
        /// <returns>ConfigurationOptions.</returns>
        private static ConfigurationOptions GetConfigurationOptions()
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(App.Configurations.GetConfig<DepositConfig>().BizRedisConnectiongString, true);
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

        private static CloudTableClient InitTableClient()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(StorageConnectionString);
            CloudTableClient tableClient = account.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(2.Seconds(), 6);
            tableClient.DefaultRequestOptions.MaximumExecutionTime = 2.Minutes();
            tableClient.DefaultRequestOptions.ServerTimeout = 2.Minutes();
            return tableClient;
        }
    }
}
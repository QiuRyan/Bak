using System;
using System.IO;
using System.Text;
using Exceptionless;
using Exceptionless.Dependency;
using Exceptionless.Json;
using Exceptionless.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using StackExchange.Redis;
using ExponentialRetry = Microsoft.WindowsAzure.Storage.RetryPolicies.ExponentialRetry;

namespace jinyinmao.Signature.lib
{
    public static class ConfigManager
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static ConfigManager()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.json");
            appConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(path, Encoding.Default));
            ConfigurationOptions options = GetConfigurationOptions();
            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
        }

        /// <summary>
        ///     Gets the cloud table client.
        /// </summary>
        /// <value>The cloud table client.</value>
        public static CloudTableClient AzureCloudTableClient
        {
            get
            {
                CloudTableClient cloudTableClient = CloudStorageAccount.CreateCloudTableClient();
                cloudTableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 6);
                cloudTableClient.DefaultRequestOptions.MaximumExecutionTime = TimeSpan.FromMinutes(5);
                cloudTableClient.DefaultRequestOptions.ServerTimeout = TimeSpan.FromMinutes(5);

                return cloudTableClient;
            }
        }

        public static string BizDBConnectionString => appConfig.BizDBConnectionString;

        /// <summary>
        ///     个人CA证书存储表名
        /// </summary>
        /// <value>The customer ca cretificate table.</value>
        public static string CustomerCACretificateTable => appConfig.CustomerCACretificateTable;

        /// <summary>
        ///     跑批时间间隔(h为单位)
        /// </summary>
        /// <value>The do work interval.</value>
        public static int DoWorkInterval => appConfig.DoWorkInterval;

        /// <summary>
        ///     Gets the exceptionless API key.
        /// </summary>
        /// <value>The exceptionless API key.</value>
        private static string ExceptionlessApiKey => appConfig.ExceptionlessApiKey;

        /// <summary>
        ///     Gets the exceptionless server URL.
        /// </summary>
        /// <value>The exceptionless server URL.</value>
        private static string ExceptionlessServerUrl => appConfig.ExceptionlessServerUrl;

        /// <summary>
        ///     Gets the FDD application identifier.
        /// </summary>
        /// <value>The FDD application identifier.</value>
        public static string FDDAppID => appConfig.FDDAppID;

        /// <summary>
        ///     Gets the FDD application secrect.
        /// </summary>
        /// <value>The FDD application secrect.</value>
        public static string FDDAppSecrect => appConfig.FDDAppSecrect;

        /// <summary>
        ///     Gets or sets the FDD base address.
        /// </summary>
        /// <value>The FDD base address.</value>
        public static string FDDBaseAddress => appConfig.FDDBaseAddress;

        /// <summary>
        ///     Gets the FDD client identifier.
        /// </summary>
        /// <value>The FDD client identifier.</value>
        public static string FDDCompanyID => appConfig.FDDCompanyID;

        /// <summary>
        ///     Gets the FDD client role.
        /// </summary>
        /// <value>The FDD client role.</value>
        public static string FDDCompanyRole => appConfig.FDDCompanyRole;

        /// <summary>
        ///     Gets the FDD company sign key.
        /// </summary>
        /// <value>The FDD company sign key.</value>
        public static string FDDCompanySignKey => appConfig.FDDCompanySignKey;

        public static string FDDCustomerRole => appConfig.FDDCustomerRole;

        /// <summary>
        ///     Gets the FDD customer sign key.
        /// </summary>
        /// <value>The FDD customer sign key.</value>
        public static string FDDCustomerSignKey => appConfig.FDDCustomerSignKey;

        /// <summary>
        ///     Gets the FDD application secrect.
        /// </summary>
        /// <value>The FDD application secrect.</value>
        public static float FDDFontSize => appConfig.FDDFontSize;

        /// <summary>
        ///     Gets the FDD application secrect.
        /// </summary>
        /// <value>The FDD application secrect.</value>
        public static int FDDFontType => appConfig.FDDFontType;

        /// <summary>
        ///     是否清理生成的PDF文件
        /// </summary>
        /// <value><c>true</c> if this instance is clear file; otherwise, <c>false</c>.</value>
        public static bool IsClearFile => appConfig.IsClearFile;

        /// <summary>
        ///     Gets the exceptionless default client.
        /// </summary>
        /// <value>The exceptionless default client.</value>
        public static ExceptionlessClient JymLogger
        {
            get
            {
                ExceptionlessClient.Default.Configuration.ServerUrl = ExceptionlessServerUrl;
                ExceptionlessClient.Default.Configuration.ApiKey = ExceptionlessApiKey;
                ExceptionlessClient.Default.Configuration.SubmissionBatchSize = 100;
                ExceptionlessClient.Default.Configuration.Resolver.Register<IObjectStorage>(new InMemoryObjectStorage(100000));
                ExceptionlessClient.Default.Configuration.UseSessions(false);

                return ExceptionlessClient.Default;
            }
        }

        /// <summary>
        ///     定期查询可生成电子签章的sql语句
        /// </summary>
        /// <value>The regular query string.</value>
        public static string RegularQueryString => appConfig.RegularQueryString;

        public static string TirisfalServiceBaseUrl => appConfig.TirisfalServiceBaseUrl;

        /// <summary>
        ///     余额猫查询可生成电子签章的SQL语句
        /// </summary>
        /// <value>The yem query string.</value>
        public static string YEMQueryString => appConfig.YEMQueryString;

        private static AppConfig appConfig { get; }

        private static CloudStorageAccount CloudStorageAccount => CloudStorageAccount.Parse(appConfig.DataConnectionString);

        private static ConnectionMultiplexer RedisConnectionMultiplexer => LazyConnection.Value;

        public static IDatabase GetBizRedisClient()
        {
            return RedisConnectionMultiplexer.GetDatabase(0, new object());
        }

        /// <summary>
        ///     Gets the configuration options.
        /// </summary>
        /// <returns>ConfigurationOptions.</returns>
        private static ConfigurationOptions GetConfigurationOptions()
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(appConfig.BizRedisConnectiongString, true);
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
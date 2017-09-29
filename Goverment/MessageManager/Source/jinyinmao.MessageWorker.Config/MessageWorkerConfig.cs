using System.Collections.Generic;
using MoeLib.Jinyinmao.Configs;

namespace jinyinmao.MessageWorker.Config
{
    public class MessageWorkerConfig : IConfig
    {
        public string ClSendMessageUrl { get; set; }

        public string ClSmsServicePassword { get; set; }

        public string ClSmsServiceUserName { get; set; }

        public string MessageQueueName { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public bool SmsEnable { get; set; }

        public string SmsMessageDbContext { get; set; }

        public string StorageConnectionString { get; set; }

        public string ZtSendMessageUrl { get; set; }

        public string ZtSmsServicePassword { get; set; }

        public string ZtSmsServiceUserName { get; set; }

        #region IConfig Members

        /// <summary>
        ///     Gets the configurations.
        /// </summary>
        /// <value>
        ///     The configurations.
        /// </value>
        public string Configurations { get; set; }

        /// <summary>
        ///     Gets the configuration version.
        /// </summary>
        /// <value>
        ///     The configuration version.
        /// </value>
        public string ConfigurationVersion { get; set; }

        /// <summary>
        ///     Gets the permissions.
        /// </summary>
        /// <value>
        ///     The permissions.
        /// </value>
        public Dictionary<string, KeyValuePair<string, string>> Permissions { get; set; }

        /// <summary>
        ///     Gets the resources.
        /// </summary>
        /// <value>
        ///     The resources.
        /// </value>
        public Dictionary<string, string> Resources { get; set; }

        #endregion IConfig Members
    }
}
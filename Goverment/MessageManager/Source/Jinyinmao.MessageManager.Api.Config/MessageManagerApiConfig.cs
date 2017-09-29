using System.Collections.Generic;
using MoeLib.Jinyinmao.Configs;

namespace Jinyinmao.MessageManager.Api.Config
{
    public class MessageManagerApiConfig : IConfig
    {
        public string BearerAuthKeys { get; set; }

        public string GovernmentServerPublicKey { get; set; }

        public string MessageManagerDbContext { get; set; }

        public string MessageQueueName { get; set; }

        public string ServiceBusConnectionString { get; set; }

        #region IConfig Members

        public string Configurations { get; set; }

        public string ConfigurationVersion { get; set; }

        public Dictionary<string, KeyValuePair<string, string>> Permissions { get; set; }

        public Dictionary<string, string> Resources { get; set; }

        #endregion IConfig Members
    }
}
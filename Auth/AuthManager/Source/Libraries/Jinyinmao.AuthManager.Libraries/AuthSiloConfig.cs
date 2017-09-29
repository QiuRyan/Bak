using MoeLib.Jinyinmao.Configs;
using System.Collections.Generic;

namespace Jinyinmao.AuthManager.Libraries
{
    public class AuthSiloConfig : IConfig
    {
        public string DataConnectionString { get; set; }

        public string JYMAuthDBContextConnectionString { get; set; }

        public string ServiceBusConnectionString { get; set; }

        #region DocumentDb

        public string EndpointUrl { get; set; }

        public string AuthorizationKey { get; set; }

        public string DocumentDatabase { get; set; }

        #endregion

        #region IConfig Members

        public List<string> IPWhitelists { get; set; }

        public Dictionary<string, string> Resources { get; set; }

        #endregion IConfig Members
    }
}
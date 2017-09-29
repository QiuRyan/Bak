using MoeLib.Jinyinmao.Configs;
using System.Collections.Generic;

namespace Jinyinmao.AuthManager.Libraries
{
    public class AuthApiConfig : IConfig
    {
        public string AdminIps { get; set; }
        public int AppSignInExpirationSeconds { get; set; }

        public string BearerAuthKeys { get; set; }

        public string CouponUrl { get; set; }
        public string DataConnectionString { get; set; }

        public string GovernmentServerPublicKey { get; set; }

        public string JYMAuthDBContextConnectionString { get; set; }

        public string JYMRedisConnectionString { get; set; }

        public string JYMTirisfalServiceRole { get; set; }

        public string MessageManagerRole { get; set; }

        public int MobileSignInExpirationSeconds { get; set; }

        public int PCSignInExpirationSeconds { get; set; }
        public string SiloDeploymentId { get; set; }

        public string VeriCodeServiceRole { get; set; }
        public string WeChatAppId { get; set; }

        public string WeChatAppSecret { get; set; }

        public int WeiChatSignInExpirationSeconds { get; set; }


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
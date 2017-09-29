using System.Collections.Generic;
using MoeLib.Jinyinmao.Configs;

namespace Jinyinmao.ValidateCode.Api.Config
{
    public class ValidateCodeApiConfig : IConfig
    {
        public string BearerAuthKeys { get; set; }

        public int DefaultMaxSendTimes { get; set; }

        public string GovernmentServerPublicKey { get; set; }

        public string InnerMessageSendUrl { get; set; }

        public int MaxSendTimeForQuickLogin { get; set; }

        public string MessageManagerDbContext { get; set; }

        public int VeriCodeExpiryMinites { get; set; }

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
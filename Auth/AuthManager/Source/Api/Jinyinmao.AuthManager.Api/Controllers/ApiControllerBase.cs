using Jinyinmao.AuthManager.Libraries;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using Moe.Lib.Web;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;
using MoeLib.Jinyinmao.Web.Auth;
using MoeLib.Jinyinmao.Web.Diagnostics;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace Jinyinmao.AuthManager.Api.Controllers
{
    /// <summary>
    ///     Class ApiControllerBase.
    /// </summary>
    public abstract class ApiControllerBase : JinyinmaoApiController
    {
        /// <summary>
        ///     The cellphone regex
        /// </summary>
        public static readonly Regex CellphoneRegex = new Regex("^(13|14|15|16|17|18)\\d{9}$");

        /// <summary>
        ///     The jym access token protector
        /// </summary>
        private static readonly Lazy<JYMAccessTokenProtector> jymAccessTokenProtector = new Lazy<JYMAccessTokenProtector>(() => InitJYMAccessTokenProtector());

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiControllerBase" /> class.
        /// </summary>
        protected ApiControllerBase()
        {
            if (AzureClient.IsInitialized || GrainClient.IsInitialized)
            {
                return;
            }
#if CLOUD
            string configFilePath = HttpContext.Current.Server.MapPath(@"~/AzureConfiguration.xml");
            ClientConfiguration clientConfiguration = ClientConfiguration.LoadFromFile(configFilePath);
            clientConfiguration.DeploymentId = App.Configurations.GetConfig<AuthApiConfig>().SiloDeploymentId;
            clientConfiguration.DataConnectionString = App.Configurations.GetConfig<AuthApiConfig>().DataConnectionString;
            AzureClient.Initialize(clientConfiguration);
#else
            string configFilePath = HttpContext.Current.Server.MapPath(@"~/LocalConfiguration.xml");
            ClientConfiguration clientConfiguration = ClientConfiguration.LoadFromFile(configFilePath);
            GrainClient.Initialize(clientConfiguration);
#endif
        }

        private JYMAccessTokenProtector AccessTokenProtector
        {
            get { return jymAccessTokenProtector.Value; }
        }

        /// <summary>
        ///     Builds the authentication token.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <returns>System.String.</returns>
        protected string BuildAuthToken(string userIdentifier, string schemeName)
        {
            this.BuildPrincipal(userIdentifier, schemeName);
            ClaimsIdentity identity = this.User.Identity as ClaimsIdentity;
            return identity != null ? this.AccessTokenProtector.Protect(identity) : null;
        }

        /// <summary>
        ///     Builds the principal.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="schemeName">Name of the scheme.</param>
        protected void BuildPrincipal(string userIdentifier, string schemeName)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userIdentifier),
                new Claim(ClaimTypes.Expiration, this.GetExpiryTimestamp().ToString())
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, schemeName);
            this.User = new ClaimsPrincipal(identity);
        }

        /// <summary>
        ///     Gets the expiry timestamp.
        /// </summary>
        /// <returns>System.Int64.</returns>
        protected long GetExpiryTimestamp()
        {
            int duration = App.Configurations.GetConfig<AuthApiConfig>().PCSignInExpirationSeconds;
            TraceEntry traceEntry = this.Request.GetTraceEntry();
            if (traceEntry.ClientId.Contains("901") || traceEntry.ClientId.Contains("902"))
            {
                duration = App.Configurations.GetConfig<AuthApiConfig>().AppSignInExpirationSeconds;
            }
            else if (HttpUtils.IsFromMobileDevice(this.Request))
            {
                duration = App.Configurations.GetConfig<AuthApiConfig>().MobileSignInExpirationSeconds;
            }
            return DateTime.UtcNow.Add(duration.Seconds()).UnixTimestamp();
        }

        /// <summary>
        ///     Creates an <see cref="T:System.Web.Http.IHttpActionResult" /> (200 OK).
        /// </summary>
        /// <returns>An <see cref="T:System.Web.Http.IHttpActionResult" /> (200 OK).</returns>
        protected new IHttpActionResult Ok()
        {
            return base.Ok(new object());
        }

        private static JYMAccessTokenProtector InitJYMAccessTokenProtector()
        {
            return new JYMAccessTokenProtector(App.Configurations.GetConfig<AuthApiConfig>().BearerAuthKeys.HtmlDecode());
        }
    }
}
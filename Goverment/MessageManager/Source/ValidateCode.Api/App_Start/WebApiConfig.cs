// ***********************************************************************
// Project          : MessageManager
// File             : WebApiConfig.cs
// Created          : 2015-12-08  12:45
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-08  12:50
// ***********************************************************************
// <copyright file="WebApiConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using Jinyinmao.ValidateCode.Api.App_Start;
using Jinyinmao.ValidateCode.Api.Config;
using Moe.Lib.Jinyinmao;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Azure;

namespace Jinyinmao.ValidateCode.Api
{
    /// <summary>
    ///     WebApiConfig.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        ///     Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            App.Initialize().ConfigForAzure().UseGovernmentServerConfigManager<ValidateCodeApiConfig>();

            string bearerAuthKeys = App.Condigurations.GetConfig<ValidateCodeApiConfig>().BearerAuthKeys;
            string governmentServerPublicKey = App.Condigurations.GetConfig<ValidateCodeApiConfig>().GovernmentServerPublicKey;

            // Web API configuration and services
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.UseJinyinmaoExceptionHandler();

            config.UseJinyinmaoTraceEntryHandler();
            config.UseJinyinmaoAuthorizationHandler(bearerAuthKeys, governmentServerPublicKey);
            config.UseJinyinmaoLogHandler();

            config.UseJinyinmaoExceptionLogger();
            config.UseJinyinmaoTraceWriter();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.SetCorsPolicyProviderFactory(new CorsPolicyFactory());
            config.EnableCors();

            NinjectConfig.RegisterDependencyResolver(config);
        }

        #region Nested type: CorsPolicyFactory

        /// <summary>
        ///     CorsPolicyFactory.
        /// </summary>
        public class CorsPolicyFactory : ICorsPolicyProviderFactory
        {
            private readonly ICorsPolicyProvider provider = new CorsPolicyProvider();

            #region ICorsPolicyProviderFactory Members

            /// <summary>
            ///     Gets the cors policy provider.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <returns>ICorsPolicyProvider.</returns>
            public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
            {
                return this.provider;
            }

            #endregion ICorsPolicyProviderFactory Members
        }

        #endregion Nested type: CorsPolicyFactory

        #region Nested type: CorsPolicyProvider

        /// <summary>
        ///     CorsPolicyProvider.
        /// </summary>
        public class CorsPolicyProvider : ICorsPolicyProvider
        {
            private readonly CorsPolicy policy;

            /// <summary>
            ///     Initializes a new instance of the <see cref="CorsPolicyProvider" /> class.
            /// </summary>
            public CorsPolicyProvider()
            {
                // Create a CORS policy.
                this.policy = new CorsPolicy
                {
                    AllowAnyHeader = true,
                    AllowAnyMethod = true,
                    AllowAnyOrigin = true,
                    PreflightMaxAge = 300L
                };

                this.policy.ExposedHeaders.Add("Date");
            }

            #region ICorsPolicyProvider Members

            /// <summary>
            ///     Gets the <see cref="T:System.Web.Cors.CorsPolicy" />.
            /// </summary>
            /// <returns>
            ///     The <see cref="T:System.Web.Cors.CorsPolicy" />.
            /// </returns>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(this.policy);
            }

            #endregion ICorsPolicyProvider Members
        }

        #endregion Nested type: CorsPolicyProvider
    }
}
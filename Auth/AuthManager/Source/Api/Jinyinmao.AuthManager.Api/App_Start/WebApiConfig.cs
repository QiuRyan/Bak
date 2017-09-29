// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : WebApiConfig.cs
// Created          : 2016-03-22  6:03 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-05-04  2:37 PM
// ***********************************************************************
// <copyright file="WebApiConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Libraries;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using WebApiContrib.Formatting.Jsonp;

namespace Jinyinmao.AuthManager.Api
{
    /// <summary>
    ///     Class WebApiConfig.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        ///     Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            App.Initialize().ConfigForAzure().UseGovernmentServerConfigManager<AuthApiConfig>();

            string bearerAuthKeys = App.Configurations.GetConfig<AuthApiConfig>().BearerAuthKeys.HtmlDecode();
            string governmentServerPublicKey = App.Configurations.GetConfig<AuthApiConfig>().GovernmentServerPublicKey.HtmlDecode();

            // Web API configuration and services
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.UseJinyinmaoExceptionHandler();

            config.UseJinyinmaoJsonResponseWapperHandler();
            config.UseJinyinmaoTraceEntryHandler();
            config.UseJinyinmaoAuthorizationHandler(bearerAuthKeys, governmentServerPublicKey);
            config.UseJinyinmaoLogHandler();

            config.UseJinyinmaoExceptionLogger();
            config.UseJinyinmaoTraceWriter();
            config.UseOrderedFilter();

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.MessageHandlers.Add(new JinyinmaoAuthorizationHandler("<RSAKeyValue><Modulus>3o+HBruoycocmkwreug2DJLWzGl7EB0X7xTVvUOauVeX8O5t47jbllacZuv691W85pGO8ng6hQDiBvXtUz2uGJ5k8V6x2xbiK1qtMMP7QMxfLcg5zCB6i4RXFfqA5PXEtJt8S9mOk92rws1BoF3cSA9f7rNyqWToMYD+oGlATuxv5+PRr24HBm9w5eSxb24HvJbThMnUg0leLr77VY1LEOkUNn5TIQ38Y0Wo9gkwrCMLPXLLZqEU46sqAykhrvDOWtTczrBeRkqUICflmSK0OD0H6O958PwPzHW2h6mpFle4NQpP+QDmNbmK4zv7j71veelcuKV0WUcbXe/hfYSXzw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateFormatString = "G",
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Include
                }
            };

            config.Formatters.Clear();
            config.Formatters.Add(formatter);
            config.AddJsonpFormatter(formatter);
            config.Formatters.Add(new XmlMediaTypeFormatter());
            config.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());
            config.Formatters.Add(new JQueryMvcFormUrlEncodedFormatter());
            config.Formatters.Add(new BsonMediaTypeFormatter());

            config.Routes.MapHttpBatchRoute("WebApiBatch", "$batch", new BatchHandler(GlobalConfiguration.DefaultServer));
            config.SetCorsPolicyProviderFactory(new CorsPolicyFactory());
            config.EnableCors();

            NinjectConfig.RegisterDependencyResolver(config);
        }
    }

    /// <summary>
    ///     CorsPolicyFactory.
    /// </summary>
    public class CorsPolicyFactory : ICorsPolicyProviderFactory
    {
        /// <summary>
        ///     The provider
        /// </summary>
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

    /// <summary>
    ///     CorsPolicyProvider.
    /// </summary>
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        /// <summary>
        ///     The policy
        /// </summary>
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
            this.policy.ExposedHeaders.Add("X-JYM-AUTH");
            this.policy.ExposedHeaders.Add("x-jym-auth");
        }

        #region ICorsPolicyProvider Members

        /// <summary>
        ///     Gets the <see cref="T:System.Web.Cors.CorsPolicy" />.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="T:System.Web.Cors.CorsPolicy" />.</returns>
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.policy);
        }

        #endregion ICorsPolicyProvider Members
    }
}
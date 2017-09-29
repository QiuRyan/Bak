// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : IpAuthorizeAttribute.cs
// Created          : 2016-08-22  3:54 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-22  3:57 PM
// ***********************************************************************
// <copyright file="IpAuthorizeAttribute.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Libraries;
using Moe.Lib.Jinyinmao;
using Moe.Lib.Web;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Jinyinmao.AuthManager.Api.Filters
{
    /// <summary>
    ///     IpAuthorizeAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class IpAuthorizeAttribute : OrderedAuthorizationFilterAttribute
    {
        /// <summary>
        ///     Gets or sets a value indicating whether [only local host].
        /// </summary>
        /// <value><c>true</c> if [only local host]; otherwise, <c>false</c>.</value>
        public bool OnlyLocalHost { get; set; }

        [SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Local")]
        private string[] AdminIps { get; } = (App.Configurations.GetConfig<AuthApiConfig>().AdminIps ?? "").Split(',');

        [SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Local")]
        private string[] AllowedIps { get; } = { "101.95.30.142", "211.152.53.50", "10.1.10.37", "10.1.25.42" };

        /// <summary>
        ///     Calls when a process requests authorization.
        /// </summary>
        /// <param name="actionContext">The action context, which encapsulates information for using <see cref="T:System.Web.Http.Filters.AuthorizationFilterAttribute" />.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!this.IpIsAuthorized(actionContext))
            {
                this.HandleUnauthorizedRequest(actionContext);
            }

            base.OnAuthorization(actionContext);
        }

        /// <summary>
        ///     Processes requests that fail authorization. This default implementation creates a new response with the
        ///     Unauthorized status code. Override this method to provide your own handling for unauthorized requests.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        private void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext), @"actionContext can not be null");
            }

            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "");
        }

        private bool IpIsAuthorized(HttpActionContext context)
        {
            HttpRequestMessage request = context.Request;
            string ip = HttpUtils.GetUserHostAddress(request);

            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }

            if (this.OnlyLocalHost)
            {
                return request.IsLocal() || ip == "::1" || this.AdminIps.Contains(ip);
            }

            return this.AllowedIps.Contains(ip) || request.IsLocal() || ip == "::1" || this.AdminIps.Contains(ip);
        }
    }
}
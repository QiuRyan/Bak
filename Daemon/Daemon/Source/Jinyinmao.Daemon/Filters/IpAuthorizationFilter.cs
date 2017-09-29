// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : IpAuthorizationFilter.cs
// Created          : 2016-10-13  16:30
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  16:55
// ******************************************************************************************************
// <copyright file="IpAuthorizationFilter.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Linq;
using Hangfire.Dashboard;
using Microsoft.Azure;

namespace Jinyinmao.Daemon.Filters
{
    /// <summary>
    ///     IpAuthorizationFilter.
    /// </summary>
    public class IpAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string[] AllowedIps = CloudConfigurationManager.GetSetting("AllowedIps").Split(',');

        #region IDashboardAuthorizationFilter Members

        public bool Authorize(DashboardContext context)
        {
            return this.IpIsAuthorized(context);
        }

        #endregion IDashboardAuthorizationFilter Members

        private bool IpIsAuthorized(DashboardContext context)
        {
            string ip = context.Request.RemoteIpAddress;

            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }

            if (ip == "127.0.0.1" || ip == "::1" || this.AllowedIps.Contains(ip))
                return true;

            return ip == context.Request.LocalIpAddress;
        }
    }
}
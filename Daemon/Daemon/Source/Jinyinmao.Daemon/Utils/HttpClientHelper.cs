// ***********************************************************************
// Project          : Jinyinmao.Daemon
// File             : HttpClientHelper.cs
// Created          : 2016-07-06  10:40 AM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-07-27  2:47 PM
// ***********************************************************************
// <copyright file="HttpClientHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.Azure;
using Moe.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Jinyinmao.Daemon.Utils
{
    /// <summary>
    ///     HttpClientHelper.
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        ///     The API host
        /// </summary>
        private static readonly string ApiHost;

        private static readonly List<string> ApiInstanseIps;

        /// <summary>
        ///     Initializes static members of the <see cref="HttpClientHelper" /> class.
        /// </summary>
        static HttpClientHelper()
        {
            string configApiHost = CloudConfigurationManager.GetSetting("ApiHost");
            ApiHost = string.IsNullOrEmpty(configApiHost) ? "http://jym-dev-api.jinyinmao.com.cn/" : configApiHost;

            string apiInstanseIpStr = CloudConfigurationManager.GetSetting("ApiInstanseIps").IsNotNullOrEmpty() ? CloudConfigurationManager.GetSetting("ApiInstanseIps") : "jym-dev-api.jinyinmao.com.cn";
            ApiInstanseIps = apiInstanseIpStr.Split(',').ToList();
        }

        /// <summary>
        ///     Initializes the HTTP client.
        /// </summary>
        /// <returns>HttpClient.</returns>
        public static HttpClient InitHttpClient()
        {
            HttpClient httpClient = InitHttpClient(ApiHost);
            return httpClient;
        }

        /// <summary>
        ///     Initializes the HTTP client.
        /// </summary>
        /// <param name="apihost">The apihost.</param>
        /// <returns>HttpClient.</returns>
        public static HttpClient InitHttpClient(string apihost)
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(apihost) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            return httpClient;
        }

        /// <summary>
        ///     Initializes the HTTP client with ip list.
        /// </summary>
        /// <returns>IEnumerable&lt;HttpClient&gt;.</returns>
        public static IEnumerable<HttpClient> InitHttpClientWithIpList()
        {
            List<HttpClient> clients = ApiInstanseIps.Select(ip => InitHttpClient("http://{0}/".FormatWith(ip))).ToList();
            return clients;
        }
    }
}
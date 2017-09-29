// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : CheckJBYProductSaleStatusService.cs
// Created          : 2016-10-13  16:30
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  16:47
// ******************************************************************************************************
// <copyright file="CheckJBYProductSaleStatusService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jinyinmao.Daemon.Utils;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;

namespace Jinyinmao.Daemon.Services
{
    /// <summary>
    ///     Class PollCheckJBYProductSaleStatusService.
    /// </summary>
    public class CheckJBYProductSaleStatusService
    {
        private static readonly string RequestUri;
        private static readonly RetryPolicy RetryPolicy;
        private readonly Lazy<HttpClient> client;

        static CheckJBYProductSaleStatusService()
        {
            RequestUri = "/CheckJBYProductSaleStatus";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckJBYProductSaleStatusService" /> class.
        /// </summary>
        public CheckJBYProductSaleStatusService()
        {
            this.client = new Lazy<HttpClient>(HttpClientHelper.InitHttpClient);
        }

        /// <summary>
        ///     Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public HttpClient Client => this.client.Value;

        /// <summary>
        ///     Works this instance.
        /// </summary>
        public void Work()
        {
            try
            {
                this.CallCheckJBYProductSaleStatusAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollCheckJBYProductSaleStatusException {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CallCheckJBYProductSaleStatusAsync()
        {
            HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(RequestUri, new ByteArrayContent(Encoding.UTF8.GetBytes(""))));
            msg.EnsureSuccessStatusCode();
        }
    }
}
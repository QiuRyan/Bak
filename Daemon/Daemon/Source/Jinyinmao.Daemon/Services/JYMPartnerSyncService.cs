// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : JYMPartnerSyncService.cs
// Created          : 2017-06-19  8:37
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:06
// ******************************************************************************************************
// <copyright file="JYMPartnerSyncService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;

namespace Jinyinmao.Daemon.Services
{
    public class JYMPartnerSyncService
    {
        #region 属性

        private static readonly string CouponApiHost;

        /// <summary>
        ///     The retry policy
        /// </summary>
        private static readonly RetryPolicy RetryPolicy;

        /// <summary>
        ///     The client
        /// </summary>
        private readonly Lazy<HttpClient> client;

        static JYMPartnerSyncService()
        {
            CouponApiHost = CloudConfigurationManager.GetSetting("CouponApiHost");
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XianFengPaymentService" /> class.
        /// </summary>
        public JYMPartnerSyncService()
        {
            this.client = new Lazy<HttpClient>(this.InitHttpClient);
        }

        /// <summary>
        ///     The clientPaymentApiHost
        /// </summary>
        public HttpClient Client => this.client.Value;

        private HttpClient InitHttpClient()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(CouponApiHost) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            return httpClient;
        }

        #endregion 属性

        #region 工作函数

        /// <summary>
        ///     News the year cash back to user acount.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        public async Task PartnerSync()
        {
            HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.GetAsync("/api/Helper/PartnerSync"));
            msg.EnsureSuccessStatusCode();

            string result = await msg.Content.ReadAsStringAsync();
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            var log = new
            {
                Time = now,
                Data = result
            };

            LogHelper.Log($"合伙人信息同步到交易系统{DateTime.UtcNow.ToChinaStandardTime()}", log.ToJson(), "PartnerSync");
        }

        [DisplayName("合伙人信息同步到交易系统")]
        public void Work()
        {
            try
            {
                this.PartnerSync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.Error(e, $"合伙人信息同步到交易系统{DateTime.UtcNow.ToChinaStandardTime()}", e.ToJson(), "PartnerSync");
            }
        }

        #endregion 工作函数
    }
}
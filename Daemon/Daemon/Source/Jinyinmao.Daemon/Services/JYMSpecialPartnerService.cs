// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : JYMSpecialPartnerService.cs
// Created          : 2017-08-05  14:15
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:13
// ******************************************************************************************************
// <copyright file="JYMSpecialPartnerService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
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
    public class JYMSpecialPartnerService
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

        static JYMSpecialPartnerService()
        {
            CouponApiHost = CloudConfigurationManager.GetSetting("CouponApiHost");
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XianFengPaymentService" /> class.
        /// </summary>
        public JYMSpecialPartnerService()
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
        public async Task SetPartnerLevel()
        {
            HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.GetAsync("/api/Helper/SetSpecialPartnerLevel"));
            msg.EnsureSuccessStatusCode();

            string result = await msg.Content.ReadAsStringAsync();
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            var log = new
            {
                Time = now,
                Data = result
            };

            LogHelper.Log($"特殊合伙人每日等级计算{DateTime.UtcNow.ToChinaStandardTime()}", log.ToJson(), "SetSpecialPartnerLevel");
        }

        [DisplayName("特殊合伙人每日等级计算")]
        public void Work()
        {
            try
            {
                this.SetPartnerLevel().Wait();
            }
            catch (Exception e)
            {
                LogHelper.Error(e, $"特殊合伙人每日等级计算{DateTime.UtcNow.ToChinaStandardTime()}", e.ToJson(), "SetSpecialPartnerLevel");
            }
        }

        #endregion 工作函数
    }
}
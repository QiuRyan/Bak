// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : JYMPartnerService.cs
// Created          : 2017-06-19  8:37
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:13
// ******************************************************************************************************
// <copyright file="JYMPartnerService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
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
    public class JYMPartnerService
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

        static JYMPartnerService()
        {
            CouponApiHost = CloudConfigurationManager.GetSetting("CouponApiHost");
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JYMPartnerService" /> class.
        /// </summary>
        public JYMPartnerService()
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
            HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.GetAsync("/api/Helper/SetPartnerLevel"));
            msg.EnsureSuccessStatusCode();

            string result = await msg.Content.ReadAsStringAsync();
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            var log = new
            {
                Time = now,
                Data = result
            };

            LogHelper.Log($"普通合伙人每日等级计算{DateTime.UtcNow.ToChinaStandardTime()}", log.ToJson(), "SetPartnerLevel");
        }

        [DisplayName("普通合伙人每日等级计算")]
        public void Work()
        {
            try
            {
                this.SetPartnerLevel().Wait();
            }
            catch (Exception e)
            {
                LogHelper.Error(e, $"普通合伙人每日等级计算{DateTime.UtcNow.ToChinaStandardTime()}", e.ToJson(), "SetPartnerLevel");
            }
        }

        #endregion 工作函数
    }
}
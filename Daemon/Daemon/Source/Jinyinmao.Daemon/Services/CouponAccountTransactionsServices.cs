// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : CouponAccountTransactionsServices.cs
// Created          : 2016-10-13  16:30
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:06
// ******************************************************************************************************
// <copyright file="CouponAccountTransactionsServices.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
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
    public class CouponAccountTransactionsServices
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

        static CouponAccountTransactionsServices()
        {
            CouponApiHost = CloudConfigurationManager.GetSetting("CouponApiHost");
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// </summary>
        public CouponAccountTransactionsServices()
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
        public async Task UpdateAccountTransactionsAsync()
        {
            HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.GetAsync("/api/Helper/UpdateAccountTransactions"));
            msg.EnsureSuccessStatusCode();

            string result = await msg.Content.ReadAsStringAsync();
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            var log = new
            {
                Time = now,
                Data = result
            };

            LogHelper.Log("Daemon卡券、红包、邀请、打赏返利", log.ToJson(), "UpdateAccountTransactionsAsync");
        }

        [DisplayName("卡券、红包、邀请、打赏返利")]
        public void Work()
        {
            try
            {
                this.UpdateAccountTransactionsAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.Error(e, "Daemon卡券、红包、邀请、打赏返利异常", e.ToJson(), "UpdateAccountTransactionsAsync");
            }
        }

        #endregion 工作函数
    }
}
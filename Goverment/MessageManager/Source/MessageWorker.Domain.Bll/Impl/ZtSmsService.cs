// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ZTSmsService.cs
// Created          : 2015-09-10  11:08
//
// Last Modified By :
// Last Modified On : 2015-09-10  13:35
// ***********************************************************************
// <copyright file="ZTSmsService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using jinyinmao.MessageWorker.Config;
using Jinyinmao.Application.Constants;
using Jinyinmao.MessageWorker.Domain.Bll;
using Jinyinmao.MessageWorker.Domain.Bll.Impl;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using Moe.Lib.TransientFaultHandling;
using MoeLib.Diagnostics;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jinyinmao.Sms.Api.Services
{
    /// <summary>
    ///     Class ZTSmsService.
    /// </summary>
    internal class ZtSmsService : ISmsService
    {
        private static readonly string MessageTemplate = @"username={0}&password={1}&mobile={2}&content={3}【{4}】&dstime=&productid={5}&xh=";
        private static readonly string Password = ConfigsManager.ZtSmsServicePassword;
        private static readonly bool SmsEnable = ConfigsManager.SmsEnable;
        private static readonly string UserName = ConfigsManager.ZtSmsServiceUserName;
        private static readonly string ZtSendMessageUrl = ConfigsManager.ZtSendMessageUrl;
        private readonly RetryPolicy retryPolicy = new RetryPolicy(new HttpRequestTransientErrorDetectionStrategy(), RetryStrategy.DefaultExponential);
        private readonly SmsServiceBase smsServiceBase;

        public ZtSmsService(SmsChannel channel)
        {
            switch (channel)
            {
                case SmsChannel.YanZhengMa:
                    this.ProductId = "676767";
                    break;

                case SmsChannel.TongZhi:
                    this.ProductId = "48661";
                    break;

                default:
                    this.ProductId = "251503";
                    break;
            }
            this.smsServiceBase = new SmsServiceBase(this);
        }

        private ILogger Logger
        {
            get { return App.LogManager.CreateLogger(); }
        }

        #region ISmsService Members

        public string ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the SMS gateway.
        /// </summary>
        /// <value>The name of the SMS gateway.</value>
        public SmsGateway SmsGatewayName { get; set; } = SmsGateway.ZhuTong;

        public async Task SendMessageAsync(string args, string cellphone, string message, string signature)
        {
            try
            {
                string responseMessage = string.Empty;
                if (SmsEnable)
                {
                    string requestUri = this.BuildRequestUri(cellphone, message, signature);

                    using (HttpClient client = InitHttpClient())
                    {
#if DEBUG
                        HttpResponseMessage response = await client.GetAsync(requestUri);
#else
                        HttpResponseMessage response = await this.retryPolicy.ExecuteAsync(async () => await client.GetAsync(requestUri));
#endif
                        responseMessage = await response.Content.ReadAsStringAsync();
                    }
                }
                //Json转换
                await this.smsServiceBase.StoreSmsMessageAsync(args, cellphone, message, responseMessage, this.SmsGatewayName);
            }
            catch (Exception e)
            {
                this.LogError(cellphone, message, signature, e);
            }
        }

        #endregion ISmsService Members

        private static bool GetSendMessageResult(string responseMessage)
        {
            return !responseMessage.IsNullOrEmpty() && responseMessage.StartsWith("1,", StringComparison.Ordinal);
        }

        private static HttpClient InitHttpClient()
        {
            return new HttpClient { Timeout = 5.Minutes() };
        }

        private string BuildRequestUri(string cellphone, string message, string signature)
        {
            if (string.IsNullOrEmpty(signature))
            {
                signature = "金银猫";
            }
            return ZtSendMessageUrl + MessageTemplate.FormatWith(
                UserName, Password, cellphone, message, signature, this.ProductId);
        }

        private void LogError(string cellphone, string message, string signature, Exception exception)
        {
            string logMessage = $"Failed to send sms message. Cellphone:{cellphone}. Message:{message}.Signature:{signature}";
            this.Logger.Error(logMessage, "ZtSmsService", 0UL, "", null, exception);
        }
    }
}
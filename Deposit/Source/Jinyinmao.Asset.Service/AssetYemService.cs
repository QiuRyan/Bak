// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : AssetYemService.cs
// Created          : 2017-08-10  18:40
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-11  13:45
// ******************************************************************************************************
// <copyright file="AssetYemService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Jinyinmao.Asset.Service.Interface;
using Jinyinmao.Deposit.Config;
using Jinyinmao.Deposit.Domain;
using Jinyinmao.Deposit.Lib;
using Jinyinmao.Deposit.Lib.Enum;
using Jinyinmao.ServiceBus.Service;
using Moe.Lib;
using MoeLib.Jinyinmao.Web.Handlers;
using MoeLib.Jinyinmao.Web.Handlers.Client;

namespace Jinyinmao.Asset.Service
{
    public class AssetYemService : IAssetYemService
    {
        private readonly ServiceBusService serviceBusService = new ServiceBusService();

        /// <summary>
        ///     资产SF
        /// </summary>
        /// <value>The asset service fabric client.</value>
        private HttpClient AssetServiceFabricClient => this.InitAssetServiceFabricHttpClient();

        #region IAssetYemService Members

        /// <summary>
        ///     债权转让放款
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<BasicResult<CreditAssignmentGrantNotify>> DealYemCreditAssignmentGrantNotifyAsync(MessageBody<CreditAssignmentGrantNotify> message)
        {
            try
            {
                LogHelper.Info("发送余额猫债权转让放款确认数据到资产系统:" + message.Data.OrderId, message.ToJson(), message.Data.OrderId);

                HttpResponseMessage businessResponse = await this.AssetServiceFabricClient.PostAsJsonAsync("MessageFromBank/ConfirmAdvanceDebt", message.Data);

                businessResponse.EnsureSuccessStatusCode();

                BussinessResponse businessResult = await businessResponse.Content.ReadAsAsync<BussinessResponse>();

                if (businessResponse.StatusCode == HttpStatusCode.OK && businessResult.IsTrue)
                {
                    LogHelper.Info("发送余额猫债权转让放款确认数据到资产系统成功:" + message.Data.OrderId, businessResult.ToJson(), message.Data.OrderId);
                    return BasicResult<CreditAssignmentGrantNotify>.Successed(message.Data);
                }
                return BasicResult<CreditAssignmentGrantNotify>.Failed(message.Data, $"发送余额猫债权转让放款确认数据到资产系统失败:{(int)businessResponse.StatusCode},Response: {businessResult.ToJson()}");
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<CreditAssignmentGrantNotify>.Failed(message.Data, $"正在重试第{message.RetryCount}次,发送余额猫债权转让放款确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<CreditAssignmentGrantNotify>.Failed(message.Data, $"重试{message.RetryCount}次失败,发送余额猫债权转让放款确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (HttpResponseException ex)
            {
                LogHelper.Error(ex, "发送余额猫债权转让放款确认数据到资产系统异常: " + ex.Message, message.ToJson(), message.Data.OrderId);
                return BasicResult<CreditAssignmentGrantNotify>.Failed(message.Data, "发送余额猫债权转让放款确认数据到资产系统异常: " + ex.Message);
            }
        }

        #endregion

        private HttpClient InitAssetServiceFabricHttpClient()
        {
            List<DelegatingHandler> delegatingHandlers = new List<DelegatingHandler>
            {
                new JinyinmaoHttpStatusHandler(),
                new JinyinmaoLogHandler("HTTP Client Request", "HTTP Client Response"),
                new DepositHttpRetryHandler()
            };

            HttpClient client = HttpClientFactory.Create(new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            }, delegatingHandlers.ToArray());

            client.BaseAddress = new Uri(ConfigManager.AssetServicefabricBaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json", 1.0));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.5));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.1));
            client.DefaultRequestHeaders.AcceptEncoding.Clear();
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip", 1.0));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate", 0.5));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("*", 0.1));
            client.DefaultRequestHeaders.Connection.Add("keep-alive");
            client.Timeout = 3.Minutes();
            return client;
        }
    }
}
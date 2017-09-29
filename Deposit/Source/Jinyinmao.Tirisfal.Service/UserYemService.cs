// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : UserYemService.cs
// Created          : 2017-08-10  16:04
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  16:12
// ******************************************************************************************************
// <copyright file="UserYemService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Jinyinmao.Deposit.Config;
using Jinyinmao.Deposit.Domain;
using Jinyinmao.Deposit.Lib;
using Jinyinmao.Deposit.Lib.Enum;
using Jinyinmao.ServiceBus.Service;
using Jinyinmao.Tirisfal.Service.Interface;
using Jinyinmao.Tirisfal.Service.Interface.Dtos;
using Jinyinmao.Tirisfal.Service.Interface.Dtos.Infos;
using Moe.Lib;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;
using MoeLib.Jinyinmao.Web.Handlers;
using MoeLib.Jinyinmao.Web.Handlers.Client;

namespace Jinyinmao.Tirisfal.Service
{
    public class UserYemService : IUserYemService
    {
        private readonly ServiceBusService serviceBusService = new ServiceBusService();

        private readonly HttpClient tirisfalClient = JYMInternalHttpClientFactory.Create(ConfigManager.TirisfalServiceRole, (TraceEntry)null);

        /// <summary>
        ///     资产SF
        /// </summary>
        /// <value>The asset service fabric client.</value>
        private HttpClient AssetServiceFabricClient => this.InitAssetServiceFabricHttpClient();

        #region IUserYemService Members

        /// <summary>
        ///     预约批量投资
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<BasicResult<BatchBookInvestNotify>> DealYemBatchBookInvestNotifyAsync(MessageBody<BatchBookInvestNotify> message)
        {
            try
            {
                LogHelper.Info("发送余额猫预约批量投资确认数据到资产系统:" + message.Data.OrderId, message.ToJson(), message.Data.OrderId);

                HttpResponseMessage businessResult = await this.AssetServiceFabricClient.PostAsJsonAsync("MessageFromBank/ProcessBatchBookInvestConfirm", message.Data);

                BussinessResponse bussinessResponse = await businessResult.Content.ReadAsAsync<BussinessResponse>();

                if (businessResult.StatusCode == HttpStatusCode.OK && bussinessResponse.IsTrue)
                {
                    LogHelper.Info("发送余额猫预约批量投资确认数据到资产系统成功:" + message.Data.OrderId, bussinessResponse.ToJson(), message.Data.OrderId);
                    return BasicResult<BatchBookInvestNotify>.Successed(message.Data);
                }
                return BasicResult<BatchBookInvestNotify>.Failed(message.Data, $"发送余额猫预约批量投资确认数据到资产系统失败:{(int)businessResult.StatusCode},Response: { bussinessResponse.ToJson()}");
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BookNotificationQueue, message.ToJson());
                    return BasicResult<BatchBookInvestNotify>.Failed(message.Data, $"正在重试第{message.RetryCount}次,发送余额猫预约批量投资确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<BatchBookInvestNotify>.Failed(message.Data, $"重试{message.RetryCount}次失败,发送余额猫预约批量投资确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送余额猫预约批量投资确认数据到资产系统异常" + ex.Message, message.ToJson(), message.Data.OrderId);

                return BasicResult<BatchBookInvestNotify>.Failed(message.Data, "发送余额猫预约批量投资确认数据到资产系统异常" + ex.Message, true);
            }
        }

        /// <summary>
        ///     预约批量债权转让
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<BasicResult<BatchCreditAssignmentCreateNotify>> DealYemBookCreditCreateBatchNotifyAsync(MessageBody<BatchCreditAssignmentCreateNotify> message)
        {
            try
            {
                LogHelper.Info("发送余额猫预约批量债权转让确认数据到资产系统:" + message.Data.OrderId, message.ToJson(), message.Data.OrderId);

                HttpResponseMessage bussinessResponse = await this.AssetServiceFabricClient.PostAsJsonAsync("MessageFromBank/ConfirmDebt", message.Data);

                BussinessResponse bussinessResult = await bussinessResponse.Content.ReadAsAsync<BussinessResponse>();

                if (bussinessResponse.StatusCode == HttpStatusCode.OK && bussinessResult.IsTrue)
                {
                    LogHelper.Info("发送余额猫预约批量债权转让确认数据到资产系统成功:" + message.Data.OrderId, bussinessResult.ToJson(), message.Data.OrderId);
                    return BasicResult<BatchCreditAssignmentCreateNotify>.Successed(message.Data);
                }
                return BasicResult<BatchCreditAssignmentCreateNotify>.Failed(message.Data, $"发送余额猫预约批量债权转让确认数据到资产系统失败:{(int)bussinessResponse.StatusCode},Response:{ bussinessResult.Result}");
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BookNotificationQueue, message.ToJson());
                    return BasicResult<BatchCreditAssignmentCreateNotify>.Failed(message.Data, $"正在重试第{message.RetryCount}次,发送余额猫预约批量债权转让确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<BatchCreditAssignmentCreateNotify>.Failed(message.Data, $"重试{message.RetryCount}次失败,发送余额猫预约批量债权转让确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送余额猫预约批量债权转让确认数据到资产系统异常: " + ex.Message, message.ToJson(), message.Data.OrderId);

                return BasicResult<BatchCreditAssignmentCreateNotify>.Failed(message.Data, "发送余额猫预约批量债权转让确认数据到资产系统异常: " + ex.Message, true);
            }
        }

        /// <summary>
        ///     余额猫取消预约冻结
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;Tuple&lt;System.Boolean, System.String&gt;&gt;.</returns>
        public async Task<BasicResult<BookFreezeCancelNotify>> DealYemBookFreezeCancelNotifyAsync(MessageBody<BookFreezeCancelNotify> message)
        {
            try
            {
                LogHelper.Info("发送余额猫取消预约冻结确认数据到资产系统:" + message.Data.OrderId, message.ToJson(), message.Data.OrderId);

                HttpResponseMessage bussinessResponse = await this.AssetServiceFabricClient.PostAsJsonAsync("MessageFromBank/ConfirmBankFreezecancel", message.Data);

                bussinessResponse.EnsureSuccessStatusCode();

                BussinessResponse bussinessResult = await bussinessResponse.Content.ReadAsAsync<BussinessResponse>();

                if (bussinessResponse.StatusCode == HttpStatusCode.OK && bussinessResult.IsTrue)
                {
                    LogHelper.Info("发送余额猫取消预约冻结确认数据到资产系统成功:" + message.Data.OrderId, bussinessResult.ToJson(), message.Data.OrderId);
                    return BasicResult<BookFreezeCancelNotify>.Successed(message.Data);
                }
                return BasicResult<BookFreezeCancelNotify>.Failed(message.Data, $"发送余额猫取消预约冻结确认数据到资产系统失败,StatusCode:{(int)bussinessResponse.StatusCode},Response:{bussinessResult.ToJson()}");
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BookNotificationQueue, message.ToJson());
                    return BasicResult<BookFreezeCancelNotify>.Failed(message.Data, $"正在重试第{message.RetryCount}次,发送余额猫取消预约冻结确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<BookFreezeCancelNotify>.Failed(message.Data, $"重试{message.RetryCount}次失败,发送余额猫取消预约冻结确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送余额猫取消预约冻结确认数据到资产系统异常: " + ex.Message, message.ToJson(), message.Data.OrderId);
                return BasicResult<BookFreezeCancelNotify>.Failed(message.Data, "发送余额猫取消预约冻结确认数据到资产系统异常: " + ex.Message, true);
            }
        }

        /// <summary>
        ///     余额猫预约冻结
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<BasicResult<YemBookFrozenRequest>> DealYemBookFreezeNotifyAsync(MessageBody<BookFreezeNotify> message)
        {
            YemBookFrozenRequest request  = new YemBookFrozenRequest { OrderId = message.Data.OrderId, ResultCode = message.Data.Status.GetResultCode() }; ;
            try
            {
                string userIdentifier = await GerUserIdentifierFromOrder(message);

                if (string.IsNullOrEmpty(userIdentifier))
                {
                    return BasicResult<YemBookFrozenRequest>.Failed(request, "发送余额猫投资冻结确认数据到交易系统失败,UserId为空");
                }

                request.UserIdentifier = userIdentifier;

                LogHelper.Info("发送余额猫投资冻结确认数据到交易系统:" + message.Data.OrderId, request.ToJson(), message.Data.OrderId);

                HttpResponseMessage tirisfalResponse = await this.tirisfalClient.PostAsJsonAsync("YemInvesting/ConfirmYemBooking", request);

                tirisfalResponse.EnsureSuccessStatusCode();

                BaseResponse tirisfalResult = await tirisfalResponse.Content.ReadAsAsync<BaseResponse>();

                if (!tirisfalResult.Result)
                {
                    return BasicResult<YemBookFrozenRequest>.Failed(request, "发送余额猫投资冻结确认数据到交易系统出错:" + tirisfalResult.Remark);
                }
                return BasicResult<YemBookFrozenRequest>.Successed(request);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BookNotificationQueue, message.ToJson());
                    return BasicResult<YemBookFrozenRequest>.Failed(request, $"正在重试第{message.RetryCount}次,发送余额猫投资冻结确认数据到交易系统异常:{ex.Message}");
                }
                return BasicResult<YemBookFrozenRequest>.Failed(request, $"重试{message.RetryCount}次失败,发送余额猫投资冻结确认数据到交易系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送余额猫投资冻结确认数据到交易系统失败:" + ex.Message, message.ToJson(), message.Data.OrderId);

                return BasicResult<YemBookFrozenRequest>.Failed(request, "发送余额猫投资冻结确认数据到交易系统失败:" + ex.Message, true);
            }
        }

        #endregion

        private static async Task<string> GerUserIdentifierFromOrder(MessageBody<BookFreezeNotify> message)
        {
            string redisValue = RedisHelper.GetStringValue($"Gateway:YemOrder:{message.Data.OrderId}");

            string userIdentifier = string.Empty;
            if (redisValue.IsNotNullOrEmpty())
            {
                userIdentifier = redisValue.FromJson<UserInfo>()?.UserId.ToGuidString();
            }

            if (string.IsNullOrEmpty(userIdentifier))
            {
                using (BizContext db = new BizContext(ConfigManager.BizDBConnectionString))
                {
                    YemAssetRecord yemAssetRecord = await db.YemAssetRecord.FirstOrDefaultAsync(p => p.YemOrderIdentifier == message.Data.OrderId);
                    if (yemAssetRecord != null)
                    {
                        userIdentifier = yemAssetRecord.UserIdentifier;
                    }
                }
            }

            return userIdentifier;
        }

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
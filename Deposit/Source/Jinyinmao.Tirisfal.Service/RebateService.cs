// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : RebateService.cs
// Created          : 2017-08-10  15:28
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  15:43
// ******************************************************************************************************
// <copyright file="RebateService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
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
using Jinyinmao.ServiceBus.Service.Interface;
using Jinyinmao.Tirisfal.Service.Interface.Dtos;
using Moe.Lib;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;
using MoeLib.Jinyinmao.Web.Handlers;
using MoeLib.Jinyinmao.Web.Handlers.Client;

namespace Jinyinmao.Tirisfal.Service
{
    public static class RebateService
    {
        private static readonly HttpClient couponClient = JYMInternalHttpClientFactory.Create(ConfigManager.CouponServiceRole, (TraceEntry)null);
        private static readonly ServiceBusService serviceBusService = new ServiceBusService();

        private static readonly HttpClient tirisfalClient = JYMInternalHttpClientFactory.Create(ConfigManager.TirisfalServiceRole, (TraceEntry)null);

        /// <summary>
        ///     资产SF
        /// </summary>
        /// <value>The asset service fabric client.</value>
        private static HttpClient AssetServiceFabricClient
        {
            get
            {
                return InitAssetServiceFabricHttpClient();
            }
        }

        /// <summary>
        ///     发送返利确认到资产系统
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;BasicResult&lt;ConfirmRebateNotifyRequest&gt;&gt;.</returns>
        public static async Task<BasicResult<ConfirmRebateNotifyRequest>> SendRebateConfirmDataToAssetAsync(MessageBody<RebateNotify> message)
        {
            ConfirmRebateNotifyRequest request = new ConfirmRebateNotifyRequest { OrderId = message.Data.OrderId, ResultCode = message.Data.Status.GetResultCode() };

            try
            {
                LogHelper.Info($"发送返利确认数据到资产系统:{message.Data.OrderId}", message.Data.ToJson(), message.Data.OrderId);

                HttpResponseMessage bussinessResponse = await AssetServiceFabricClient.PostAsJsonAsync("MessageFromBank/ConfirmRebate", message.Data);

                bussinessResponse.EnsureSuccessStatusCode();

                BussinessResponse businessResult = await bussinessResponse.Content.ReadAsAsync<BussinessResponse>();

                if (!businessResult.IsTrue)
                {
                    return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, $"发送返利确认数据到资产系统失败:{businessResult.ToJson()}");
                }
                return BasicResult<ConfirmRebateNotifyRequest>.Successed(request);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, $"正在重试第{message.RetryCount}次,发送返利确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, $"重试{message.RetryCount}次失败,发送返利确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送返利确认数据到资产系统异常: " + ex.Message, message.ToJson(), message.Data.OrderId);
                return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, "发送返利确认数据到资产系统异常: " + ex.Message);
            }
        }

        /// <summary>
        ///     发送返利确认数据到卡券系统
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;Tuple&lt;System.Boolean, System.String&gt;&gt;.</returns>
        public static async Task<BasicResult<ConfirmRebateNotifyRequest>> SendRebateConfirmDataToCouponAsync(MessageBody<RebateNotify> message)
        {
            ConfirmRebateNotifyRequest request = new ConfirmRebateNotifyRequest { OrderId = message.Data.OrderId, ResultCode = message.Data.Status.GetResultCode() };

            try
            {
                LogHelper.Info($"发送返利确认数据到卡券系统:{message.Data.OrderId}", message.Data.ToJson(), message.Data.OrderId);
                HttpResponseMessage couponResult = await couponClient.GetAsync($"api/Coupon/Transaction/SetTransactionPending/{message.Data.OrderId}/{message.Data.Status.GetResultCode()}");

                couponResult.EnsureSuccessStatusCode();

                RebateTransaction rebateTransaction = await GetUserIdentifierFromRebateAsync(message.Data.OrderId, Constants.COUPONDB);

                LogHelper.Info($"发送返利流水到后台队列:{rebateTransaction.TransactionId}", rebateTransaction.ToJson(), rebateTransaction.TransactionId);

                await serviceBusService.SendRebateTransactionToEbibpbCenterAsync(ConfigManager.RebateTransactionQueue, rebateTransaction.ToJson());

                return BasicResult<ConfirmRebateNotifyRequest>.Successed(request);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, $"正在重试第{message.RetryCount}次,发送返利确认数据到卡券系统异常:{ex.Message}");
                }
                return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, $"重试{message.RetryCount}次失败,发送返利确认数据到卡券系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送返利确认数据到卡券系统异常: " + ex.Message, message.ToJson(), message.Data.OrderId);
                return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, "发送返利确认数据到卡券系统异常: " + ex.Message);
            }
        }

        /// <summary>
        ///     发送返利确认数据到交易系统
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;Tuple&lt;System.Boolean, System.String&gt;&gt;.</returns>
        public static async Task<BasicResult<ConfirmRebateNotifyRequest>> SendRebateConfirmDataToTirisfalAsync(MessageBody<RebateNotify> message)
        {
            ConfirmRebateNotifyRequest request = new ConfirmRebateNotifyRequest { OrderId = message.Data.OrderId, ResultCode = message.Data.Status.GetResultCode() };
            try
            {
                LogHelper.Info($"发送返利确认数据到交易系统:{message.Data.OrderId}", message.Data.ToJson(), message.Data.OrderId);

                RebateTransaction rebateTransaction = await GetUserIdentifierFromRebateAsync(message.Data.OrderId, Constants.BIZDB);

                if (rebateTransaction == null || rebateTransaction.UserId.IsNullOrEmpty())
                {
                    return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, "发送返利确认数据到交易系统失败,UersIdentifier为空");
                }
                request.UserIdentifier = rebateTransaction.UserId;

                HttpResponseMessage tirisfalResult = await tirisfalClient.PostAsJsonAsync("User/Settle/ConfirmRebate", request);

                tirisfalResult.EnsureSuccessStatusCode();

                BaseResponse result = await tirisfalResult.Content.ReadAsAsync<BaseResponse>();

                if (!result.Result)
                {
                    return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, "发送返利确认数据到交易系统失败: " + result.Remark);
                }

                LogHelper.Info($"发送返利流水到后台队列:{rebateTransaction.TransactionId}", rebateTransaction.ToJson(), rebateTransaction.TransactionId);
                await serviceBusService.SendRebateTransactionToEbibpbCenterAsync(ConfigManager.RebateTransactionQueue, rebateTransaction.ToJson());

                return BasicResult<ConfirmRebateNotifyRequest>.Successed(request);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, $"正在重试第{message.RetryCount}次,发送返利确认数据到交易系统异常:{ex.Message}");
                }
                return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, $"重试{message.RetryCount}次失败,发送返利确认数据到交易系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送返利确认数据到交易系统失败: " + ex.Message, message.ToJson(), message.Data.OrderId);
                return BasicResult<ConfirmRebateNotifyRequest>.Failed(request, "发送返利确认数据到交易系统失败: " + ex.Message, true);
            }
        }

        private static BaseRebateTransaction BuildBaseRebateTransaction(AccountTransaction transaction)
        {
            return new BaseRebateTransaction
            {
                Amount = transaction.Amount,
                OrderId = transaction.OrderIdentifier,
                TransactionId = transaction.TransactionIdentifier,
                Remark = transaction.TransDesc,
                UserId = transaction.UserIdentifier
            };
        }
        private static BaseRebateTransaction BuildBaseRebateTransaction(CouponAccountTransaction transaction)
        {
            return new BaseRebateTransaction
            {
                Amount = transaction.Amount,
                OrderId = Guid.Empty.ToGuidString(),
                TransactionId = transaction.TransactionIdentifier,
                Remark = transaction.TransDesc,
                UserId = transaction.UserIdentifier
            };
        }

        private static async Task<RebateTransaction> GetUserIdentifierFromRebateAsync(string orderId, int dbType)
        {
            string redisValue = RedisHelper.GetStringValue($"Gateway:Rebate:{orderId}");
            if (redisValue.IsNotNullOrEmpty())
            {
                BaseRebateTransaction info = redisValue.FromJson<BaseRebateTransaction>();
                return BuildRebateTransaction(info);
            }
            BaseRebateTransaction transaction = await BuildRebateInfoFromDBAsync(orderId, dbType);
            if (transaction == null)
            {
                return null;
            }
            return BuildRebateTransaction(transaction);
        }

        private static async Task<BaseRebateTransaction> BuildRebateInfoFromDBAsync(string orderId, int dbType)
        {
            BaseRebateTransaction info = null;
            switch (dbType)
            {
                case Constants.BIZDB:
                    using (BizContext db = new BizContext(ConfigManager.BizDBConnectionString))
                    {
                        AccountTransaction transaction = await db.AccountTransactions.FirstOrDefaultAsync(p => p.TransactionIdentifier == orderId);
                        if (transaction != null)
                        {
                            info = BuildBaseRebateTransaction(transaction);
                        }
                    }
                    break;
                case Constants.COUPONDB:
                    using (CouponContext db = new CouponContext(ConfigManager.CouponDBConnectionString))
                    {
                        try
                        {

                            CouponAccountTransaction transaction = await db.AccountTransactions.FirstOrDefaultAsync(p => p.TransactionIdentifier == orderId);
                            if (transaction != null)
                            {
                                info = BuildBaseRebateTransaction(transaction);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Info(ex.Message, "", "");
                            throw;
                        }
                    }
                    break;
                default:
                    return null;
            }
            return info;
        }

        private static HttpClient InitAssetServiceFabricHttpClient()
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

        private static RebateTransaction BuildRebateTransaction(BaseRebateTransaction transaction)
        {
            return new RebateTransaction
            {
                Amount = transaction.Amount,
                OrderId = transaction.OrderId,
                TransactionId = transaction.TransactionId,
                Remark = transaction.Remark,
                UserId = transaction.UserId,
                AssetId = string.Empty,
                PlatformCgId = ConfigManager.JYMMarketingIdentifier
            };
        }
    }
}
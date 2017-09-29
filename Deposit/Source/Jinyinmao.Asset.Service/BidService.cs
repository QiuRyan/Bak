// ******************************************************************************************************
// Project : Jinyinmao.Deposit File : BidService.cs Created : 2017-08-10 18:40
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-08-19 10:15 ******************************************************************************************************
// <copyright file="BidService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jinyinmao.Asset.Service.Interface;
using Jinyinmao.Asset.Service.Interface.Dtos;
using Jinyinmao.Deposit.Config;
using Jinyinmao.Deposit.Domain;
using Jinyinmao.Deposit.Lib;
using Jinyinmao.Deposit.Lib.Enum;
using Jinyinmao.ServiceBus.Service;
using Moe.Lib;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;
using Newtonsoft.Json;

namespace Jinyinmao.Asset.Service
{
    public class BidService : IBidService
    {
        private readonly HttpClient businesshttpClient = JYMInternalHttpClientFactory.Create(ConfigManager.BusinessRole, (TraceEntry)null);
        private readonly ServiceBusService serviceBusService = new ServiceBusService();

        private readonly HttpClient tirisfalClient = JYMInternalHttpClientFactory.Create(ConfigManager.TirisfalServiceRole, (TraceEntry)null);

        #region IBidService Members

        public async Task<BasicResult<BidCancelNotify>> DealBidCancelAsync(MessageBody<BidCancelNotify> message)
        {
            try
            {
                BasicResult<BidCancelNotify> sendBidCancelToAssetResult = await this.SendBidCancelToAssetAsync(message);
                if (!sendBidCancelToAssetResult.Result)
                {
                    return sendBidCancelToAssetResult;
                }

                LogHelper.Info("发送流标确认数据到交易系统:" + message.Data.OrderId, message.ToJson(), message.Data.OrderId);

                HttpResponseMessage tirisfalResult = await this.tirisfalClient.PostAsync($"Product/Regular/ProductCancel?productIdentifier={message.Data.OrderId}", new StringContent("", Encoding.UTF8));

                tirisfalResult.EnsureSuccessStatusCode();

                BaseResponse tirisfalResponse = await tirisfalResult.Content.ReadAsAsync<BaseResponse>();

                return !tirisfalResponse.Result ? BasicResult<BidCancelNotify>.Failed(message.Data, "发送流标确认数据到交易系统出错:" + tirisfalResponse.Remark) : BasicResult<BidCancelNotify>.Successed(message.Data);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<BidCancelNotify>.Failed(message.Data, $"正在重试第{message.RetryCount}次,发送流标确认数据到交易系统异常:{ex.Message}");
                }
                return BasicResult<BidCancelNotify>.Failed(message.Data, $"重试{message.RetryCount}次失败,发送流标确认数据到交易系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送流标确认数据到交易系统异常", message.Data.ToJson(), message.Data.OrderId);
                return BasicResult<BidCancelNotify>.Failed(message.Data, "发送流标确认数据到交易系统异常:" + ex);
            }
        }

        /// <summary>
        ///     标的报备
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<BasicResult<BidCreateNotify>> DealBidCreateAsync(MessageBody<BidCreateNotify> message)
        {
            try
            {
                LogHelper.Info("发送标的报备确认数据到资产系统:" + message.Data.BidId, message.Data.ToJson(), message.Data.BidId);

                HttpResponseMessage bussinessResponse = await this.businesshttpClient.PostAsJsonAsync("Products/RegisterRegularProduct", message.Data);

                bussinessResponse.EnsureSuccessStatusCode();

                BasicBussinessResponse response = await bussinessResponse.Content.ReadAsAsync<BasicBussinessResponse>();

                if (response.Code != 1000)
                {
                    return BasicResult<BidCreateNotify>.Failed(message.Data, $"发送标的报备确认数据到资产系统出错:{(int)bussinessResponse.StatusCode},Response: { response.Result}");
                }

                LogHelper.Info("发送标的报备确认数据到资产系统成功:" + message.Data.BidId, response.ToJson(), message.Data.BidId);

                return BasicResult<BidCreateNotify>.Successed(message.Data);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BidNotificationQueue, message.ToJson());
                    return BasicResult<BidCreateNotify>.Failed(message.Data, $"正在重试第{message.RetryCount}次,发送标的报备确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<BidCreateNotify>.Failed(message.Data, $"重试{message.RetryCount}次失败,发送标的报备确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送标的报备确认数据到资产系统异常 ", message.Data.ToJson(), message.Data.BidId);

                return BasicResult<BidCreateNotify>.Failed(message.Data, "发送标的报备确认数据到资产系统异常: " + ex.Message, true);
            }
        }

        public async Task<BasicResult<BidUpdateNotify>> DealBidUpdateAsync(MessageBody<BidUpdateNotify> message)
        {
            try
            {
                LogHelper.Info("发送标的修改确认数据到资产系统:" + message.Data.BidId, message.Data.ToJson(), message.Data.OrderId);

                HttpResponseMessage bussinessResponse = await this.businesshttpClient.PostAsJsonAsync("DepositAsset/EntrustedPay/Dispose", message.Data);

                bussinessResponse.EnsureSuccessStatusCode();

                BasicBussinessResponse response = await bussinessResponse.Content.ReadAsAsync<BasicBussinessResponse>();

                if (response.Code != 1000)
                {
                    return BasicResult<BidUpdateNotify>.Failed(message.Data, $"发送标的修改确认数据到资产系统出错:{(int)bussinessResponse.StatusCode},Response: { response.Result}");
                }
                LogHelper.Info("发送标的修改确认数据到资产系统成功:" + message.Data.BidId, response.ToJson(), message.Data.OrderId);

                return BasicResult<BidUpdateNotify>.Successed(message.Data);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BidNotificationQueue, message.ToJson());
                    return BasicResult<BidUpdateNotify>.Failed(message.Data, $"正在重试第{message.RetryCount}次,发送标的修改确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<BidUpdateNotify>.Failed(message.Data, $"重试{message.RetryCount}次失败,发送标的修改确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送标的修改确认数据到资产系统异常", message.Data.ToJson(), message.Data.OrderId);
                return BasicResult<BidUpdateNotify>.Failed(message.Data, "发送标的修改确认数据到资产系统异常: " + ex.Message, true);
            }
        }

        #endregion IBidService Members

        private async Task<BasicResult<BidCancelNotify>> SendBidCancelToAssetAsync(MessageBody<BidCancelNotify> message)
        {
            try
            {
                LogHelper.Info("发送流标确认数据到资产系统:" + message.Data.OrderId, message.Data.ToJson(), message.Data.OrderId);

                HttpResponseMessage bussinessResponse = await this.businesshttpClient.PostAsJsonAsync("DepositAsset/EntrustedPay/ConfirmBidCancel", message.Data);

                bussinessResponse.EnsureSuccessStatusCode();

                BasicBussinessResponse bussinessResult = JsonConvert.DeserializeObject<BasicBussinessResponse>(await bussinessResponse.Content.ReadAsStringAsync());

                if (bussinessResult.Code != 1000)
                {
                    return BasicResult<BidCancelNotify>.Failed(message.Data, $"发送流标确认数据到资产系统失败:{bussinessResult.Message}");
                }

                LogHelper.Info("发送流标确认数据到资产系统成功:" + message.Data.OrderId, bussinessResult.ToJson(), message.Data.OrderId);

                return BasicResult<BidCancelNotify>.Successed(message.Data);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<BidCancelNotify>.Failed(message.Data, $"正在重试第{message.RetryCount}次,发送流标确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<BidCancelNotify>.Failed(message.Data, $"重试{message.RetryCount}次失败,发送流标确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送流标确认数据到资产系统异常", message.Data.ToJson(), message.Data.OrderId);
                return BasicResult<BidCancelNotify>.Failed(message.Data, "发送流标确认数据到资产系统异常:" + ex);
            }
        }
    }
}
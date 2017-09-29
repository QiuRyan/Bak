// ******************************************************************************************************
// Project : Jinyinmao.Deposit File : UserService.cs Created : 2017-08-10 18:40
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-08-19 13:26 ******************************************************************************************************
// <copyright file="UserService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
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

namespace Jinyinmao.Tirisfal.Service
{
    [SuppressMessage("ReSharper", "RedundantAssignment")]
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    public class UserService : IUserService
    {
        private readonly HttpClient businesshttpClient = JYMInternalHttpClientFactory.Create(ConfigManager.BusinessRole, (TraceEntry)null);
        private readonly ServiceBusService serviceBusService = new ServiceBusService();

        private readonly HttpClient tirisfalClient = JYMInternalHttpClientFactory.Create(ConfigManager.TirisfalServiceRole, (TraceEntry)null);

        #region IUserService Members

        /// <summary>
        ///     放款
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<BasicResult<string>> BidLoansNotifyAsync(MessageBody<BidLoansNotify> message)
        {
            bool type = false;
            //MessageBody<BidLoansRepayNotify> request = null;
            try
            {
                BasicResult<string> businessResult = await this.SendBidLoansRepayToAssetAsync(message);
                if (!businessResult.Result)
                {
                    return BasicResult<string>.Failed(businessResult.Data, businessResult.Remark);
                }

                if (string.IsNullOrEmpty(businessResult.Data.Trim()))
                {
                    LogHelper.Info("发送放款确认数据到资产系统返回的ProductId为空不发送到交易系统:" + businessResult.Data, businessResult.Data.ToJson(), businessResult.Data);
                }

                LogHelper.Info("发送放款确认数据到交易系统,ProductId: " + businessResult.Data, businessResult.ToJson(), businessResult.Data);

                HttpResponseMessage tirisfalResult = await this.tirisfalClient.PostAsync($"BackOffice/RegularProduct/Grant/{businessResult.Data}", null);

                tirisfalResult.EnsureSuccessStatusCode();

                LogHelper.Info("发送放款确认数据到交易系统成功,ProductId: " + businessResult.Data, businessResult.Data.ToJson(), businessResult.Data);

                return BasicResult<string>.Successed(businessResult.Data);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    type = true;
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<string>.Failed(string.Empty, $"网络异常正在重试第{message.RetryCount}次,发送放款确认数据到交易系统异常:{ex.Message}");
                }

                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送放款确认数据到交易系统异常:" + ex.Message, true);
            }
            catch (TaskCanceledException ex) when (!type)
            {
                if (message.RetryCount < ConfigManager.MaxRetryCount)
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<string>.Failed(string.Empty, $"Task异常正在重试第{message.RetryCount}次,发送放款确认数据到交易系统异常:{ex.Message}");
                }

                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送放款确认数据到交易系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送放款确认数据到交易系统系统异常: " + ex.Message, message.Data.ToJson(), message.Data.OrderId);

                return BasicResult<string>.Failed(string.Empty, "发送放款确认数据到交易系统异常: " + ex.Message, true);
            }
        }

        /// <summary>
        ///     还款
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<BasicResult<string>> BidRepayNotifyAsync(MessageBody<BidRepayNotify> message)
        {
            bool type = false;
            try
            {
                BasicResult<string> businessResult = await this.SendBidRepayToAssetAsync(message);
                if (!businessResult.Result)
                {
                    return BasicResult<string>.Failed(businessResult.Data, businessResult.Remark);
                }

                if (string.IsNullOrEmpty(businessResult.Data.Trim()))
                {
                    LogHelper.Info("发送还款确认数据到资产系统返回的ProductId为空不发送到交易系统:" + businessResult.Data, businessResult.Data.ToJson(), businessResult.Data);
                }

                LogHelper.Info("发送还款确认数据到交易系统,ProductId: " + businessResult.Data, businessResult.Data.ToJson(), businessResult.Data);

                HttpResponseMessage tirisfalResult = await this.tirisfalClient.PostAsync($"BackOffice/RegularProduct/Repay/{businessResult.Data}", null);

                tirisfalResult.EnsureSuccessStatusCode();

                LogHelper.Info("发送还款确认数据到交易系统成功,ProductId: " + businessResult.Data, businessResult.Data.ToJson(), businessResult.Data);

                return BasicResult<string>.Successed(businessResult.Data);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    type = true;
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<string>.Failed(string.Empty, $"网络异常正在重试第{message.RetryCount}次,发送还款确认数据到交易系统异常:{ex.Message}");
                }
                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送还款确认数据到交易系统异常:" + ex.Message, true);
            }
            catch (TaskCanceledException ex) when (!type)
            {
                if (message.RetryCount < ConfigManager.MaxRetryCount)
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<string>.Failed(string.Empty, $"Task异常正在重试第{message.RetryCount}次,发送放款确认数据到交易系统异常:{ex.Message}");
                }

                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送放款确认数据到交易系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送还款确认数据到资产系统异常" + ex.Message, message.ToJson(), message.Data.OrderId);
                return BasicResult<string>.Failed(string.Empty, "发送还款确认数据到交易/资产系统异常" + ex.Message, true);
            }
        }

        public async Task<BasicResult<CreateAccountSuccessRequest>> DealCreateAccountAsync(MessageBody<FinishCreateAccountNotify> message)
        {
            CreateAccountSuccessRequest request = null;
            try
            {
                request = this.BuildCreateAccountSuccessRequest(message);
                if (message.Data.Status == "F") //开户失败不调用交易系统
                {
                    LogHelper.Info(message.Data.FailReason, message.ToJson(), message.Data.UserId);
                    return BasicResult<CreateAccountSuccessRequest>.Failed(request, message.Data.FailReason, true);
                }
                LogHelper.Info("发送用户开户确认数据到交易系统:" + request.UserIdentifier, request.ToJson(), request.UserIdentifier);

                HttpResponseMessage tirisfalResponse = await this.tirisfalClient.PostAsJsonAsync("/User/Auth/CreateAccountSuccess", request);

                BaseResponse tirisfalResult = await tirisfalResponse.Content.ReadAsAsync<BaseResponse>();

                tirisfalResponse.EnsureSuccessStatusCode();

                return !tirisfalResult.Result ? BasicResult<CreateAccountSuccessRequest>.Failed(request, $"发送用户开户确认数据到交易系统出错:{tirisfalResult.Remark}") : BasicResult<CreateAccountSuccessRequest>.Successed(request);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.FinishCreateAccountQueue, message.ToJson());
                    return BasicResult<CreateAccountSuccessRequest>.Failed(request, $"正在重试第{message.RetryCount}次,发送用户开户确认数据到交易系统异常:{ex.Message}");
                }
                return BasicResult<CreateAccountSuccessRequest>.Failed(request, $"重试{message.RetryCount}次失败,发送用户开户确认数据到交易系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送用户开户确认数据到交易系统异常", message.ToJson(), message.Data.UserId);

                return BasicResult<CreateAccountSuccessRequest>.Failed(request, "发送用户开户确认数据到交易系统异常:" + ex.Message, true);
            }
        }

        /// <summary>
        ///     返利
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;Tuple&lt;System.Boolean, System.String&gt;&gt;.</returns>
        public async Task<BasicResult<ConfirmRebateNotifyRequest>> RebateNotifyAsync(MessageBody<RebateNotify> message)
        {
            try
            {
                //01 - 签到返利，02 - 打赏返利，03 - 投资购买卡券返利，04 - 偏差补贴返利
                switch (message.Data.RebateType)
                {
                    case "01":
                        return await RebateService.SendRebateConfirmDataToTirisfalAsync(message);

                    case "02":
                    case "03":
                        return await RebateService.SendRebateConfirmDataToCouponAsync(message);

                    case "04":
                        return await RebateService.SendRebateConfirmDataToAssetAsync(message);

                    default:
                        return await Task.FromResult(new BasicResult<ConfirmRebateNotifyRequest> { Result = false, Remark = "返利渠道错误", ResultCode = ResultCode.Failed }); //TODO 资产待处理
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "返利回调异常:" + ex.Message, message.Data.ToJson(), message.Data.OrderId);
                return await Task.FromResult(new BasicResult<ConfirmRebateNotifyRequest> { Result = false, Remark = "返利异常: " + ex.Message, ResultCode = ResultCode.Failed });
            }
        }

        public async Task<BasicResult<RechargeRequest>> SetRechargeWithdrawResultAsync(MessageBody<RechargeWithdrawNotify> message)
        {
            string opType = message.NotifyType == NotifyTypes.RechargeNotify ? "充值" : "提现";
            RechargeRequest request = this.BuildRechargeRequest(message, ""); ;
            try
            {
                string userIdentifier = await GetUserIdentiferFromPaymentAsync(message.Data);

                if (userIdentifier.IsNullOrEmpty())
                {
                    return BasicResult<RechargeRequest>.Failed(request, "用户唯一编号为空");
                }
                request.UserIdentifier = userIdentifier;

                LogHelper.Info($"发送{opType}确认数据到交易系统:{request.OrderId}", request.ToJson(), request.OrderId);

                HttpResponseMessage responseMessage = await this.tirisfalClient.PostAsJsonAsync("User/Payment/SetRechargeResult", request);

                BaseResponse tirisfalResult = await responseMessage.Content.ReadAsAsync<BaseResponse>();

                responseMessage.EnsureSuccessStatusCode();

                return !tirisfalResult.Result ? BasicResult<RechargeRequest>.Failed(request, $"发送{opType}确认数据到交易系统失败:{tirisfalResult.Remark}") : BasicResult<RechargeRequest>.Successed(request);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.RechargeNotificationQueue, message.ToJson());
                    return BasicResult<RechargeRequest>.Failed(request, $"正在重试第{message.RetryCount}次,发送{opType}确认数据到交易系统异常:{ex.Message}");
                }
                return BasicResult<RechargeRequest>.Failed(request, $"重试{message.RetryCount}次失败,发送{opType}确认数据到交易系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, $"发送{opType}确认数据到交易系统异常:" + ex.Message, message.ToJson(), message.Data.OrderId);

                return BasicResult<RechargeRequest>.Failed(request, $"发送{opType}确认数据到交易系统异常:" + ex.Message, true);
            }
        }

        public async Task<BasicResult<ConfirmInvestingRequest>> UserInvestNotifyAsync(MessageBody<InvestNotify> message)
        {
            ConfirmInvestingRequest request = this.BuildConfirmInvestingRequest(message, "");
            try
            {
                string userIdentifier = await GetUserIdentiferFromOrderAsync(message.Data);

                if (string.IsNullOrEmpty(userIdentifier))
                {
                    return BasicResult<ConfirmInvestingRequest>.Failed(request, "发送用户投资确认数据到交易系统失败,UserId为空", true);
                }

                request.UserIdentifier = userIdentifier;

                LogHelper.Info($"发送用户投资确认数据到交易系统:{message.Data.OrderId}", request.ToJson(), message.Data.OrderId);

                HttpResponseMessage response = await this.tirisfalClient.PostAsJsonAsync("Investing/ConfirmRegular", request);
                response.EnsureSuccessStatusCode();

                BaseResponse result = await response.Content.ReadAsAsync<BaseResponse>();

                return !result.Result ? BasicResult<ConfirmInvestingRequest>.Failed(request, "发送用户投资确认数据到交易系统失败:" + result.Remark, true) : BasicResult<ConfirmInvestingRequest>.Successed(request);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.RegularInvestNotificationQueue, message.ToJson());
                    return BasicResult<ConfirmInvestingRequest>.Failed(request, $"正在重试第{message.RetryCount}次,发送用户投资确认数据到交易系统异常:{ex.Message}");
                }
                return BasicResult<ConfirmInvestingRequest>.Failed(request, $"重试{message.RetryCount}次失败,发送用户投资确认数据到交易系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送用户投资确认数据到交易系统异常:" + ex.Message, message.ToJson(), message.Data.OrderId);

                return BasicResult<ConfirmInvestingRequest>.Failed(request, "发送用户投资确认数据到交易系统异常:" + ex.Message, true);
            }
        }

        #endregion IUserService Members

        private static async Task<string> GetUserIdentiferFromOrderAsync(InvestNotify notifyInfo)
        {
            string redisValue = RedisHelper.GetStringValue($"Gateway:Order:{notifyInfo.OrderId}");

            string userIdentifier = string.Empty;
            if (redisValue.IsNotNullOrEmpty())
            {
                userIdentifier = redisValue.FromJson<UserInfo>()?.UserId.ToGuidString();
            }
            if (userIdentifier.IsNullOrEmpty())
            {
                using (BizContext db = new BizContext(ConfigManager.BizDBConnectionString))
                {
                    Order order = await db.Orders.FirstOrDefaultAsync(p => p.OrderIdentifier == notifyInfo.OrderId);
                    if (order != null)
                    {
                        userIdentifier = order.UserIdentifier;
                    }
                }
            }

            return userIdentifier;
        }

        private static async Task<string> GetUserIdentiferFromPaymentAsync(RechargeWithdrawNotify notifyInfo)
        {
            string redisValue = RedisHelper.GetStringValue($"Gateway:Payment:{notifyInfo.OrderId}");

            string userIdentifier = string.Empty;
            if (redisValue.IsNotNullOrEmpty())
            {
                userIdentifier = redisValue.FromJson<UserInfo>()?.UserId.ToGuidString();
            }

            if (userIdentifier.IsNullOrEmpty())
            {
                using (BizContext db = new BizContext(ConfigManager.BizDBConnectionString))
                {
                    AccountTransaction transaction = await db.AccountTransactions.FirstOrDefaultAsync(p => p.TransactionIdentifier == notifyInfo.OrderId);
                    if (transaction != null)
                    {
                        userIdentifier = transaction.UserIdentifier;
                    }
                }
            }

            return userIdentifier;
        }

        private ConfirmInvestingRequest BuildConfirmInvestingRequest(MessageBody<InvestNotify> message, string userIdentifier)
        {
            return new ConfirmInvestingRequest
            {
                OrderId = message.Data.OrderId,
                UserIdentifier = userIdentifier,
                ResultCode = message.Data.Status.GetResultCode()
            };
        }

        private CreateAccountSuccessRequest BuildCreateAccountSuccessRequest(MessageBody<FinishCreateAccountNotify> message)
        {
            return new CreateAccountSuccessRequest
            {
                BankCardCellphone = message.Data.Phone,
                BankCardNo = message.Data.BankCardNo,
                BankCode = message.Data.BankCode,
                UserIdentifier = message.Data.UserId
            };
        }

        private RechargeRequest BuildRechargeRequest(MessageBody<RechargeWithdrawNotify> message, string useridentifier)
        {
            RechargeWithdrawNotify notifyInfo = message.Data;

            return new RechargeRequest
            {
                ResultCode = notifyInfo.Status.GetResultCode(),
                OrderId = notifyInfo.OrderId,
                UserIdentifier = useridentifier,
                RespSubCode = notifyInfo.Status.GetResultCode() == -1 ? Constants.RESPSUBCODE : "",
                NotifyType = message.NotifyType,
                FailReason = message.Data.FailReason
            };
        }

        private async Task<BasicResult<string>> SendBidLoansRepayToAssetAsync(MessageBody<BidLoansNotify> message)
        {
            bool type = false;
            try
            {
                LogHelper.Info("发送放款确认数据到资产系统:" + message.Data.OrderId, message.Data.ToJson(), message.Data.OrderId);

                //成功与否均需要通知资产
                HttpResponseMessage businessResponse = await this.businesshttpClient.PostAsJsonAsync("FinanciereGrant/FinanciereGrant/AffirmLoan", message.Data);
                NotifyBussinessResponse businessResult = await businessResponse.Content.ReadAsAsync<NotifyBussinessResponse>();

                businessResponse.EnsureSuccessStatusCode();

                if (businessResult.Result == null || !businessResult.Result.Result || businessResult.Code != 1000)
                {
                    return BasicResult<string>.Failed(businessResult.ToJson(), "发送放款确认数据到资产系统失败:" + businessResult.Message);
                }

                if (businessResult.Result?.ProductCategoryCode == "100000030")
                {
                    return BasicResult<string>.Failed(businessResult.ToJson(), "放款确认数据产品类型为余额猫,不通知交易系统");
                }

                if (message.Data.Status.GetResultCode() != 1)
                {
                    return BasicResult<string>.Failed(businessResult.ToJson(), $"放款确认数据状态为{message.Data.Status.GetResultCode()}不通知交易系统");
                }

                LogHelper.Info("发送放款确认数据到资产系统成功:" + message.Data.OrderId, businessResult.ToJson(), message.Data.OrderId);

                return BasicResult<string>.Successed(businessResult.Result?.ProductId);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    type = true;
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<string>.Failed(string.Empty, $"正在重试第{message.RetryCount}次,发送放款确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送放款确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (TaskCanceledException ex) when (!type)
            {
                if (message.RetryCount < ConfigManager.MaxRetryCount)
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<string>.Failed(string.Empty, $"正在重试第{message.RetryCount}次,发送放款确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送放款确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送放款确认数据到资产系统异常: " + ex.Message, message.Data.ToJson(), message.Data.OrderId);
                return BasicResult<string>.Failed(string.Empty, "发送放款确认数据到资产系统异常: " + ex.Message, true);
            }
        }

        private async Task<BasicResult<string>> SendBidRepayToAssetAsync(MessageBody<BidRepayNotify> message)
        {
            bool type = false;
            try
            {
                LogHelper.Info($"发送还款确认数据到资产系统,OrderId:{ message.Data.OrderId}", message.Data.ToJson(), message.Data.OrderId);

                //成功与否均需要通知资产
                HttpResponseMessage businessResponse = await this.businesshttpClient.PostAsJsonAsync("FinanciereGrant/FinanciereGrant/AffirmRepayment", message.Data);
                NotifyBussinessResponse businessResult = await businessResponse.Content.ReadAsAsync<NotifyBussinessResponse>();

                businessResponse.EnsureSuccessStatusCode();

                if (businessResult.Result == null || !businessResult.Result.Result || businessResult.Code != 1000)
                {
                    return BasicResult<string>.Failed(businessResult.ToJson(), $"发送还款确认数据到资产系统失败:{businessResult.Message}");
                }

                if (businessResult.Result?.ProductCategoryCode == "100000030")
                {
                    return BasicResult<string>.Failed(businessResult.ToJson(), "还款确认数据产品类型为余额猫,不通知交易系统");
                }

                if (message.Data.Status.GetResultCode() != 1)
                {
                    return BasicResult<string>.Failed(businessResult.ToJson(), $"还款确认数据状态为{message.Data.Status.GetResultCode()}不通知交易系统");
                }

                LogHelper.Info("发送还款确认数据到资产系统成功:" + message.Data.OrderId, businessResult.ToJson(), message.Data.OrderId);

                return BasicResult<string>.Successed(businessResult.Result?.ProductId);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    type = true;
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());

                    return BasicResult<string>.Failed(string.Empty, $"正在重试第{message.RetryCount}次,发送还款确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送还款确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (TaskCanceledException ex) when (!type)
            {
                if (message.RetryCount < ConfigManager.MaxRetryCount)
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.BusinessQueue, message.ToJson());
                    return BasicResult<string>.Failed(string.Empty, $"正在重试第{message.RetryCount}次,发送还款确认数据到资产系统异常:{ex.Message}");
                }
                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送还款确认数据到资产系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发送还款确认数据到资产系统异常", message.ToJson(), message.Data.OrderId);

                return BasicResult<string>.Failed(string.Empty, "发送还款确认数据到资产系统异常:" + ex.Message, true);
            }
        }
    }
}
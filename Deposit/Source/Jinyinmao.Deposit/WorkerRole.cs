// ******************************************************************************************************
// Project : Jinyinmao.Deposit File : WorkerRole.cs Created : 2017-08-10 18:40
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-08-19 10:12 ******************************************************************************************************
// <copyright file="WorkerRole.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Jinyinmao.Asset.Service;
using Jinyinmao.Asset.Service.Interface;
using Jinyinmao.Asset.Service.Interface.Dtos;
using Jinyinmao.Coupon.Service;
using Jinyinmao.Coupon.Service.Interface;
using Jinyinmao.Deposit.Config;
using Jinyinmao.Deposit.Domain;
using Jinyinmao.Deposit.Lib;
using Jinyinmao.Deposit.Lib.Enum;
using Jinyinmao.Message.Service;
using Jinyinmao.Message.Service.Interface;
using Jinyinmao.Tirisfal.Service;
using Jinyinmao.Tirisfal.Service.Interface;
using Jinyinmao.Tirisfal.Service.Interface.Dtos;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Moe.Lib;
using BatchCreditAssignmentCreateNotify = Jinyinmao.Tirisfal.Service.Interface.Dtos.BatchCreditAssignmentCreateNotify;

namespace Jinyinmao.Deposit
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly ManualResetEvent CompletedEvent = new ManualResetEvent(false);
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private AssetYemService assetYemService;
        private IBidService bidService;
        private ICouponService couponService;
        private IMessageService messageService;
        private IUserService userService;
        private IUserYemService userYemService;

        public override bool OnStart()
        {
            WorkerRoleRegister.Register();

            this.assetYemService = new AssetYemService();
            this.bidService = new BidService();
            this.userService = new UserService();
            this.userYemService = new UserYemService();
            this.couponService = new CouponService();
            this.messageService = new MessageService();

            this.bidNotificationQueue = WorkerRoleRegister.CreateQueueClient(ConfigManager.BidNotificationQueue);
            this.rechargeNotificationQueue = WorkerRoleRegister.CreateQueueClient(ConfigManager.RechargeNotificationQueue);
            this.withdrawNotificationQueue = WorkerRoleRegister.CreateQueueClient(ConfigManager.WithdrawNotificationQueue);
            this.finishCreateAccountQueue = WorkerRoleRegister.CreateQueueClient(ConfigManager.FinishCreateAccountQueue);
            this.businessQueue = WorkerRoleRegister.CreateQueueClient(ConfigManager.BusinessQueue);
            this.bookNotificationQueue = WorkerRoleRegister.CreateQueueClient(ConfigManager.BookNotificationQueue);
            this.regularinvestnotificationQueue = WorkerRoleRegister.CreateQueueClient(ConfigManager.RegularInvestNotificationQueue);
            //this.couponUseQueue = WorkerRoleRegister.CreateQueueClient(ConfigManager.CouponUseQueue);
            this.MessageNotificationQueue = WorkerRoleRegister.CreateQueueClient(ConfigManager.MessageQueue);
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 512;

            return base.OnStart();
        }

        public override void OnStop()
        {
            this.bidNotificationQueue.Close();
            this.rechargeNotificationQueue.Close();
            this.withdrawNotificationQueue.Close();
            this.finishCreateAccountQueue.Close();
            this.businessQueue.Close();
            this.bookNotificationQueue.Close();
            this.regularinvestnotificationQueue.Close();
            //this.couponUseQueue.Close();
            this.MessageNotificationQueue.Close();

            this.CompletedEvent.Set();
            this.tokenSource.Cancel();
            base.OnStop();
        }

        public override void Run()
        {
            //充值回调
            this.rechargeNotificationQueue.OnMessageAsync(async receivedMessage => await this.RechargeNotificationQueueAsync(receivedMessage));

            //提现回调
            this.withdrawNotificationQueue.OnMessageAsync(async receivedMessage => await this.WithdrawNotificationQueueAsync(receivedMessage));

            //开户成功
            this.finishCreateAccountQueue.OnMessageAsync(async receivedMessage => await this.FinishCreateAccountQueueAsync(receivedMessage));

            //标的报备 修改
            this.bidNotificationQueue.OnMessageAsync(async receivedMessage => await this.BidNotificationQueueAsync(receivedMessage));

            // 放款回调 流标回调 还款回调 返利回调 代偿还款回调 还代偿款回调 债权转让回调 免密债权转让回调债权转让放款回调
            this.businessQueue.OnMessageAsync(async receivedMessage => await this.BusinessQueueAsync(receivedMessage));

            //用户投资回调
            //this.regularinvestnotificationQueue.OnMessageAsync(async receivedMessage => await this.RegularInvestQueueAsync(receivedMessage));

            //余额猫回调处理
            this.bookNotificationQueue.OnMessageAsync(async receivedMessage => await this.BookNotificationQueueAsync(receivedMessage));

            //卡券使用
            //this.couponUseQueue.OnMessageAsync(async receivedMessage => await this.CouponUseQueueAsync(receivedMessage));

            //短信发送
            this.MessageNotificationQueue.OnMessageAsync(async receivedMessage => await this.MessageQueueAsync(receivedMessage));

            this.BatchDealRegularInvestNotifyConfirmAsync();

            //Thread.Sleep(TimeSpan.FromSeconds(2));

            this.CompletedEvent.WaitOne();
        }

        #region 队列处理私有方法

        /// <summary>
        ///     批量处理定期购买回调确认
        /// </summary>
        private void BatchDealRegularInvestNotifyConfirmAsync()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    IEnumerable<BrokeredMessage> list = await this.regularinvestnotificationQueue.ReceiveBatchAsync(5);

                    list.ForEach(message =>
                    {
                        Task.Factory.StartNew(async () => await this.RegularInvestQueueAsync(message)).ContinueWith(task =>
                        {
                            if (task.Exception?.InnerExceptions != null)
                            {
                                LogHelper.Error(task.Exception?.InnerException, "批量处理定期购买回调异常", message.ToJson(), this.regularinvestnotificationQueue.Path);
                            }
                        });
                    });

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
                // ReSharper disable once FunctionNeverReturns
            }).ContinueWith(task =>
            {
                if (task.Exception?.InnerExceptions != null)
                {
                    LogHelper.Error(task.Exception?.InnerException, "批量处理定期购买回调异常", null, this.regularinvestnotificationQueue.Path);
                }
            });

            //Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     标的报备、修改
        /// </summary>
        /// <param name="brokeredMessage"></param>
        /// <returns>Task.</returns>
        private async Task BidNotificationQueueAsync(BrokeredMessage brokeredMessage)
        {
            string messagebody = brokeredMessage.GetBody<string>();
            NotifyTypeResult notifyType = messagebody.FromJson<NotifyTypeResult>();
            //标的报备
            if (notifyType.NotifyType == NotifyTypes.BidCreatedNotify)
            {
                MessageBody<BidCreateNotify> message = messagebody.FromJson<MessageBody<BidCreateNotify>>();
                BasicResult<BidCreateNotify> result = await this.bidService.DealBidCreateAsync(message);

                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }
                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.BidId);
                }
            }

            //标的修改
            if (notifyType.NotifyType == NotifyTypes.BidUpdateNotify)
            {
                MessageBody<BidUpdateNotify> message = messagebody.FromJson<MessageBody<BidUpdateNotify>>();
                BasicResult<BidUpdateNotify> result = await this.bidService.DealBidUpdateAsync(message);
                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }
                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.BidId);
                }
            }
        }

        /// <summary>
        ///     余额猫回调处理
        /// </summary>
        /// <param name="brokeredMessage"></param>
        /// <returns>Task.</returns>
        private async Task BookNotificationQueueAsync(BrokeredMessage brokeredMessage)
        {
            string messagebody = brokeredMessage.GetBody<string>();
            NotifyTypeResult notifyType = messagebody.FromJson<NotifyTypeResult>();
            //预约冻结
            if (notifyType.NotifyType == NotifyTypes.BookFreeze)
            {
                MessageBody<BookFreezeNotify> message = messagebody.FromJson<MessageBody<BookFreezeNotify>>();
                BasicResult<YemBookFrozenRequest> result = await this.userYemService.DealYemBookFreezeNotifyAsync(message);
                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }
                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.OrderId);
                }
            }

            //取消预约冻结
            if (notifyType.NotifyType == NotifyTypes.BookFreezeCancel)
            {
                MessageBody<BookFreezeCancelNotify> message = messagebody.FromJson<MessageBody<BookFreezeCancelNotify>>();
                BasicResult<BookFreezeCancelNotify> result = await this.userYemService.DealYemBookFreezeCancelNotifyAsync(message);
                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }
                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.OrderId);
                }
            }

            //预约批量债权转让投资
            if (notifyType.NotifyType == NotifyTypes.BookCreditCreateBatch)
            {
                MessageBody<BatchCreditAssignmentCreateNotify> message = messagebody.FromJson<MessageBody<BatchCreditAssignmentCreateNotify>>();
                BasicResult<BatchCreditAssignmentCreateNotify> result = await this.userYemService.DealYemBookCreditCreateBatchNotifyAsync(message);
                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }
                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.OrderId);
                }
            }

            //预约批量投资
            if (notifyType.NotifyType == NotifyTypes.BatchBookInvest)
            {
                MessageBody<BatchBookInvestNotify> message = messagebody.FromJson<MessageBody<BatchBookInvestNotify>>();
                BasicResult<BatchBookInvestNotify> result = await this.userYemService.DealYemBatchBookInvestNotifyAsync(message);
                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }
                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.OrderId);
                }
            }
        }

        /// <summary>
        ///     放款回调 流标回调 还款回调 返利回调 代偿还款回调 还代偿款回调 债权转让回调 免密债权转让回调 债权转让放款回调
        /// </summary>
        /// <param name="brokeredMessage"></param>
        /// <returns>Task.</returns>
        private async Task BusinessQueueAsync(BrokeredMessage brokeredMessage)
        {
            string messagebody = brokeredMessage.GetBody<string>();
            NotifyTypeResult notifyType = messagebody.FromJson<NotifyTypeResult>();
            //流标
            if (notifyType.NotifyType == NotifyTypes.BidCancelNotify)
            {
                MessageBody<BidCancelNotify> message = messagebody.FromJson<MessageBody<BidCancelNotify>>();
                BasicResult<BidCancelNotify> result = await this.bidService.DealBidCancelAsync(message);
                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }
                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.OrderId);
                }
            }

            ////用户投资(未来会移除)
            //if (notifyType.NotifyType == NotifyTypes.UserInvertmentNotify)
            //{
            //    MessageBody<InvestNotify> investmessage = messagebody.FromJson<MessageBody<InvestNotify>>();
            //    BasicResult<ConfirmInvestingRequest> result = await this.userService.UserInvestNotifyAsync(investmessage);
            //    if (!result.Result)
            //    {
            //        if (result.IsSendDeadLetter)
            //        {
            //            await brokeredMessage.DeadLetterAsync();
            //        }
            //        LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.OrderId);
            //    }
            //}
            //放款
            if (notifyType.NotifyType == NotifyTypes.BidLoansNotidy)
            {
                MessageBody<BidLoansNotify> loansmessage = messagebody.FromJson<MessageBody<BidLoansNotify>>();
                BasicResult<string> result = await this.userService.BidLoansNotifyAsync(loansmessage);
                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }

                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data);
                }
            }

            //还款 代偿还款
            if (notifyType.NotifyType == NotifyTypes.BidRepayNotify || notifyType.NotifyType == NotifyTypes.CompensationRepayNotify)
            {
                MessageBody<BidRepayNotify> repaymessage = messagebody.FromJson<MessageBody<BidRepayNotify>>();
                BasicResult<string> result = await this.userService.BidRepayNotifyAsync(repaymessage);
                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }
                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data);
                }
            }

            //返利
            if (notifyType.NotifyType == NotifyTypes.RebateNotify)
            {
                MessageBody<RebateNotify> rebatemessage = messagebody.FromJson<MessageBody<RebateNotify>>();

                BasicResult<ConfirmRebateNotifyRequest> result = await this.userService.RebateNotifyAsync(rebatemessage);

                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }
                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data?.OrderId);
                }
            }

            //债权转让投资放款
            if (notifyType.NotifyType == NotifyTypes.CreditAssignmentGrant)
            {
                MessageBody<CreditAssignmentGrantNotify> message = messagebody.FromJson<MessageBody<CreditAssignmentGrantNotify>>();
                BasicResult<CreditAssignmentGrantNotify> result = await this.assetYemService.DealYemCreditAssignmentGrantNotifyAsync(message);
                if (!result.Result)
                {
                    if (result.IsSendDeadLetter)
                    {
                        await brokeredMessage.DeadLetterAsync();
                    }
                    LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.OrderId);
                }
            }
        }

        /// <summary>
        ///     卡券使用
        /// </summary>
        /// <param name="brokeredMessage">The brokered message.</param>
        /// <returns>Task.</returns>
        private async Task CouponUseQueueAsync(BrokeredMessage brokeredMessage)
        {
            string messagebody = brokeredMessage.GetBody<string>();
            UseCouponMessage message = messagebody.FromJson<UseCouponMessage>();

            BasicResult<string> result = await this.UseCouponAsync(message);

            if (!result.Result)
            {
                if (result.IsSendDeadLetter)
                {
                    await brokeredMessage.DeadLetterAsync();
                }
                LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data);
            }
        }

        /// <summary>
        ///     开户成功
        /// </summary>
        /// <param name="brokeredMessage"></param>
        /// <returns>Task.</returns>
        private async Task FinishCreateAccountQueueAsync(BrokeredMessage brokeredMessage)
        {
            string messagebody = brokeredMessage.GetBody<string>();
            MessageBody<FinishCreateAccountNotify> message = messagebody.FromJson<MessageBody<FinishCreateAccountNotify>>();

            BasicResult<CreateAccountSuccessRequest> result = await this.userService.DealCreateAccountAsync(message);
            if (!result.Result)
            {
                if (result.IsSendDeadLetter)
                {
                    await brokeredMessage.DeadLetterAsync();
                }
                LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.UserIdentifier);
            }
        }

        /// <summary>
        ///     短信发送
        /// </summary>
        /// <param name="brokeredMessage">The brokered message.</param>
        /// <returns>Task.</returns>
        private async Task MessageQueueAsync(BrokeredMessage brokeredMessage)
        {
            string messagebody = brokeredMessage.GetBody<string>();
            MessageRquest message = messagebody.FromJson<MessageRquest>();

            bool sendResult = await this.messageService.SendRegularRepaySuccessMessageAsync(new SmsMessageSenderRequest(message.Cellphone, message.BizCode, message.Content));
            if (!sendResult)
            {
                await brokeredMessage.DeadLetterAsync();
                LogHelper.Info("发送定期理财还款短信失败:" + message.Cellphone, message.ToJson(), message.Cellphone);
            }
            LogHelper.Info("发送定期理财还款短信:" + message.Cellphone, message.ToJson(), message.Cellphone);
        }

        /// <summary>
        ///     充值回调
        /// </summary>
        /// <param name="brokeredMessage"></param>
        /// <returns>Task.</returns>
        private async Task RechargeNotificationQueueAsync(BrokeredMessage brokeredMessage)
        {
            string messagebody = brokeredMessage.GetBody<string>();
            MessageBody<RechargeWithdrawNotify> message = messagebody.FromJson<MessageBody<RechargeWithdrawNotify>>();
            BasicResult<RechargeRequest> result = await this.userService.SetRechargeWithdrawResultAsync(message);
            if (!result.Result)
            {
                if (result.IsSendDeadLetter)
                {
                    await brokeredMessage.DeadLetterAsync();
                }
                LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.OrderId);
            }
        }

        //用户投资回调
        private async Task RegularInvestQueueAsync(BrokeredMessage brokeredMessage)
        {
            string messagebody = brokeredMessage.GetBody<string>();
            //用户投资
            MessageBody<InvestNotify> investmessage = messagebody.FromJson<MessageBody<InvestNotify>>();
            BasicResult<ConfirmInvestingRequest> result = await this.userService.UserInvestNotifyAsync(investmessage);
            if (!result.Result)
            {
                if (!result.IsSendDeadLetter)
                {
                    await brokeredMessage.DeadLetterAsync();
                }
                await brokeredMessage.CompleteAsync();
                LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.OrderId);
            }
            await brokeredMessage.CompleteAsync();
        }

        //卡券使用
        private async Task<BasicResult<string>> UseCouponAsync(UseCouponMessage request)
        {
            LogHelper.Info($"发送卡券使用数据到卡券系统,类型:{request.CouponType}", request.ToJson(), request.CouponMessage.CouponIdentifier);

            switch (request.CouponType)
            {
                case 20: return await this.couponService.UsePrincipalCouponAsync(request);

                case 40: return await this.couponService.UseIncreaseCouponAsync(request);

                case 50: return await this.couponService.UseCashBackCouponAsync(request);

                default:
                    return BasicResult<string>.Failed(string.Empty, "未知错误");
            }
        }

        /// <summary>
        ///     提现回调
        /// </summary>
        /// <param name="brokeredMessage"></param>
        /// <returns>Task.</returns>
        private async Task WithdrawNotificationQueueAsync(BrokeredMessage brokeredMessage)
        {
            string messagebody = brokeredMessage.GetBody<string>();
            MessageBody<RechargeWithdrawNotify> message = messagebody.FromJson<MessageBody<RechargeWithdrawNotify>>();
            BasicResult<RechargeRequest> result = await this.userService.SetRechargeWithdrawResultAsync(message);
            if (!result.Result)
            {
                if (result.IsSendDeadLetter)
                {
                    await brokeredMessage.DeadLetterAsync();
                }
                LogHelper.Info(result.Remark, result.Data.ToJson(), result.Data.OrderId);
            }
        }

        #endregion 队列处理私有方法

        #region 字段属性

        /// <summary>
        ///     标的队列
        /// </summary>
        private QueueClient bidNotificationQueue;

        /// <summary>
        ///     The book notification queue
        /// </summary>
        private QueueClient bookNotificationQueue;

        /// <summary>
        ///     用户投资回调 放款回调 流标回调 还款回调 返利回调 代偿还款回调 还代偿款回调 债权转让回调 免密债权转让回调债权转让放款回调
        /// </summary>
        private QueueClient businessQueue;

        /// <summary>
        ///     卡券使用队列
        /// </summary>
        /// <summary>
        ///     开户成功队列
        /// </summary>
        private QueueClient finishCreateAccountQueue;

        /// <summary>
        ///     短信队列
        /// </summary>
        private QueueClient MessageNotificationQueue;

        /// <summary>
        ///     充值队列
        /// </summary>
        private QueueClient rechargeNotificationQueue;

        /// <summary>
        ///     定期投资队列
        /// </summary>
        private QueueClient regularinvestnotificationQueue;

        /// <summary>
        ///     充值队列
        /// </summary>
        private QueueClient withdrawNotificationQueue;

        #endregion 字段属性
    }
}
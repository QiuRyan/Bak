// ******************************************************************************************************
// Project : Jinyinmao.Deposit File : IUserService.cs Created : 2017-08-10 13:09
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-08-10 15:46 ******************************************************************************************************
// <copyright file="IUserService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Threading.Tasks;
using Jinyinmao.Deposit.Domain;
using Jinyinmao.Tirisfal.Service.Interface.Dtos;

namespace Jinyinmao.Tirisfal.Service.Interface
{
    public interface IUserService
    {
        /// <summary>
        /// 放款
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;BasicResult&lt;MessageBody&lt;BidLoansRepayNotify&gt;&gt;&gt;.</returns>
        Task<BasicResult<string>> BidLoansNotifyAsync(MessageBody<BidLoansNotify> message);

        /// <summary>
        /// 还款
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;BasicResult&lt;MessageBody&lt;BidRepayNotify&gt;&gt;&gt;.</returns>
        Task<BasicResult<string>> BidRepayNotifyAsync(MessageBody<BidRepayNotify> message);

        /// <summary>
        /// 投资开户回调
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;BasicResult&lt;CreateAccountSuccessRequest&gt;&gt;.</returns>
        Task<BasicResult<CreateAccountSuccessRequest>> DealCreateAccountAsync(MessageBody<FinishCreateAccountNotify> message);

        /// <summary>
        /// 返利
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;Tuple&lt;System.Boolean, System.String&gt;&gt;.</returns>
        Task<BasicResult<ConfirmRebateNotifyRequest>> RebateNotifyAsync(MessageBody<RebateNotify> message);

        /// <summary>
        /// 发送充值/提现确认数据到交易系统
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;BasicResult&lt;RechargeRequest&gt;&gt;.</returns>
        Task<BasicResult<RechargeRequest>> SetRechargeWithdrawResultAsync(MessageBody<RechargeWithdrawNotify> message);

        /// <summary>
        /// 用户投资
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;BasicResult&lt;ConfirmInvestingRequest&gt;&gt;.</returns>
        Task<BasicResult<ConfirmInvestingRequest>> UserInvestNotifyAsync(MessageBody<InvestNotify> message);
    }
}
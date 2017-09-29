// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : UserYemService.cs
// Created          : 2017-08-10  16:21
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  16:21
// ******************************************************************************************************
// <copyright file="UserYemService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Threading.Tasks;
using Jinyinmao.Deposit.Domain;
using Jinyinmao.Tirisfal.Service.Interface.Dtos;

namespace Jinyinmao.Tirisfal.Service.Interface
{
    public interface IUserYemService
    {
        /// <summary>
        ///     预约批量投资
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<BasicResult<BatchBookInvestNotify>> DealYemBatchBookInvestNotifyAsync(MessageBody<BatchBookInvestNotify> message);

        /// <summary>
        ///     预约批量债权转让
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<BasicResult<BatchCreditAssignmentCreateNotify>> DealYemBookCreditCreateBatchNotifyAsync(MessageBody<BatchCreditAssignmentCreateNotify> message);

        /// <summary>
        ///     余额猫取消预约冻结
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;Tuple&lt;System.Boolean, System.String&gt;&gt;.</returns>
        Task<BasicResult<BookFreezeCancelNotify>> DealYemBookFreezeCancelNotifyAsync(MessageBody<BookFreezeCancelNotify> message);

        /// <summary>
        ///     余额猫预约冻结
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<BasicResult<YemBookFrozenRequest>> DealYemBookFreezeNotifyAsync(MessageBody<BookFreezeNotify> message);
    }
}
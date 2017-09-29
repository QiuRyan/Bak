// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : IAssetYemService.cs
// Created          : 2017-08-10  15:57
// 
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  16:26
// ******************************************************************************************************
// <copyright file="IAssetYemService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Threading.Tasks;
using Jinyinmao.Deposit.Domain;

namespace Jinyinmao.Asset.Service.Interface
{
    public interface IAssetYemService
    {
        /// <summary>
        ///     债权转让放款
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;BasicResult&lt;CreditAssignmentGrantNotify&gt;&gt;.</returns>
        Task<BasicResult<CreditAssignmentGrantNotify>> DealYemCreditAssignmentGrantNotifyAsync(MessageBody<CreditAssignmentGrantNotify> message);
    }
}
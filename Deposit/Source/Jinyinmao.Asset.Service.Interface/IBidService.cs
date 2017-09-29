// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : IBidService.cs
// Created          : 2017-08-10  14:17
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  14:32
// ******************************************************************************************************
// <copyright file="IBidService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Threading.Tasks;
using Jinyinmao.Asset.Service.Interface.Dtos;
using Jinyinmao.Deposit.Domain;

namespace Jinyinmao.Asset.Service.Interface
{
    public interface IBidService
    {
        /// <summary>
        ///     流标
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;BasicResult&lt;BidCreateNotify&gt;&gt;.</returns>
        Task<BasicResult<BidCancelNotify>> DealBidCancelAsync(MessageBody<BidCancelNotify> message);

        /// <summary>
        ///     标的报备
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;BasicResult&lt;BidCreateNotify&gt;&gt;.</returns>
        Task<BasicResult<BidCreateNotify>> DealBidCreateAsync(MessageBody<BidCreateNotify> message);

        /// <summary>
        ///     标的修改
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;BasicResult&lt;BidCreateNotify&gt;&gt;.</returns>
        Task<BasicResult<BidUpdateNotify>> DealBidUpdateAsync(MessageBody<BidUpdateNotify> message);
    }
}
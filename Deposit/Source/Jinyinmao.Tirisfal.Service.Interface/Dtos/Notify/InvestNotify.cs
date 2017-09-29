// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : InvestNotify.cs
// Created          : 2017-08-10  15:07
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  15:08
// ******************************************************************************************************
// <copyright file="InvestNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Jinyinmao.Deposit.Domain;
using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     用户投资回调返回类型
    /// </summary>
    /// <seealso cref="Jinyinmao.Deposit.Domain.BasicBusinessNotify" />
    public class InvestNotify : BasicBusinessNotify
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        [JsonProperty("amount")]
        public long Amount { get; set; }
    }
}
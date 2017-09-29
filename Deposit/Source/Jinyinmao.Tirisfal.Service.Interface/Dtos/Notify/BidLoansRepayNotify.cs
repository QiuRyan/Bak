// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BidLoansRepayNotify.cs
// Created          : 2017-08-10  15:16
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  15:16
// ******************************************************************************************************
// <copyright file="BidLoansRepayNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    public class BidLoansRepayNotify
    {
        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        [JsonProperty("productIdentifier")]
        public string ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
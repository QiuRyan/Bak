// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : RechargeWithdrawNotify.cs
// Created          : 2017-08-10  13:17
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:17
// ******************************************************************************************************
// <copyright file="RechargeWithdrawNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     充值提现信息
    /// </summary>
    public class RechargeWithdrawNotify
    {
        /// <summary>
        ///     金额(单位分)
        /// </summary>
        /// <value>The amount.</value>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        ///     失败原因
        /// </summary>
        [JsonProperty("failReason")]
        public string FailReason { get; set; }

        /// <summary>
        ///     商户号
        /// </summary>
        /// <value>The merchant identifier.</value>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     Gets or sets the remark.
        /// </summary>
        /// <value>The remark.</value>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        ///     Gets or sets the signature.
        /// </summary>
        /// <value>The signature.</value>
        [JsonProperty("signature")]
        public string Signature { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
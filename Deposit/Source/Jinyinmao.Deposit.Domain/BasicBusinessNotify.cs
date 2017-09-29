// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BasicBusinessNotify.cs
// Created          : 2017-08-10  15:07
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  15:08
// ******************************************************************************************************
// <copyright file="BasicBusinessNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Deposit.Domain
{
    public class BasicBusinessNotify
    {
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
        [JsonProperty("orderID")]
        public string OrderId { get; set; }

        /// <summary>
        ///     备注
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
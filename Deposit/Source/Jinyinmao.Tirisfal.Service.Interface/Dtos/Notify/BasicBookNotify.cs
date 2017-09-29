// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BasicBookNotify.cs
// Created          : 2017-08-10  16:09
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  16:09
// ******************************************************************************************************
// <copyright file="BasicBookNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     活期回调基类
    /// </summary>
    public class BasicBookNotify
    {
        /// <summary>
        ///     商户号
        /// </summary>
        /// <value>The merchant identifier.</value>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

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
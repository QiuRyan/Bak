// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BidCreateNotify.cs
// Created          : 2017-08-10  14:30
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  14:30
// ******************************************************************************************************
// <copyright file="BidCreateNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Asset.Service.Interface.Dtos
{
    /// <summary>
    ///     标的报备回调信息
    /// </summary>
    public class BidCreateNotify
    {
        /// <summary>
        ///     银行审查结果
        /// </summary>
        /// <value>The bank audit status.</value>
        [JsonProperty("bankAuditStatus")]
        public string BankAuditStatus { get; set; }

        /// <summary>
        ///     标的代码
        /// </summary>
        /// <value>The bid identifier.</value>
        [JsonProperty("bidId")]
        public string BidId { get; set; }

        /// <summary>
        ///     标的状态
        /// </summary>
        /// <value>The bid status.</value>
        [JsonProperty("bidStatus")]
        public string BidStatus { get; set; }

        /// <summary>
        ///     商户号
        /// </summary>
        /// <value>The merchant identifier.</value>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

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
    }
}
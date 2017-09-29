// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : CreditAssignmentGrantNotify.cs
// Created          : 2017-08-10  16:20
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  16:20
// ******************************************************************************************************
// <copyright file="CreditAssignmentGrantNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Deposit.Domain
{
    /// <summary>
    ///     债权转让放款异步通知
    /// </summary>
    public class CreditAssignmentGrantNotify
    {
        /// <summary>
        ///     商户编号
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        ///     失败原因
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///     由网贷平台生成的唯一的交易流水号
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        ///     签名，附加说明
        /// </summary>
        [JsonIgnore]
        [JsonProperty("signature")]
        public string Signature { get; set; }

        /// <summary>
        ///     状态(S-开户成功;F-开户失败)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
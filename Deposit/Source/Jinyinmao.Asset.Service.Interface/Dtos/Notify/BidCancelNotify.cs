﻿// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BidCancelNotify.cs
// Created          : 2017-08-10  14:30
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  14:30
// ******************************************************************************************************
// <copyright file="BidCancelNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Asset.Service.Interface.Dtos
{
    /// <summary>
    ///     流标
    /// </summary>
    public class BidCancelNotify
    {
        /// <summary>
        ///     商户编号
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        ///     商户订单ID或者标的ID
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
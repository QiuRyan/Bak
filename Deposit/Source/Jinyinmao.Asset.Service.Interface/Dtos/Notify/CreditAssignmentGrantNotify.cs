// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : CreditAssignmentGrantNotify.cs
// Created          : 2017-08-10  15:54
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  16:20
// ******************************************************************************************************
// <copyright file="CreditAssignmentGrantNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jinyinmao.Asset.Service.Interface.Dtos
{
    /// <summary>
    ///     批量债权转让异步回调
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class BatchCreditAssignmentCreateNotify
    {
        /// <summary>
        ///     商户编号
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

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
        ///     处理子单结果集合
        /// </summary>
        [JsonProperty("resultList")]
        public List<BatchCreditCreateResult> ResultList { get; set; }

        /// <summary>
        ///     签名，附加说明
        /// </summary>
        [JsonIgnore]
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BatchBookInvestNotify.cs
// Created          : 2017-08-10  16:18
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  16:19
// ******************************************************************************************************
// <copyright file="BatchBookInvestNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     预约批量投资异步回调
    /// </summary>
    public class BatchBookInvestNotify
    {
        /// <summary>
        ///     网贷平台商户号
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
        ///     订单集合
        /// </summary>
        [JsonProperty("resultList")]
        public List<BatchCreditCreateResult> ResultList { get; set; }
    }

    /// <summary>
    ///     批量债权转让异步回调结果集数据
    /// </summary>
    public class BatchCreditCreateResult
    {
        /// <summary>
        ///     失败原因
        /// </summary>
        [JsonProperty("failReason")]
        public string FailReason { get; set; }

        /// <summary>
        ///     S = 成功F = 失败
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        ///     请求流水号
        /// </summary>
        [JsonProperty("subOrderId")]
        public string SubOrderId { get; set; }
    }
}
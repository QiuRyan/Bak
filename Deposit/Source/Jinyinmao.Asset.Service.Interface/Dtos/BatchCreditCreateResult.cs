// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BatchCreditCreateResult.cs
// Created          : 2017-08-10  15:59
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  15:59
// ******************************************************************************************************
// <copyright file="BatchCreditCreateResult.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Asset.Service.Interface.Dtos
{
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
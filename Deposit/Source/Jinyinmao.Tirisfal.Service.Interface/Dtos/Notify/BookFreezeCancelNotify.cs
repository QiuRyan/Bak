// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BookFreezeCancelNotify.cs
// Created          : 2017-08-10  16:16
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  16:16
// ******************************************************************************************************
// <copyright file="BookFreezeCancelNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     取消预约冻结异步回调通知
    /// </summary>
    public class BookFreezeCancelNotify : BasicBookNotify
    {
        /// <summary>
        ///     解冻金额
        /// </summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        ///     失败原因
        /// </summary>
        [JsonProperty("failReason")]
        public string FailReason { get; set; }

        /// <summary>
        ///     收费金额
        /// </summary>
        [JsonProperty("feeAmount")]
        public long FeeAmount { get; set; }

        /// <summary>
        ///     手续费账户ID
        /// </summary>
        [JsonProperty("feeUserId")]
        public string FeeUserId { get; set; }

        /// <summary>
        ///     请求流水号
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        ///     解冻类型，默认：预约投资
        /// </summary>
        [JsonProperty("unFreezeType")]
        public string UnFreezeType { get; set; }
    }
}
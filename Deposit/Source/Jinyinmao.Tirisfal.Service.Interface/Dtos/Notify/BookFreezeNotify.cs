// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BookFreezeNotify.cs
// Created          : 2017-08-10  16:08
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  16:09
// ******************************************************************************************************
// <copyright file="BookFreezeNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     预约冻结异步回调
    /// </summary>
    public class BookFreezeNotify : BasicBookNotify
    {
        /// <summary>
        ///     失败原因
        /// </summary>
        [JsonProperty("failReason")]
        public string FailReason { get; set; }

        /// <summary>
        ///     冻结账户金额
        /// </summary>
        [JsonProperty("freezeAccountAmount")]
        public long FreezeAccountAmount { get; set; }

        /// <summary>
        ///     冻结类型，默认：预约投资
        /// </summary>
        [JsonProperty("freezeType")]
        public string FreezeType { get; set; }

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
        ///     P2P用户ID
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
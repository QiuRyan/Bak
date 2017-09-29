// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : ConfirmRebateNotifyRequest.cs
// Created          : 2017-08-10  15:24
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  15:25
// ******************************************************************************************************
// <copyright file="ConfirmRebateNotifyRequest.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     交易系统返利回调
    /// </summary>
    public class ConfirmRebateNotifyRequest
    {
        /// <summary>
        ///     订单ID
        /// </summary>
        /// <value>The order identifier.</value>
        [JsonProperty("orderIdentifier")]
        public string OrderId { get; set; }

        /// <summary>
        ///     回调结果
        /// </summary>
        /// <value>The result codo.</value>
        [JsonProperty("resultCode")]
        public int ResultCode { get; set; }

        /// <summary>
        ///     用户ID
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}
// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : RechargeRequest.cs
// Created          : 2017-08-10  13:16
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:16
// ******************************************************************************************************
// <copyright file="RechargeRequest.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     充值回调请求信息
    /// </summary>
    public class RechargeRequest
    {
        /// <summary>
        ///     失败原因
        /// </summary>
        /// <value>The fail reason.</value>
        [JsonProperty("failReason")]
        public string FailReason{get; set;}

        /// <summary>
        ///     Gets or sets the type of the notify.
        /// </summary>
        /// <value>The type of the notify.</value>
        [JsonProperty("notifytype")]
        public int NotifyType{get; set;}

        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        [JsonProperty("transactionIdentifier")]
        public string OrderId{get; set;}

        [JsonProperty("respSubCode")]
        public string RespSubCode{get; set;}

        /// <summary>
        ///     Gets or sets the result code.
        /// </summary>
        /// <value>The result code.</value>
        [JsonProperty("resultCode")]
        public int ResultCode{get; set;}

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        public string UserIdentifier{get; set;}
    }
}
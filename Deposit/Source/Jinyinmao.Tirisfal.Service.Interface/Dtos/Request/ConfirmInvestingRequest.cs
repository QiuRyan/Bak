// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : ConfirmInvestingRequest.cs
// Created          : 2017-08-10  15:08
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  15:09
// ******************************************************************************************************
// <copyright file="ConfirmInvestingRequest.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     投资确认回调请求(交易系统)
    /// </summary>
    public class ConfirmInvestingRequest
    {
        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        [JsonProperty("orderIdentifier")]
        [Required]
        public string OrderId { get; set; }

        /// <summary>
        ///     Gets or sets the result code.
        /// </summary>
        /// <value>The result code.</value>
        [JsonProperty("resultCode")]
        [Required]
        public int ResultCode { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        [Required]
        public string UserIdentifier { get; set; }
    }
}
// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : YemBookFrozenRequest.cs
// Created          : 2017-08-10  16:08
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  16:08
// ******************************************************************************************************
// <copyright file="YemBookFrozenRequest.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     余额猫冻结回调请求
    /// </summary>
    public class YemBookFrozenRequest
    {
        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        [JsonProperty("orderIdentifier")]
        [Required]
        public string OrderId { get; set; }

        /// <summary>
        ///     1:放款成功 -1:失败: 0:购买成功 -2:初始状态
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
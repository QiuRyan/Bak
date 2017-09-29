// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : NotifyBussinessResponse.cs
// Created          : 2017-08-10  15:18
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  15:18
// ******************************************************************************************************
// <copyright file="NotifyBussinessResponse.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     放款通知资产输出
    /// </summary>
    public class NotifyBussinessResponse
    {
        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [JsonProperty("result")]
        public NotifyBussinessResultResponse Result { get; set; }
    }
}
// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BasicBussinessResponse.cs
// Created          : 2017-08-10  18:40
// 
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-19  10:15
// ******************************************************************************************************
// <copyright file="BasicBussinessResponse.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Deposit.Domain
{
    /// <summary>
    ///     资产系统输出公用
    /// </summary>
    public class BasicBussinessResponse
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
        public object Result { get; set; }
    }

    public class BussinessResponse
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this instance is true.
        /// </summary>
        /// <value><c>true</c> if this instance is true; otherwise, <c>false</c>.</value>
        [JsonProperty("isTrue")]
        public bool IsTrue { get; set; }

        /// <summary>
        ///     Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
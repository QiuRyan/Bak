// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : MessageBody.cs
// Created          : 2017-08-10  13:21
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:21
// ******************************************************************************************************
// <copyright file="MessageBody.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Deposit.Domain
{
    public class MessageBody<T> where T : class, new()
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("notifytype")]
        public int NotifyType { get; set; }

        [JsonProperty("retryCount")]
        public int RetryCount { get; set; }
    }
}
// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : NotifyTypeResult.cs
// Created          : 2017-08-10  14:56
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  14:56
// ******************************************************************************************************
// <copyright file="NotifyTypeResult.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Deposit.Domain
{
    public class NotifyTypeResult
    {
        [JsonProperty("notifyType")]
        public int NotifyType { get; set; }
    }
}
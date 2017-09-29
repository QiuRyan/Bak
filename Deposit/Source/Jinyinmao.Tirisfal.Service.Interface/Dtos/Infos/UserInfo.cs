// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : UserInfo.cs
// Created          : 2017-08-10  13:18
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:18
// ******************************************************************************************************
// <copyright file="UserInfo.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos.Infos
{
    public class UserInfo
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
    }
}
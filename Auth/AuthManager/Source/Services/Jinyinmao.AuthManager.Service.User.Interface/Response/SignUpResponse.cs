// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : SignUpResponse.cs
// Created          : 2016-12-14  20:16
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:20
// ***********************************************************************
// <copyright file="SignUpResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Service.User.Interface.Response
{
    public class SignUpResponse
    {
        /// <summary>
        ///     用户的手机号码
        /// </summary>
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}
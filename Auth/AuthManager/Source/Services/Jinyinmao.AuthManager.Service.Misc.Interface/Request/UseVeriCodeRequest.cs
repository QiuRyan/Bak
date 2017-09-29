// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : UseVeriCodeRequest.cs
// Created          : 2016-12-14  20:19
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:20
// ***********************************************************************
// <copyright file="UseVeriCodeRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Service.Misc.Request
{
    public class UseVeriCodeRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        ///     验证码用途类型。10 => 注册，20 => 重置登录密码，30 => 重置支付密码，40 => 快速登录，50 => 微信注册，60 => 微信绑定
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }
    }
}
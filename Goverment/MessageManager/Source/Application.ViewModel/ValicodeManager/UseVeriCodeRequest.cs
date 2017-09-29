// ***********************************************************************
// Project          : MessageManager
// File             : UseVeriCodeRequest.cs
// Created          : 2015-12-08  10:45
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-08  10:47
// ***********************************************************************
// <copyright file="UseVeriCodeRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jinyinmao.Application.ViewModel.ValicodeManager
{
    /// <summary>
    ///     使用验证码请求
    /// </summary>
    public class UseVeriCodeRequest
    {
        /// <summary>
        ///     验签
        /// </summary>
        [Required, JsonProperty("token"), StringLength(32, MinimumLength = 32)]
        public string Token { get; set; }

        /// <summary>
        ///     验证码类型
        /// </summary>
        [Required, JsonProperty("type")]
        public VeriCodeType Type { get; set; }
    }
}
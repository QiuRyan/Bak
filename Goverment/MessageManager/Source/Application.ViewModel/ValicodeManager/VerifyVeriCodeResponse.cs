// ***********************************************************************
// Project          : MessageManager
// File             : VerifyVeriCodeResponse.cs
// Created          : 2015-12-07  13:32
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-07  13:36
// ***********************************************************************
// <copyright file="VerifyVeriCodeResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jinyinmao.Application.ViewModel.ValicodeManager
{
    /// <summary>
    ///     验证验证码结果
    /// </summary>
    public class VerifyVeriCodeResponse
    {
        /// <summary>
        ///     剩余的验证次数，该次数不需要告知用户，若为 -1 ，则该验证码已失效。验证码过期等其他非异常情况也会返回 -1 。
        ///     若为 0 ，则该验证码失效，不能再进行验证。该值为 -1 或者 0 时，可以显示“请重新发送验证码”
        /// </summary>
        /// <value>The remain count.</value>
        [Required]
        [JsonProperty("remainCount")]
        public int RemainCount { get; set; }

        /// <summary>
        ///     本次验证结果
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        [Required]
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        ///     验证码验证后的token，若验证码验证成功，则该token为32位字符串，若验证失败，为空字符串
        /// </summary>
        /// <value>The token.</value>
        [Required]
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
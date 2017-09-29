// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : VerifyVeriCodeResult.cs
// Created          : 2016-12-14  20:19
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:20
// ***********************************************************************
// <copyright file="VerifyVeriCodeResult.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Jinyinmao.AuthManager.Service.Misc.Interface.ResponseResult
{
    /// <summary>
    ///     Class VerifyVeriCodeResult.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class VerifyVeriCodeResult
    {
        /// <summary>
        ///     剩余的验证次数，该次数不需要告知用户，若为 -1 ，则该验证码已失效。验证码过期等其他非异常情况也会返回 -1 。
        ///     若为 0 ，则该验证码失效，不能再进行验证。该值为 -1 或者 0 时，可以显示“请重新发送验证码”
        /// </summary>
        [JsonProperty("remainCount")]
        public int RemainCount { get; set; }

        /// <summary>
        ///     本次验证结果
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        ///     验证码验证后的token，若验证码验证成功，则该token为32位字符串，若验证失败，为空字符串
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
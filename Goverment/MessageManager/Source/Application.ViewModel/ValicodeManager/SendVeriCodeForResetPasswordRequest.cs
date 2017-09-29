// ***********************************************************************
// Project          : MessageManager
// File             : SendVeriCodeRequest.cs
// Created          : 2015-12-07  11:11
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-07  13:34
// ***********************************************************************
// <copyright file="SendVeriCodeRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.Lib.Web;
using Newtonsoft.Json;

namespace Jinyinmao.Application.ViewModel.ValicodeManager
{
    /// <summary>
    ///     SendVeriCodeForResetPasswordRequest.
    /// </summary>
    public class SendVeriCodeForResetPasswordRequest
    {
        /// <summary>
        ///     手机号码，验证使用正则表达式：^(13|14|15|16|17|18)\d{9}$
        /// </summary>
        [Required]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     验证码用途类型。30 => 重置支付密码
        /// </summary>
        [Required]
        [AvailableValues(VeriCodeType.ResetPaymentPassword)]
        [JsonProperty("type")]
        public VeriCodeType Type { get; set; }
    }
}
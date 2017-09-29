// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ValidateOldCellphoneRequest.cs
// Created          : 2016-08-15  11:43 AM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-15  11:44 AM
// ***********************************************************************
// <copyright file="ValidateOldCellphoneRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request.Basic
{
    /// <summary>
    ///     Class ValidateOldCellphoneRequest.
    /// </summary>
    public class ValidateOldCellphoneRequest
    {
        /// <summary>
        ///     手机号码
        /// </summary>
        /// <value>The cellphone.</value>
        [JsonProperty("cellphone")]
        [Required]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        public string OriginCellphone { get; set; }

        /// <summary>
        ///     短信Token
        /// </summary>
        /// <value>The token.</value>
        [JsonProperty("smsToken")]
        [Required]
        public string OriginCellphoneToken { get; set; }

        /// <summary>
        ///     验证码
        /// </summary>
        /// <value>The verification code.</value>
        [JsonProperty("verificationCode")]
        [Required]
        public string VerificationCode { get; set; }
    }
}
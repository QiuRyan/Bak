// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ValidateNewCellphoneRequest.cs
// Created          : 2016-08-15  5:31 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-15  6:00 PM
// ***********************************************************************
// <copyright file="ValidateNewCellphoneRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request.Basic
{
    /// <summary>
    ///     Class ValidateCredentialNo.
    /// </summary>
    public class ValidateNewCellphoneRequest
    {
        /// <summary>
        ///     新手机号码
        /// </summary>
        /// <value>The cellphone.</value>
        [JsonProperty("newCellphone")]
        [Required]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        public string NewCellphone { get; set; }

        /// <summary>
        ///     新的短信Token
        /// </summary>
        /// <value>The token.</value>
        [JsonProperty("smsToken")]
        [Required]
        public string SMSToken { get; set; }

        /// <summary>
        ///     身份验证的token
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("stepSecondToken")]
        [Required]
        public string StepSecondToken { get; set; }

        /// <summary>
        ///     验证码
        /// </summary>
        /// <value>The verification code.</value>
        [JsonProperty("verificationCode")]
        [Required]
        public string VerificationCode { get; set; }
    }
}
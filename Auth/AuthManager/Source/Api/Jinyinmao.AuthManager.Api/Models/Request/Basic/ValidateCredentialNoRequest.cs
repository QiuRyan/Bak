// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ValidateCredentialNoRequest.cs
// Created          : 2016-08-12  4:50 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-15  11:44 AM
// ***********************************************************************
// <copyright file="ValidateCredentialNoRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
    public class ValidateCredentialNoRequest
    {
        /// <summary>
        ///     身份证号码
        /// </summary>
        /// <value>The credential no.</value>
        [JsonProperty("credentialNo")]
        [Required]
        public string CredentialNo { get; set; }

        /// <summary>
        ///     姓名
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     第一步验证通过的Token
        /// </summary>
        /// <value>The token.</value>
        [JsonProperty("stepFirstToken")]
        [Required]
        public string StepFirstToken { get; set; }
    }
}
// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : AdminModifyCellphoneRequest.cs
// Created          : 2016-08-22  4:01 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-22  4:29 PM
// ***********************************************************************
// <copyright file="AdminModifyCellphoneRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request.Basic
{
    /// <summary>
    ///     Class AdminModifyCellphoneRequest.
    /// </summary>
    public class AdminModifyCellphoneRequest
    {
        /// <summary>
        ///     新手机号码
        /// </summary>
        /// <value>The new cellphone.</value>
        [Required]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        [JsonProperty("newCellphone")]
        public string NewCellphone { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        /// <value>The user identifier.</value>
        [Required]
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}
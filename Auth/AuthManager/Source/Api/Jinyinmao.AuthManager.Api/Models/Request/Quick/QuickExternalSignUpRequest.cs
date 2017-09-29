// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : QuickExternalSignUpRequest.cs
// Created          : 2016-08-01  10:24 AM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-01  10:30 AM
// ***********************************************************************
// <copyright file="QuickExternalSignUpRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class QuickExternalSignUpRequest.
    /// </summary>
    public class QuickExternalSignUpRequest
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        [Required]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     客户端标识, 900 => PC, 901 => iPhone, 902 => Android, 903 => M
        /// </summary>
        [JsonProperty("clientType")]
        public long? ClientType { get; set; }

        /// <summary>
        ///     活动编号(推广相关)
        /// </summary>
        [JsonProperty("contractId")]
        public long? ContractId { get; set; }

        /// <summary>
        ///     额外信息.
        /// </summary>
        [JsonProperty("info")]
        public Dictionary<string, object> Info { get; set; }
    }
}
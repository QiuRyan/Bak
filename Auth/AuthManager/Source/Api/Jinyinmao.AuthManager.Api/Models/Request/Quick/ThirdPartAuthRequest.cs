// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ThirdPartAuthRequest.cs
// Created          : 2016-10-09  13:40
//
//
// Last Modified On : 2016-10-13  09:21
// ***********************************************************************
// <copyright file="ThirdPartAuthRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class ThirdPartAuthRequest.
    /// </summary>
    public class ThirdPartAuthRequest
    {
        /// <summary>
        ///     手机号码
        /// </summary>
        /// <value>The token.</value>
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
        ///     EMAIL
        /// </summary>
        /// <value>正確格式 如EMAIL@EMAIL.com</value>
        [RegularExpression(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$")]
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        ///     the credential no.
        /// </summary>
        /// <value>The credential no.</value>
        [JsonProperty("idcard")]
        public string Idcard { get; set; }

        /// <summary>
        ///     额外信息.
        /// </summary>
        [JsonProperty("info")]
        public Dictionary<string, object> Info { get; set; }

        /// <summary>
        ///     邀请码(推广相关)
        /// </summary>
        [JsonProperty("inviteBy")]
        public string InviteBy { get; set; }

        /// <summary>
        ///     金银e家代码
        /// </summary>
        [JsonProperty("outletCode")]
        public string OutletCode { get; set; }
    }
}
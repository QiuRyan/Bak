// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : SignUpRequest.cs
// Created          : 2016-12-14  20:16
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:17
// ***********************************************************************
// <copyright file="SignUpRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Service.User.Interface.Request
{
    public class SignUpRequest
    {
        /// <summary>
        ///     客户端标识, 900 => PC, 901 => iPhone, 902 => Android, 903 => M
        /// </summary>
        [JsonProperty("clientType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? ClientType { get; set; }

        /// <summary>
        ///     活动编号(推广相关)
        /// </summary>
        [JsonProperty("contractId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? ContractId { get; set; }

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

        /// <summary>
        ///     用户设置的密码（6-18位的数字、字母、一般特殊字符组合）
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        ///     验证码口令
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : RegisterMessageRequest.cs
// Created          : 2016-12-14  21:00
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  21:00
// ***********************************************************************
// <copyright file="RegisterMessageRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Service.Coupon.Interface.Dtos
{
    /// <summary>
    ///     Class RegisterMessageRequest.
    /// </summary>
    public class RegisterMessageRequest
    {
        /// <summary>
        ///     用户手机号码
        /// </summary>
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     活动编号(推广相关)
        /// </summary>
        [JsonProperty("contractId")]
        public long? ContractId { get; set; }

        /// <summary>
        ///     邀请人手机号码
        /// </summary>
        [JsonProperty("inviterCellphone")]
        public string InviterCellphone { get; set; }

        /// <summary>
        ///     邀请人用户唯一标识
        /// </summary>
        [JsonProperty("inviterIdentifier")]
        public string InviterIdentifier { get; set; }

        /// <summary>
        ///     邀请人自己的邀请码
        /// </summary>
        [JsonProperty("inviterInviteFor")]
        public string InviterInviteFor { get; set; }

        /// <summary>
        ///     是否需要建立邀请关系
        /// </summary>
        /// <value><c>true</c> if this instance is need to build relationship; otherwise, <c>false</c>.</value>
        [JsonProperty("isNeedToBuildRelationship")]
        public bool IsNeedToBuildRelationship { get; set; }

        /// <summary>
        ///     活动平台
        /// </summary>
        [JsonProperty("terminal")]
        public int? Terminal { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}
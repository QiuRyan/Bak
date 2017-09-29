// ***********************************************************************
// Project          : MessageManager
// File             : UserInfoRequest.cs
// Created          : 2015-11-30  19:21
//
// Last Modified By : 张政平
// Last Modified On : 2015-11-30  19:21
// ***********************************************************************
// <copyright file="UserInfoRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.MessageManager.Domain.Entity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.MessageManager.Api.Models
{
    /// <summary>
    ///
    /// </summary>
    public class UserInfoRequest
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [JsonProperty("emailNum")]
        [StringLength(30)]
        public string Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [JsonProperty("phoneNum")]
        [StringLength(30)]
        public string PhoneNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("remark")]
        [StringLength(300)]
        public string Remark { get; set; }

        /// <summary>
        /// 终端Id
        /// </summary>
        [JsonProperty("uAppId")]
        [StringLength(20, MinimumLength = 2)]
        public string UAppId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        [JsonProperty("uId")]
        [StringLength(50)]
        public string UId { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        [JsonProperty("weChatNum")]
        [StringLength(30)]
        public string WeChatNum { get; set; }
    }

    internal static partial class UserInfoEx
    {
        internal static UserInfo ToEntity(this UserInfoRequest request)
        {
            return new UserInfo
            {
                UId = request.UId,
                PhoneNum = request.PhoneNum,
                Remark = request.Remark,
                WeChatNum = request.WeChatNum,
                UAppId = request.UAppId
            };
        }
    }
}
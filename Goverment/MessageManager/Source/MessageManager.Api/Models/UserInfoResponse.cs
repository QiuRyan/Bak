// ***********************************************************************
// Project          : MessageManager
// File             : UserInfoResponse.cs
// Created          : 2015-11-30  19:22
//
// Last Modified By : 张政平
// Last Modified On : 2015-11-30  19:22
// ***********************************************************************
// <copyright file="UserInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Jinyinmao.MessageManager.Domain.Entity;
using Newtonsoft.Json;

namespace Jinyinmao.MessageManager.Api.Models
{
    /// <summary>
    ///     UserInfoResponse.
    /// </summary>
    public class UserInfoResponse
    {
        /// <summary>
        ///     手机号
        /// </summary>
        /// <value>The phone number.</value>
        [JsonProperty("phoneNum")]
        [StringLength(30)]
        public string PhoneNum { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        /// <value>The remark.</value>
        [JsonProperty("remark")]
        [StringLength(300)]
        public string Remark { get; set; }

        /// <summary>
        ///     终端Id
        /// </summary>
        /// <value>The u application identifier.</value>
        [JsonProperty("uAppId")]
        [StringLength(20, MinimumLength = 2)]
        public string UAppId { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        /// <value>The u identifier.</value>
        [Required]
        [JsonProperty("uId")]
        [StringLength(50)]
        public string UId { get; set; }

        /// <summary>
        ///     微信号
        /// </summary>
        /// <value>The we chat number.</value>
        [JsonProperty("weChatNum")]
        [StringLength(30)]
        public string WeChatNum { get; set; }
    }

    /// <summary>
    ///     UserInfoEx.
    /// </summary>
    internal static partial class UserInfoEx
    {
        /// <summary>
        ///     To the response.
        /// </summary>
        /// <param name="objUserInfo">The object user information.</param>
        /// <returns>UserInfoResponse.</returns>
        internal static UserInfoResponse ToResponse(this UserInfo objUserInfo)
        {
            if (objUserInfo != null)
            {
                return new UserInfoResponse
                {
                    UId = objUserInfo.UId,
                    PhoneNum = objUserInfo.PhoneNum,
                    Remark = objUserInfo.Remark,
                    WeChatNum = objUserInfo.WeChatNum,
                    UAppId = objUserInfo.UAppId
                };
            }
            return null;
        }
    }
}
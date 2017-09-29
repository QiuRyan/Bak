// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : SignInUpResponse.cs
// Created          : 2016-10-11  14:38
//
//
// Last Modified On : 2016-10-13  09:43
// ***********************************************************************
// <copyright file="SignInUpResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class SignInUpResponse.
    /// </summary>
    public class SignInUpResponse
    {
        /// <summary>
        ///     用户认证token
        /// </summary>
        [Required]
        [JsonProperty("auth")]
        public string Auth { get; set; }

        /// <summary>
        ///     是否為新註冊用戶
        /// </summary>
        public bool IsNewer { get; set; }

        /// <summary>
        ///     UserAuthInfoResponse
        /// </summary>
        public UserAuthInfoResponse UserAuthInfoResponse { get; set; }
    }

    internal static class SignInUpResponseEx
    {
        internal static SignInUpResponse ToThirdPartInUpResponse(this UserInfo result, UserInfo userAuthInfoResponse, bool isNewer, string authToken = "")
        {
            return new SignInUpResponse
            {
                Auth = authToken ?? string.Empty,
                IsNewer = isNewer,
                UserAuthInfoResponse = userAuthInfoResponse.ToResponse()
            };
        }
    }
}
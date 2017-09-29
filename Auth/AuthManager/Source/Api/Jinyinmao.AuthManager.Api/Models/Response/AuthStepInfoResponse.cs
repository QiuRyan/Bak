// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : AuthStepInfoResponse.cs
// Created          : 2016-08-15  4:46 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-15  4:51 PM
// ***********************************************************************
// <copyright file="AuthStepInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class AuthStepInfoResponseEx.
    /// </summary>
    public static class AuthStepInfoResponseEx
    {
        /// <summary>
        ///     To the response.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>AuthStepInfoResponse.</returns>
        public static AuthStepInfoResponse ToResponse(this AuthStepInfo info)
        {
            return new AuthStepInfoResponse
            {
                Token = info.Token
            };
        }
    }

    /// <summary>
    ///     Class AuthStepInfoResponse.
    /// </summary>
    public class AuthStepInfoResponse
    {
        /// <summary>
        ///     验证信息Token
        /// </summary>
        /// <value>The token.</value>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
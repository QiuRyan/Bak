using Jinyinmao.AuthManager.Service.User.Interface.Dtos;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class SignUpUserIdInfoEx.
    /// </summary>
    internal static class SignUpUserIdInfoEx
    {
        /// <summary>
        ///     To the response.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>SignUpUserIdResponse.</returns>
        public static SignUpUserIdResponse ToResponse(this SignUpUserIdInfo info)
        {
            return new SignUpUserIdResponse
            {
                Cellphone = info.Cellphone,
                Registered = info.Registered,
                UserId = info.UserId
            };
        }
    }

    /// <summary>
    ///     Class SignUpUserIdResponse.
    /// </summary>
    internal class SignUpUserIdResponse
    {
        /// <summary>
        ///     手机号码.
        /// </summary>
        /// <value>The cellphone.</value>
        [JsonProperty("cellphone")]
        [Required]
        public string Cellphone { get; set; }

        /// <summary>
        ///     是否注册
        /// </summary>
        /// <value><c>true</c> if registered; otherwise, <c>false</c>.</value>
        [JsonProperty("registered")]
        [Required]
        public bool Registered { get; set; }

        /// <summary>
        ///     用户唯一标识.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userId")]
        [Required]
        public Guid UserId { get; set; }
    }
}
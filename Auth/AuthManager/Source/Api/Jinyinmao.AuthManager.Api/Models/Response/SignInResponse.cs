using Jinyinmao.AuthManager.Service.User.Interface.Dtos;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class SignInResponse.
    /// </summary>
    public class SignInResponse
    {
        /// <summary>
        ///     用户认证token
        /// </summary>
        [Required]
        [JsonProperty("auth")]
        public string Auth { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        [Required]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     账户是否被锁定
        /// </summary>
        [Required]
        [JsonProperty("lock")]
        public bool Lock { get; set; }

        /// <summary>
        ///     剩余尝试登录次数，如果账户被锁定，只能通过修改登录密码的方式进行重置
        /// </summary>
        [Required]
        [JsonProperty("remainCount")]
        public int RemainCount { get; set; }

        /// <summary>
        ///     登录结果
        /// </summary>
        [Required]
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        ///     用户是否存在
        /// </summary>
        [Required]
        [JsonProperty("userExist")]
        public bool UserExist { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [Required]
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
    }

    internal static class SignInResultEx
    {
        internal static SignInResponse ToResponse(this SignInResult result, string authToken = "")
        {
            return new SignInResponse
            {
                Auth = authToken ?? string.Empty,
                Cellphone = result.Cellphone,
                Lock = result.Lock,
                RemainCount = result.RemainCount,
                Success = result.Success,
                UserExist = result.UserExist,
                UserId = result.UserId
            };
        }
    }
}
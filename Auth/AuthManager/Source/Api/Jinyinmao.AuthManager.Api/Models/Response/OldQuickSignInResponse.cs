using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Moe.Lib;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class OldSignInResponse.
    /// </summary>
    public class OldQuickSignInResponse
    {
        /// <summary>
        ///     登录认证cookie值
        /// </summary>
        [Required]
        [JsonProperty("auth")]
        public string Auth { get; set; }

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
        ///     登陆结果
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
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }

    internal static class OldQuickSignInResponseEx
    {
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public static OldQuickSignInResponse ToSignInResponse(this UserInfo userInfo, string auth = null)
        {
            return new OldQuickSignInResponse
            {
                Auth = auth,
                Lock = 9 < userInfo.PasswordErrorCount,
                RemainCount = 10 - userInfo.PasswordErrorCount,
                Success = userInfo != null,
                UserExist = userInfo.UserId.ToGuidString().IsNotNullOrEmpty(),
                UserIdentifier = userInfo.UserId.ToGuidString()
            };
        }
    }
}
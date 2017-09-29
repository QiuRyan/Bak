using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Moe.Lib;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class SignUpResponse.
    /// </summary>
    public class OldQuickSignUpResponse
    {
        /// <summary>
        ///     登录认证cookie值
        /// </summary>
        [Required]
        [JsonProperty("auth")]
        public string Auth { get; set; }

        /// <summary>
        ///     用户的手机号码
        /// </summary>
        /// <value>The cellphone.</value>
        [Required]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        /// <value>The user identifier.</value>
        [Required]
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }

    internal static class OldQuickSignUpResponseEx
    {
        internal static OldQuickSignUpResponse ToSignUpResponse(this UserInfo userInfo, string auth)
        {
            return new OldQuickSignUpResponse
            {
                Auth = auth,
                Cellphone = userInfo.Cellphone,
                UserIdentifier = userInfo.UserId.ToGuidString()
            };
        }
    }
}
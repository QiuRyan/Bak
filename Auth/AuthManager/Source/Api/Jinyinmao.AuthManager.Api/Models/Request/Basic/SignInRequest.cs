using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class SignInRequest.
    /// </summary>
    public class SignInRequest
    {
        /// <summary>
        ///     用户登录名
        /// </summary>
        [Required]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        [JsonProperty("loginName")]
        public string LoginName { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        [Required]
        [StringLength(18, MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z\d~!@#$%^&*_]{6,18}$")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
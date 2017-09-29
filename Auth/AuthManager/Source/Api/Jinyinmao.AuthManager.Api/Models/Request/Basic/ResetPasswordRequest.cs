using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     ResetPasswordRequest.
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        ///     用户设置的密码
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z\d~!@#$%^&*_]{6,18}$")]
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        ///     验证码口令
        /// </summary>
        /// <value>The token.</value>
        [Required, StringLength(32, MinimumLength = 32)]
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
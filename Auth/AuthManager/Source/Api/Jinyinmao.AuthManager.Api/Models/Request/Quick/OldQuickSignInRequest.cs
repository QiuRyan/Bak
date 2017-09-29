using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class OldSignUpRequest.
    /// </summary>
    public class OldQuickSignInRequest
    {
        /// <summary>
        ///     用户手机号码
        /// </summary>
        /// <value>The cellphone.</value>
        [Required]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     验证码，用于验证，最短6位
        /// </summary>
        /// <value>The code.</value>
        [Required]
        [MinLength(6)]
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
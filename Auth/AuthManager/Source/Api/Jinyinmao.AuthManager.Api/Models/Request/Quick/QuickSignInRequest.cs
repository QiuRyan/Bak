using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class QuickSignInRequest.
    /// </summary>
    public class QuickSignInRequest
    {
        /// <summary>
        ///     验证码令牌
        /// </summary>
        /// <value>The token.</value>
        [JsonProperty("token")]
        [Required]
        public string Token { get; set; }
    }
}
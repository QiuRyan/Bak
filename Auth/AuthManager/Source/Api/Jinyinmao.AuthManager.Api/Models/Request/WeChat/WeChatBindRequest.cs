using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class WeChatBindRequest.
    /// </summary>
    public class WeChatBindRequest
    {
        /// <summary>
        ///     微信OpenId.
        /// </summary>
        [Required]
        [JsonProperty("openId")]
        public string OpenId { get; set; }

        /// <summary>
        ///     验证码口令.
        /// </summary>
        [Required]
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
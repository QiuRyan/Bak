using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     TokenResponse.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        ///     用户认证的Authorization Token.
        /// </summary>
        [Required]
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        ///     过期时间 UnixTimestamp.
        /// </summary>
        [Required]
        [JsonProperty("expiration")]
        public long Expiration { get; set; }
    }
}
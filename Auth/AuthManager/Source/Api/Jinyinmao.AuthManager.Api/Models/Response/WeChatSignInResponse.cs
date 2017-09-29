using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class WeChatSignInResponse.
    /// </summary>
    public class WeChatSignInResponse
    {
        /// <summary>
        ///     微信OpenId
        /// </summary>
        [Required]
        [JsonProperty("openId")]
        public string OpenId { get; set; }
    }
}
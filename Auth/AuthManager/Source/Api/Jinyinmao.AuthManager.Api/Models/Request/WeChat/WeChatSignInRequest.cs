using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class WeChatSignIn.
    /// </summary>
    public class WeChatSignInRequest
    {
        /// <summary>
        ///     微信授权Code.
        /// </summary>
        [Required]
        [JsonProperty("code")]
        public string Code { get; set; }

        ///// <summary>
        /////     微信OpenId.
        ///// </summary>
        //[Required]
        //[JsonProperty("openId")]
        //public string OpenId { get; set; }

        ///// <summary>
        /////     用户唯一标识.
        ///// </summary>
        //[Required]
        //[StringLength(32, MinimumLength = 32)]
        //[JsonProperty("userIdentifier")]
        //public string UserIdentifier { get; set; }
    }
}
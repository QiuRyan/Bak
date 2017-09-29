using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class UnbindRequest.
    /// </summary>
    public class UnbindRequest
    {
        /// <summary>
        ///     用户唯一标识.
        /// </summary>
        /// <value>The user identifier.</value>
        [Required]
        [StringLength(32, MinimumLength = 32)]
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}
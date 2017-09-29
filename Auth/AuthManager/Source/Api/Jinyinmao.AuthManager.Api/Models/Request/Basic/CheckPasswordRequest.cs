using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class CheckPasswordRequest.
    /// </summary>
    public class CheckPasswordRequest
    {
        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [JsonProperty("password")]
        [StringLength(18, MinimumLength = 6)]
        [RegularExpression(@"^(?![^a-zA-Z~!@#$%^&*_]+$)(?!\D+$).{8,18}$")]
        [Required]
        public string Password { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        [Required]
        [StringLength(32)]
        public string UserIdentifier { get; set; }
    }
}
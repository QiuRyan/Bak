using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class SetPasswodRequest.
    /// </summary>
    public class SetPasswodRequest
    {
        /// <summary>
        ///     密码.
        /// </summary>
        /// <value>The password.</value>
        [Required]
        [RegularExpression(@"^[a-zA-Z\d~!@#$%^&*_]{6,18}$")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
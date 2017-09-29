using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class CheckPasswordResponse.
    /// </summary>
    public class CheckPasswordResponse
    {
        /// <summary>
        ///     密码是否正确
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        [Required]
        [JsonProperty("result")]
        public bool Result { get; set; }
    }
}
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class SignUpUserIdRequest.
    /// </summary>
    public class SignUpUserIdRequest
    {
        /// <summary>
        ///     登录手机号
        /// </summary>
        [Required]
        [RegularExpression("^(13|14|15|16|17|18)\\d{9}$")]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }
    }
}
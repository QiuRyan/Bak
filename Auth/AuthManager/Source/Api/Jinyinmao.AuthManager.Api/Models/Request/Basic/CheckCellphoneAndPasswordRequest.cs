using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     CheckCellphoneAndPasswordRequest.
    /// </summary>
    public class CheckCellphoneAndPasswordRequest
    {
        /// <summary>
        ///     登录手机号
        /// </summary>
        [Required]
        [RegularExpression("^(13|14|15|16|17|18)\\d{9}$")]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     用户设置的登录密码（6-18位的数字、字母、一般特殊字符组合）
        /// </summary>
        [Required]
        [StringLength(18, MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z\d~!@#$%^&*_]{6,18}$")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
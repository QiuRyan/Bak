using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class ChangeLoginCellphoneRequest.
    /// </summary>
    public class ChangeLoginCellphoneRequest
    {
        /// <summary>
        ///     当前登录手机号.
        /// </summary>
        [Required, RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        [JsonProperty("loginCellphone")]
        public string LoginCellphone { get; set; }

        /// <summary>
        ///     更改手机号.
        /// </summary>
        [Required, RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        [JsonProperty("newCellphone")]
        public string NewCellphone { get; set; }
    }
}
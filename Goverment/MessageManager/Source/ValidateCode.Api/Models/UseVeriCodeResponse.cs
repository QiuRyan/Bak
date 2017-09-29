using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jinyinmao.ValidateCode.Api.Models
{
    /// <summary>
    ///     使用验证码Token请求.
    /// </summary>
    public class UseVeriCodeResponse
    {
        /// <summary>
        ///     手机号码
        /// </summary>
        /// <returns></returns>
        [Required, JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     结果
        /// </summary>
        [Required, JsonProperty("result")]
        public bool Result { get; set; }
    }
}
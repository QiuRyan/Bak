using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jinyinmao.Application.ViewModel.ValicodeManager
{
    /// <summary>
    ///     UseVeriCodeResult.
    /// </summary>
    public class UseVeriCodeResult
    {
        /// <summary>
        ///     手机号码，验证使用正则表达式：^(13|14|15|16|17|18)\d{9}$
        /// </summary>
        /// <returns></returns>
        [JsonProperty("cellphone")]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     结果
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        [JsonProperty("result")]
        public bool Result { get; set; }
    }
}
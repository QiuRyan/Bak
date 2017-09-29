using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Models
{
    /// <summary>
    ///     BoolResponse.
    /// </summary>
    public class BoolResponse
    {
        /// <summary>
        ///     bool 型结果
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        [Required]
        [JsonProperty("result")]
        public bool Result { get; set; }
    }
}
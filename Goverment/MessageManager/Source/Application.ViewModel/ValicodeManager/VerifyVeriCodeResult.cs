using Newtonsoft.Json;

namespace Jinyinmao.Application.ViewModel.ValicodeManager
{
    /// <summary>
    ///     VerifyVeriCodeResult.
    /// </summary>
    public class VerifyVeriCodeResult
    {
        /// <summary>
        ///     剩余次数
        /// </summary>
        /// <value>The remain count.</value>
        [JsonProperty("remaincount")]
        public int RemainCount { get; set; }

        /// <summary>
        ///     是否成功
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        ///     验签
        /// </summary>
        /// <value>The token.</value>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
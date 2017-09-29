using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Service.User.Response
{
    /// <summary>
    ///     StringResponse.
    /// </summary>
    public class StringResponse
    {
        /// <summary>
        ///     string 型结果
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
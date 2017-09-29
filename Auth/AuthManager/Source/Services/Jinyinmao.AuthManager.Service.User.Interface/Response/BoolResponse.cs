using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Service.User.Response
{
    /// <summary>
    ///     BoolResponse.
    /// </summary>
    public class BoolResponse
    {
        /// <summary>
        ///     bool 型结果
        /// </summary>
        [JsonProperty("result")]
        public bool Result { get; set; }
    }
}
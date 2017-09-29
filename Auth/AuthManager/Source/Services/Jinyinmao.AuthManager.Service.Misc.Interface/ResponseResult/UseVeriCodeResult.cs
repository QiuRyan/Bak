using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Service.Misc.Interface.ResponseResult
{
    /// <summary>
    ///     Class UseVeriCodeResult.
    /// </summary>
    public class UseVeriCodeResult
    {
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }
    }
}
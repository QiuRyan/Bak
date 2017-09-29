using Jinyinmao.Application.ViewModel.ValicodeManager;
using Newtonsoft.Json;

namespace Jinyinmao.ValidateCode.Api.Models
{
    /// <summary>
    ///     发送验证码的请求响应.
    /// </summary>
    public class SendVeriCodeResponse
    {
        /// <summary>
        ///     今天剩余发送次数，若为-1，则今天不能再次发送该类型验证码
        /// </summary>
        /// <returns></returns>
        [JsonProperty("remainCount")]
        public int RemainCount { get; set; }

        /// <summary>
        ///     本次发送结果
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    internal static class SendVeriCodeResultEx
    {
        internal static SendVeriCodeResponse ToResponse(this SendVeriCodeResult result)
        {
            return new SendVeriCodeResponse
            {
                RemainCount = result.RemainCount,
                Success = result.Success
            };
        }
    }
}
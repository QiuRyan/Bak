using System.ComponentModel.DataAnnotations;
using Jinyinmao.Application.Constants;
using Newtonsoft.Json;

namespace Jinyinmao.Application.ViewModel.MessageWorker
{
    /// <summary>
    ///     SmsGatewayRequest.
    /// </summary>
    public class SmsGatewayRequest
    {
        /// <summary>
        ///     是否启用
        /// </summary>
        [Required]
        [JsonProperty("enable")]
        public bool Enable { get; set; }

        /// <summary>
        ///     网关类型 创蓝,助通,客信通
        /// </summary>
        [Required]
        [JsonProperty("smsGateWay")]
        public SmsGateway SmsGateWay { get; set; }
    }
}
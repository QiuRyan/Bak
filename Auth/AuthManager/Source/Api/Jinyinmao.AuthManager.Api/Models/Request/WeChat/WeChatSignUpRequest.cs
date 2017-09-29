using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class WeChatSignUpRequest.
    /// </summary>
    public class WeChatSignUpRequest
    {
        /// <summary>
        ///     客户端标识, 900 => PC, 901 => iPhone, 902 => Android, 903 => M
        /// </summary>
        [JsonProperty("clientType")]
        public long? ClientType { get; set; }

        /// <summary>
        ///     活动编号(推广相关)
        /// </summary>
        [JsonProperty("contractId")]
        public long? ContractId { get; set; }

        /// <summary>
        ///     额外信息
        /// </summary>
        [JsonProperty("info")]
        public Dictionary<string, object> Info { get; set; }

        /// <summary>
        ///     邀请码(推广相关)
        /// </summary>
        [JsonProperty("inviteBy")]
        public string InviteBy { get; set; }

        /// <summary>
        ///     微信OpenId.
        /// </summary>
        [Required]
        [JsonProperty("openId")]
        public string OpenId { get; set; }

        /// <summary>
        ///     金银e家代码
        /// </summary>
        [JsonProperty("outletCode")]
        public string OutletCode { get; set; }

        /// <summary>
        ///     验证码口令.
        /// </summary>
        [Required]
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
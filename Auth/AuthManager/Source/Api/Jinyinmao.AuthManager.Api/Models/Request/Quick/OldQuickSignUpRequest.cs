using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request
{
    /// <summary>
    ///     Class OldQuickSignUpRequest.
    /// </summary>
    public class OldQuickSignUpRequest
    {
        /// <summary>
        ///     手机号码
        /// </summary>
        /// <value>The token.</value>
        [Required]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     客户端标识, 900 =&gt; PC, 901 =&gt; iPhone, 902 =&gt; Android, 903 =&gt; M
        /// </summary>
        /// <value>The type of the client.</value>
        [JsonProperty("clientType")]
        public long? ClientType { get; set; }

        /// <summary>
        ///     验证码，用于验证，最短6位
        /// </summary>
        /// <value>The code.</value>
        [Required]
        [MinLength(6)]
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        ///     活动编号(推广相关)
        /// </summary>
        /// <value>The contract identifier.</value>
        [JsonProperty("contractId")]
        public long? ContractId { get; set; }

        /// <summary>
        ///     邀请码(推广相关)
        /// </summary>
        /// <value>The invite by.</value>
        [JsonProperty("inviteBy")]
        public string InviteBy { get; set; }

        /// <summary>
        ///     金银e家代码
        /// </summary>
        /// <value>The outlet code.</value>
        [JsonProperty("outletCode")]
        public string OutletCode { get; set; }
    }
}
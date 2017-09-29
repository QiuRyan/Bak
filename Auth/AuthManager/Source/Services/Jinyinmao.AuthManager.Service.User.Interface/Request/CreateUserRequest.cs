using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Service.User.Interface.Request
{
    public class CreateUserRequest
    {
        /// <summary>
        ///     用户手机号码.
        /// </summary>
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     客户端标识, 900 =&gt; PC, 901 =&gt; iPhone, 902 =&gt; Android, 903 =&gt; M
        /// </summary>
        [JsonProperty("clientType")]
        public long? ClientType { get; set; }

        /// <summary>
        ///     活动编号(推广相关)
        /// </summary>
        [JsonProperty("contractId")]
        public long? ContractId { get; set; }

        /// <summary>
        ///     邀请码(推广相关)
        /// </summary>
        [JsonProperty("inviteBy")]
        public string InviteBy { get; set; }

        /// <summary>
        ///     金银e家代码
        /// </summary>
        [JsonProperty("outletCode")]
        public string OutletCode { get; set; }

        /// <summary>
        ///     用户设置的密码（6-18位的数字、字母、一般特殊字符组合）
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Request.Basic
{
    /// <summary>
    ///     Class UserRegisterRequest.
    /// </summary>
    public class UserRegisterRequest
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        [JsonProperty("cellphone")]
        [RegularExpression(@"^(13|14|15|16|17|18)\d{9}$")]
        [Required]
        public string Cellphone { get; set; }

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
        [Required]
        [StringLength(18, MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z\d~!@#$%^&*_]{6,18}$")]
        [JsonProperty("password")]
        public string Password { get; set; }

        //[Required]
        //[JsonProperty("userIdentifier")]
        ///// <value>The user identifier.</value>
        ///// </summary>
        /////     Gets or sets the user identifier.

        ///// <summary>
        //public string UserIdentifier { get; set; }
    }
}
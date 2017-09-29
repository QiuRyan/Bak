using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace jinyinmao.Signature.Service.Model
{
    /// <summary>
    ///     Class ArchiveRequest.
    /// </summary>
    public class ArchiveRequest
    {
        /// <summary>
        ///     key: basicInfo accountInfo investInfo
        /// </summary>
        /// <value>The key.</value>
        [Required]
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// </summary>
        /// <value>The value list.</value>
        [JsonProperty("valueList")]
        public List<string> ValueList { get; set; }
    }

    public class YemAgreementInfo
    {
        /// <summary>
        ///     协议下载URL
        /// </summary>
        /// <value>The jk agreement down URL.</value>
        public string AgreementDownUrl { get; set; }

        /// <summary>
        ///     协议查看URL
        /// </summary>
        /// <value>The jk agreement view URL.</value>
        public string AgreementViewUrl { get; set; }

        /// <summary>
        ///     法大大默认账户名称
        /// </summary>
        /// <value>The FDD account identifier.</value>
        public string FddAccountId { get; set; }

        /// <summary>
        ///     Gets or sets the order indentifier.
        /// </summary>
        /// <value>The order indentifier.</value>
        public string OrderIndentifier { get; set; }

        /// <summary>
        ///     Gets or sets the user indentifier.
        /// </summary>
        /// <value>The user indentifier.</value>
        public string UserIndentifier { get; set; }
    }

    public class YEMOrderUserInfo
    {
        /// <summary>
        ///     手机号码
        /// </summary>
        /// <value>The cellphone.</value>
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     证件类型。10 =&gt; 身份证， 20 =&gt; 护照，30 =&gt; 台湾， 40 =&gt; 军官证 ,
        /// </summary>
        /// <value>The credential.</value>
        [JsonProperty("credential")]
        public string Credential { get; set; }

        /// <summary>
        ///     证件号码
        /// </summary>
        /// <value>The credential no.</value>
        [JsonProperty("credentialNo")]
        public string CredentialNo { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        /// <value>The name of the real.</value>
        [JsonProperty("realName")]
        public string RealName { get; set; }

        /// <summary>
        ///     用户唯一编码
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        public string UserId { get; set; }
    }
}
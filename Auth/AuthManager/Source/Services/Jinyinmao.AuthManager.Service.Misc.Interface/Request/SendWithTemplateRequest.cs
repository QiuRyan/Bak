using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jinyinmao.AuthManager.Service.Misc.Request
{
    public class SendWithTemplateRequest
    {
        [JsonProperty("bizCode")]
        public string BizCode { get; set; }

        [JsonProperty("channel")]
        public int Channel { get; set; }

        [JsonProperty("templateParams")]
        public Dictionary<string, string> TemplateParams { get; set; }

        [JsonProperty("userInfoList")]
        public List<UserInfo> UserInfoList { get; set; }
    }

    public class UserInfo
    {
        [JsonProperty("phoneNum")]
        public string PhoneNum { get; set; }
    }
}
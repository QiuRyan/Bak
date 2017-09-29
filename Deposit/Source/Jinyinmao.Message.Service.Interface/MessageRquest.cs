using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jinyinmao.Message.Service.Interface
{
    public class BaseMessageRquest
    {
        [JsonProperty("bizCode")]
        public string BizCode { get; set; }

        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        [JsonProperty("args")]
        public Dictionary<string, string> Content { get; set; }
    }

    /// <summary>
    /// 短信发输出
    /// </summary>
    public class MessageResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    /// <summary>
    /// 短信发送请求
    /// </summary>
    /// <seealso cref="Jinyinmao.Message.Service.Interface.BaseMessageRquest"/>
    public class MessageRquest : BaseMessageRquest
    {
    }

    public class SmsMessageSenderRequest
    {
        public SmsMessageSenderRequest(string cellphone, string bizCode, Dictionary<string, string> args = null)
        {
            this.UserInfoList = new List<UserInfo>
            {
                new UserInfo { PhoneNum = cellphone }
            };

            this.BizCode = bizCode;
            this.TemplateParams = args ?? new Dictionary<string, string>();
        }

        [JsonProperty("bizCode")]
        public string BizCode { get; set; }

        [JsonProperty("channel")]
        public string Channel => "100002";

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
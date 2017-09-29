using System.Collections.Generic;
using System.Threading.Tasks;
using MoeLib.Diagnostics;

namespace Jinyinmao.MessageManager.Services
{
    public abstract class SmsServiceBase : ISmsService
    {
        public abstract Task<MessageManagerResponse> SendAsync(DispatcherRequest request, TraceEntry traceEntry);

        public Task<MessageManagerResponse> SendVeriCodeAsync(string bizCode, string cellphone, string validateCode, TraceEntry traceEntry)
        {
            return this.SendAsync(BuildDispatcherRequest(bizCode, cellphone, validateCode), traceEntry);
        }

        private static DispatcherRequest BuildDispatcherRequest(string bizCode, string cellphone, string validateCode)
        {
            return new DispatcherRequest
            {
                BizCode = bizCode,
                TemplateParams = new Dictionary<string, string>
                {
                    { "ValidateCode", validateCode },
                    { "Minute", "30" }
                },
                UserInfoList = new List<UserInfo>
                {
                    new UserInfo { PhoneNum = cellphone }
                }
            };
        }
    }
}
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Jinyinmao.Deposit.Config;
using Jinyinmao.Deposit.Lib;
using Jinyinmao.Message.Service.Interface;
using Moe.Lib;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;

namespace Jinyinmao.Message.Service
{
    public class MessageService : IMessageService
    {
        private readonly Lazy<HttpClient> smsClient = new Lazy<HttpClient>(() => JYMInternalHttpClientFactory.Create(ConfigManager.MessageServiceRole, (TraceEntry)null));

        #region IMessageService Members

        /// <summary>
        ///     定期还款通知
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> SendRegularRepaySuccessMessageAsync(SmsMessageSenderRequest request)
        {
            string orderNo = request.TemplateParams["OrderNo"];
            if (RedisHelper.KeyExists($"OrderRepay:{orderNo}"))
            {
                return await Task.FromResult(true);
            }

            HttpResponseMessage responseMessage = await this.smsClient.Value.PostAsJsonAsync("/api/MessageManager/SendWithTemplate", request);
            bool result = (await responseMessage.Content.ReadAsAsync<MessageResponse>()).Status == 0;
            if (result)
            {
                RedisHelper.SetStringValue($"OrderRepay:{orderNo}", request.ToJson(), TimeSpan.FromDays(1));
            }

            return result;
        }

        #endregion IMessageService Members
    }
}
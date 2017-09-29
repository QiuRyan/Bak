// ***********************************************************************
// Project          : MessageManager
// File             : SmsService.cs
// Created          : 2015-12-12  21:51
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-12  21:57
// ***********************************************************************
// <copyright file="SmsService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Net.Http;
using System.Threading.Tasks;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;

namespace Jinyinmao.MessageManager.Services
{
    public sealed class SmsService : SmsServiceBase
    {
        private readonly string messageTemplatesSendUri;
        private readonly string serviceRoleName;

        public SmsService(string messageTemplatesSendUri, string serviceRoleName)
        {
            this.messageTemplatesSendUri = messageTemplatesSendUri;
            this.serviceRoleName = serviceRoleName;
        }

        public override async Task<MessageManagerResponse> SendAsync(DispatcherRequest request, TraceEntry traceEntry)
        {
            using (HttpClient client = JYMInternalHttpClientFactory.Create(this.serviceRoleName, traceEntry))
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(this.messageTemplatesSendUri, request);
                return await response.Content.ReadAsAsync<MessageManagerResponse>();
            }
        }
    }
}
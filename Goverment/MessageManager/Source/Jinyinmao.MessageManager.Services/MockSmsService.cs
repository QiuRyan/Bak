using System;
using System.Threading.Tasks;
using MoeLib.Diagnostics;

namespace Jinyinmao.MessageManager.Services
{
    public class MockSmsService : SmsServiceBase
    {
        public override Task<MessageManagerResponse> SendAsync(DispatcherRequest request, TraceEntry traceEntry)
        {
            Random r = new Random();
            int randomValue = r.Next(0, 100);
            return Task.FromResult(randomValue > 30 ? new MessageManagerResponse { Status = 0L } : new MessageManagerResponse { Status = 1L });
        }
    }
}
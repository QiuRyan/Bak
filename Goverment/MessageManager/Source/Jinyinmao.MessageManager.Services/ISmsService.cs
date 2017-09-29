using System.Threading.Tasks;
using MoeLib.Diagnostics;

namespace Jinyinmao.MessageManager.Services
{
    /// <summary>
    ///     ISmsService.
    /// </summary>
    public interface ISmsService
    {
        Task<MessageManagerResponse> SendAsync(DispatcherRequest request, TraceEntry traceEntry);

        Task<MessageManagerResponse> SendVeriCodeAsync(string bizCode, string cellphone, string validateCode, TraceEntry traceEntry);
    }
}
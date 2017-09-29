using System.Threading.Tasks;

namespace Jinyinmao.Message.Service.Interface
{
    public interface IMessageService
    {
        Task<bool> SendRegularRepaySuccessMessageAsync(SmsMessageSenderRequest request);
    }
}
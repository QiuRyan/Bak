using MessageWorker.Domain.Entity;
using System.Threading.Tasks;

namespace MessageWorker.Domain.Bll.Impl
{
    public interface ISmsMessageService
    {
        Task<SmsMessage> CreateAsync(SmsMessage smsMessage);
    }
}
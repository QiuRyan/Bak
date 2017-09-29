using Microsoft.WindowsAzure.Storage;

namespace Jinyinmao.MessageWorker.Domain.Bll.Config
{
    internal interface IConfigProvider
    {
        string ClSendMessageUrl { get; }
        string ClSmsServicePassword { get; }
        string ClSmsServiceUserName { get; }
        bool SmsEnable { get; }
        CloudStorageAccount StorageAccount { get; }
        string ZtSendMessageUrl { get; }
        string ZtSmsServicePassword { get; }
        string ZtSmsServiceUserName { get; }

        void CheckConfig();

        void InitConfig();
    }
}
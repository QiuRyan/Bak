#if DEBUG
#else

using Moe.Lib.Jinyinmao;

#endif

namespace Jinyinmao.MessageManager.Api.Config
{
    public static class ConfigsManager
    {
        public static string MessageManagerDbContext
        {
            get
            {
#if DEBUG
                return "Data Source=10.1.25.30;Database=jym-message;Integrated Security=False;User ID=db-admin-dev;Password=0SmDXp8i7MRfg29HJk1N;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
#else
                return App.Condigurations.GetConfig<MessageManagerApiConfig>().MessageManagerDbContext;
#endif
            }
        }

        public static string ServiceBusConnectionString
        {
            get
            {
#if DEBUG
                return "Endpoint=sb://jym-dev.servicebus.chinacloudapi.cn/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LYI30TEnoYOz8C4jNK8j9+MS4kSkFsyi9DA78IRzKHA=";
#else
                return App.Condigurations.GetConfig<MessageManagerApiConfig>().ServiceBusConnectionString;
#endif
            }
        }

        public static string MessageQueueName
        {
            get
            {
#if DEBUG
                return "jym-messages-messagequeue";
#else
                return App.Condigurations.GetConfig<MessageManagerApiConfig>().MessageQueueName;
#endif
            }
        }
    }
}
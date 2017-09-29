// ***********************************************************************
// Project          : Jinyinmao.MessageManager
// File             : ConfigsManager.cs
// Created          : 2015-12-13  17:25
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-13  18:14
// ***********************************************************************
// <copyright file="ConfigsManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Microsoft.WindowsAzure.Storage;

#if DEBUG
#else

using Moe.Lib.Jinyinmao;

#endif

namespace jinyinmao.MessageWorker.Config
{
    public static class ConfigsManager
    {
        public static string MessageQueueName
        {
            get
            {
#if DEBUG
                return "jym-messages-messagequeue";
#else
                return App.Condigurations.GetConfig<MessageWorkerConfig>().MessageQueueName;
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
                return App.Condigurations.GetConfig<MessageWorkerConfig>().ServiceBusConnectionString;
#endif
            }
        }

        public static string ClSendMessageUrl
        {
            get
            {
#if DEBUG
                return "http://222.73.117.158:80/msg/HttpBatchSendSM?";
#else
                return App.Condigurations.GetConfig<MessageWorkerConfig>().ClSendMessageUrl;
#endif
            }
        }

        public static string ClSmsServicePassword
        {
            get
            {
#if DEBUG
                return "Tch123456";
#else
                return App.Condigurations.GetConfig<MessageWorkerConfig>().ClSmsServicePassword;
#endif
            }
        }

        public static string ClSmsServiceUserName
        {
            get
            {
#if DEBUG
                return "jinyinmao";
#else
                return App.Condigurations.GetConfig<MessageWorkerConfig>().ClSmsServiceUserName;
#endif
            }
        }

        public static bool SmsEnable
        {
            get
            {
#if DEBUG
                return true;
#else
                return App.Condigurations.GetConfig<MessageWorkerConfig>().SmsEnable;
#endif
            }
        }

        public static string SmsMessageDbContext
        {
            get
            {
#if DEBUG
                return "Data Source=10.1.25.30;Database=jym-message;Integrated Security=False;User ID=db-admin-dev;Password=0SmDXp8i7MRfg29HJk1N;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
#else
                return App.Condigurations.GetConfig<MessageWorkerConfig>().SmsMessageDbContext;
#endif
            }
        }

        public static CloudStorageAccount StorageAccount
        {
            get
            {
#if DEBUG
                return CloudStorageAccount.Parse("BlobEndpoint=https://jymstoredev.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredev.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredev.table.core.chinacloudapi.cn/;AccountName=jymstoredev;AccountKey=1dCLRLeIeUlLAIBsS9rYdCyFg3UNU239MkwzNOj3BYbREOlnBmM4kfTPrgvKDhSmh6sRp2MdkEYJTv4Ht3fCcg==");
#else
                return CloudStorageAccount.Parse(App.Condigurations.GetConfig<MessageWorkerConfig>().StorageConnectionString);
#endif
            }
        }

        public static string ZtSendMessageUrl
        {
            get
            {
#if DEBUG
                return "http://www.ztsms.cn:8800/sendSms.do?";
#else
                return App.Condigurations.GetConfig<MessageWorkerConfig>().ZtSendMessageUrl;
#endif
            }
        }

        public static string ZtSmsServicePassword
        {
            get
            {
#if DEBUG
                return "DRTkGfh9";
#else
                return App.Condigurations.GetConfig<MessageWorkerConfig>().ZtSmsServicePassword;
#endif
            }
        }

        public static string ZtSmsServiceUserName
        {
            get
            {
#if DEBUG
                return "jymao";
#else
                return App.Condigurations.GetConfig<MessageWorkerConfig>().ZtSmsServiceUserName;
#endif
            }
        }
    }
}
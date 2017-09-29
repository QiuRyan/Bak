using System;
using System.Diagnostics;
using System.Threading.Tasks;
using jinyinmao.MessageWorker.Config;
using Jinyinmao.Application.ViewModel.MessageManager;
using Jinyinmao.MessageWorker.Domain.Bll;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Moe.Lib;
using Moe.Lib.Jinyinmao;

namespace Jinyinmao.MessageWorkerHost
{
    internal class Program
    {
        private static readonly string messageQueueName = ConfigsManager.MessageQueueName;
        private static readonly string serviceBusConnectionString = ConfigsManager.ServiceBusConnectionString;
        private static QueueClient client;

        private static async Task DoWorkAsync(MessageModel request)
        {
            //接收的请求===如果渠道没有传入相应的数据,则将按照我们默认启动的渠道进行发送,否则就按照请求的渠道和业务进行发送
            foreach (ISmsService service in SmsServiceFactory.GetAllSmsServiceByChannel(request.Channel, request.Gateway))
            {
                await service.SendMessageAsync(request.Args, request.Cellphone, request.Message, request.Signature);
            }
        }

        private static void Main(string[] args)
        {
            // 如果队列不存在，则创建队列
            try
            {
                App.Initialize().Config().UseGovernmentServerConfigManager<MessageWorkerConfig>();
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
                if (!namespaceManager.QueueExists(messageQueueName))
                {
                    namespaceManager.CreateQueue(messageQueueName);
                }

                // 初始化与 Service Bus 队列的连接
                client = QueueClient.CreateFromConnectionString(serviceBusConnectionString, messageQueueName);

                // 启动消息泵，并且将为每个已收到的消息调用回调，在客户端上调用关闭将停止该泵。
                client.OnMessage(receivedMessage =>
                {
                    string strRequest = receivedMessage.GetBody<string>();
                    MessageModel request = strRequest.FromJson<MessageModel>();
                    DoWorkAsync(request).Wait();

                    // 处理消息
                    Trace.WriteLine("正在处理 Service Bus 消息: " + receivedMessage.SequenceNumber.ToString());
                });
                Console.WriteLine("按任意键结束...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
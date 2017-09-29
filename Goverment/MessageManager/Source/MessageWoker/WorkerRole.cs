// ***********************************************************************
// Project          : MessageManager
// File             : WorkerRole.cs
// Created          : 2015-12-12  21:51
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-12  21:54
// ***********************************************************************
// <copyright file="WorkerRole.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using jinyinmao.MessageWorker.Config;
using Jinyinmao.Application.ViewModel.MessageManager;
using Jinyinmao.MessageWorker.Domain.Bll;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using MoeLib.Jinyinmao.Azure;

namespace Jinyinmao.MessageWoker
{
    internal class WorkerRole : RoleEntryPoint
    {
        // 队列的名称

        private static readonly string messageQueueName = ConfigsManager.MessageQueueName;
        private static readonly string serviceBusConnectionString = ConfigsManager.ServiceBusConnectionString;

        private readonly ManualResetEvent completedEvent = new ManualResetEvent(false);

        // QueueClient 是线程安全的。建议你进行缓存，
        // 而不是针对每一个请求重新创建它
        private QueueClient client;

        public override bool OnStart()
        {
            App.Initialize().ConfigForAzure().UseGovernmentServerConfigManager<MessageWorkerConfig>();
            // 设置最大并发连接数
            ServicePointManager.DefaultConnectionLimit = 12;

            // 如果队列不存在，则创建队列
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            if (!namespaceManager.QueueExists(messageQueueName))
            {
                namespaceManager.CreateQueue(messageQueueName);
            }

            // 初始化与 Service Bus 队列的连接
            this.client = QueueClient.CreateFromConnectionString(serviceBusConnectionString, messageQueueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // 关闭与 Service Bus 队列的连接
            this.client.Close();
            this.completedEvent.Set();
            base.OnStop();
        }

        public override void Run()
        {
            Trace.WriteLine("正在开始处理消息");

            // 启动消息泵，并且将为每个已收到的消息调用回调，在客户端上调用关闭将停止该泵。
            this.client.OnMessage(receivedMessage =>
            {
                string strRequest = receivedMessage.GetBody<string>();
                MessageModel model = strRequest.FromJson<MessageModel>();
                if (model != null)
                {
                    DoWorkAsync(model).Wait();
                }
                // 处理消息
                Trace.WriteLine("正在处理 Service Bus 消息: " + receivedMessage.SequenceNumber.ToString());
            });

            this.completedEvent.WaitOne();
        }

        private static async Task DoWorkAsync(MessageModel model)
        {
            //接收的请求===如果渠道没有传入相应的数据,则将按照我们默认启动的渠道进行发送,否则就按照请求的渠道和业务进行发送
            foreach (ISmsService service in SmsServiceFactory.GetAllSmsServiceByChannel(model.Channel, model.Gateway))
            {
                await service.SendMessageAsync(model.Args, model.Cellphone, model.Message, model.Signature);
            }
        }
    }
}
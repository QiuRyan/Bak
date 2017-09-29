// ***********************************************************************
// Project          : MessageManager
// File             : WorkerRole.cs
// Created          : 2015-12-01  10:04
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-01  11:54
// ***********************************************************************
// <copyright file="WorkerRole.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WeChatWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        // 队列的名称
        private const string QueueName = "wechatqueue";

        private readonly ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        // QueueClient 是线程安全的。建议你进行缓存，
        // 而不是针对每一个请求重新创建它
        private QueueClient Client;

        public override bool OnStart()
        {
            // 设置最大并发连接数
            ServicePointManager.DefaultConnectionLimit = 12;

            // 如果队列不存在，则创建队列
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // 初始化与 Service Bus 队列的连接
            this.Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // 关闭与 Service Bus 队列的连接
            this.Client.Close();
            this.CompletedEvent.Set();
            base.OnStop();
        }

        public override void Run()
        {
            Trace.WriteLine("正在开始处理消息");

            // 启动消息泵，并且将为每个已收到的消息调用回调，在客户端上调用关闭将停止该泵。
            this.Client.OnMessage(receivedMessage =>
            {
                try
                {
                    // 处理消息
                    Trace.WriteLine("正在处理 Service Bus 消息: " + receivedMessage.SequenceNumber.ToString());
                }
                catch
                {
                    // 在此处处理任何处理特定异常的消息
                }
            });

            this.CompletedEvent.WaitOne();
        }
    }
}
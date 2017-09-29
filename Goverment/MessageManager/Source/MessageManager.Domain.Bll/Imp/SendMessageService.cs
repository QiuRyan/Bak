// ***********************************************************************
// Project          : MessageManager
// File             : SendMessageService.cs
// Created          : 2015-12-09  11:30
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-09  17:22
// ***********************************************************************
// <copyright file="SendMessageService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Jinyinmao.Application.ViewModel.MessageManager;
using Jinyinmao.MessageManager.Api.Config;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Moe.Lib;

namespace Jinyinmao.MessageManager.Domain.Bll
{
    /// <summary>
    ///     SendMessageService.
    /// </summary>
    public class SendMessageService : ISendMessageService
    {
        private static readonly string messageQueueName = ConfigsManager.MessageQueueName;
        private static readonly Lazy<MessageSender> messageSender = new Lazy<MessageSender>(() => InitMessageSender());
        private static readonly string serviceBusConnectionString = ConfigsManager.ServiceBusConnectionString;

        private MessageSender MessageSender
        {
            get { return messageSender.Value; }
        }

        #region ISendMessageService Members

        /// <summary>
        ///     Sends the message action.
        /// </summary>
        /// <param name="messageModels"></param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task SendMessageActionAsync(params MessageModel[] messageModels)
        {
            await messageModels.ForEach(async m => await this.MessageSender.SendAsync(new BrokeredMessage(m.ToJson())));
        }

        #endregion ISendMessageService Members

        private static MessageSender InitMessageSender()
        {
            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(serviceBusConnectionString);
            factory.RetryPolicy = RetryPolicy.Default;
            return factory.CreateMessageSender(messageQueueName);
        }
    }
}
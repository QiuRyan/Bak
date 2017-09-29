using System.Threading.Tasks;
using Jinyinmao.Deposit.Config;
using Jinyinmao.ServiceBus.Service.Interface;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Jinyinmao.ServiceBus.Service
{
    public class ServiceBusService : IServiceBusService
    {
        #region IServiceBusService Members

        /// <summary>
        ///     写入数据到队列
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        public async Task SendMessageToServiceBusAsync(string queueName, string message)
        {
            MessageSender messageSender = InitMessageSender(queueName);
            await messageSender.SendAsync(new BrokeredMessage(message));
        }


        /// <summary>
        ///  返利流水
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        public async Task SendRebateTransactionToEbibpbCenterAsync(string queueName, string message)
        { 
            MessageSender messageSender = InitMessageSender(queueName);
            await messageSender.SendAsync(new BrokeredMessage(message));
        }


        #endregion IServiceBusService Members

        public async Task SendMessageToDeadLetterAsync(string queueName, string message)
        {
            BrokeredMessage broker = new BrokeredMessage(message);
            await broker.DeadLetterAsync();
        }

        private static MessageSender InitMessageSender(string queueName)
        {
            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(ConfigManager.ServiceBusConnectionString);
            factory.RetryPolicy = RetryPolicy.Default;
            return factory.CreateMessageSender(queueName);
        }
    }
}
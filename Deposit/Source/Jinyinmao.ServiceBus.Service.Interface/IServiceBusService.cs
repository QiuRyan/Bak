using System.Threading.Tasks;

namespace Jinyinmao.ServiceBus.Service.Interface
{
    public interface IServiceBusService
    {
        /// <summary>
        ///     写入数据到队列
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task SendMessageToServiceBusAsync(string queueName, string message);

        /// <summary>
        ///     Signs the success transaction asynchronous.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task SendRebateTransactionToEbibpbCenterAsync(string queueName, string message);
    }
}
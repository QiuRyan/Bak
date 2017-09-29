using Jinyinmao.Application.Constants;
using System.Threading.Tasks;

namespace Jinyinmao.MessageWorker.Domain.Bll
{
    /// <summary>
    ///     Interface ISmsService
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// </summary>
        string ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the SMS gateway.
        /// </summary>
        /// <value>The name of the SMS gateway.</value>
        SmsGateway SmsGatewayName { get; }

        /// <summary>
        ///     Sends the message asynchronous.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cellphone"></param>
        /// <param name="message">The message.</param>
        /// <param name="signature">The signature.</param>
        /// <returns>Task.</returns>
        Task SendMessageAsync(string args, string cellphone, string message, string signature);
    }
}
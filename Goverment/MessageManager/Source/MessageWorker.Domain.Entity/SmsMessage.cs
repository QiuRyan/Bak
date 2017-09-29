using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jinyinmao.MessageWorker.Domain.Entity
{
    /// <summary>
    ///     SmsMessage.
    /// </summary>
    public class SmsMessage : TableEntity
    {
        /// <summary>
        ///     Gets or sets the application identifier.
        /// </summary>
        /// <value>The application identifier.</value>
        public string AppId { get; set; }

        /// <summary>
        ///     Gets or sets the cellphones.
        /// </summary>
        /// <value>The cellphones.</value>
        public string Cellphones { get; set; }

        /// <summary>
        ///     Gets or sets the gateway.
        /// </summary>
        /// <value>The gateway.</value>
        public int Gateway { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

        /// <summary>
        ///     Gets or sets the response.
        /// </summary>
        /// <value>The response.</value>
        public string Response { get; set; }

        /// <summary>
        ///     Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }
    }
}
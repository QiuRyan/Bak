using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Models
{
    public class JytTransactionResult
    {
        /// <summary>
        ///     Gets or sets the channel code.
        /// </summary>
        /// <value> The channel code.</value>
        [JsonProperty("channelCode")]
        public string ChannelCode { get; set; }

        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        [JsonProperty("transactionIdentifier")]
        public string TransactionIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}
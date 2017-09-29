using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Models
{
    public class MergeTransactionEntities
    {
        /// <summary>
        ///     流水编号
        /// </summary>
        [JsonProperty("transactionIdentifier")]
        public string TransactionIdentifier { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }

    /// <summary>
    /// </summary>
    public class PollMergeTransactionInfoLog
    {
        /// <summary>
        ///     Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        ///     Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public List<MergeTransactionEntities> Data { get; set; }

        /// <summary>
        ///     Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }
    }
}
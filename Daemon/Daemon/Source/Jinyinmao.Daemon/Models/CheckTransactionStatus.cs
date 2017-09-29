using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Models
{
    public class CheckTransactionStatus
    {
        [JsonProperty("notifytype")]
        public int Notifytype { get; set; }

        [JsonProperty("resultCode")]
        public int ResultCode { get; set; }

        [JsonProperty("transactionIdentifier")]
        public string TransactionIdentifier { get; set; }

        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}
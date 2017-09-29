using System;
using Newtonsoft.Json;

namespace jinyinmao.Signature.lib.Common
{
    public class YEMOrderInfo
    {
        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     订单类型 0 赎回订单 1 申购订单
        /// </summary>
        /// <value>The type of the order.</value>
        [JsonProperty("orderType")]
        public int OrderType { get; set; }

        /// <summary>
        ///     成功时间
        /// </summary>
        /// <value>The result time.</value>
        [JsonProperty("resultTime")]
        public DateTime ResultTime { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
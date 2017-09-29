using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Models
{
    /// <summary>
    /// Class OrderInfoRequest.
    /// </summary>
    public class OrderInfoRequest
    {
        /// <summary>
        /// 金额(单位:分)
        /// </summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// 业务类别(8001-返利，8003-红包)
        /// </summary>
        [JsonProperty("biztype")]
        public string BizType { get; set; }

        /// <summary>
        /// 出款方网贷平台唯一的用户编码
        /// </summary>
        [JsonProperty("payUserId")]
        public string PayUserIdentifier { get; set; }

        /// <summary>
        /// 子订单编号
        /// </summary>
        [JsonProperty("subOrderId")]
        public string TransactionIdentifier { get; set; }

        /// <summary>
        /// 收款方网贷平台唯一的用户编码
        /// </summary>
        [JsonProperty("receiveUserId")]
        public string UserIdentifier { get; set; }
    }

    public class RebateRequest
    {
        /// <summary>
        /// 币种(默认:CNY)
        /// </summary>
        /// <value>The currency.</value>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// 由网贷平台生成的唯一的交易流水号
        /// </summary>
        /// <value>The currency.</value>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 明细列表
        /// </summary>
        /// <value>The currency.</value>
        [JsonProperty("subOrderList")]
        public List<OrderInfoRequest> OrderList { get; set; }

        /// <summary>
        /// 返利渠道类型（01-签到返利，02-打赏返利，03-投资购买卡券返利，04-偏差补贴返利）
        /// </summary>
        /// <value>The type of the rebate.</value>
        [JsonProperty("rebateType")]
        public string RebateType { get; set; }
    }
}
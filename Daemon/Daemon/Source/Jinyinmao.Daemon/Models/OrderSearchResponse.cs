namespace Jinyinmao.Daemon.Models
{
    /// <summary>
    ///     Class OrderSearchResponse.
    /// </summary>
    public class OrderSearchResponse
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public long Amount { get; set; }

        /// <summary>
        ///     Gets or sets the type of the biz.
        /// </summary>
        /// <value>The type of the biz.</value>
        public string BizType { get; set; }

        /// <summary>
        ///     Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }

        /// <summary>
        ///     Gets or sets the merchant identifier.
        /// </summary>
        /// <value>The merchant identifier.</value>
        public string MerchantId { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId { get; set; }

        /// <summary>
        ///     Gets or sets the pay user identifier.
        /// </summary>
        /// <value>The pay user identifier.</value>
        public string PayUserId { get; set; }

        /// <summary>
        ///     Gets or sets the receive user identifier.
        /// </summary>
        /// <value>The receive user identifier.</value>
        public string ReceiveUserId { get; set; }

        /// <summary>
        ///     Gets or sets the resp code.
        /// </summary>
        /// <value>The resp code.</value>
        public string RespCode { get; set; }

        /// <summary>
        ///     Gets or sets the resp MSG.
        /// </summary>
        /// <value>The resp MSG.</value>
        public string RespMsg { get; set; }

        /// <summary>
        ///     Gets or sets the resp sub code.
        /// </summary>
        /// <value>The resp sub code.</value>
        public string RespSubCode { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }
    }
}
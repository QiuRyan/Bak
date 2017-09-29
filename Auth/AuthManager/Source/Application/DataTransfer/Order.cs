using System;
using System.Collections.Generic;

namespace DataTransfer
{
    /// <summary>
    ///     Order.
    /// </summary>
    public class Order
    {
        /// <summary>
        ///     Gets or sets the account transaction identifier.
        /// </summary>
        /// <value>The account transaction identifier.</value>
        public Guid AccountTransactionId { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the extra interest.
        /// </summary>
        /// <value>The extra interest.</value>
        public long ExtraInterest { get; set; }

        /// <summary>
        ///     Gets or sets the extra interest records.
        /// </summary>
        /// <value>The extra interest records.</value>
        public List<ExtraInterestRecord> ExtraInterestRecords { get; set; }

        /// <summary>
        ///     Gets or sets the extra yield.
        /// </summary>
        /// <value>The extra yield.</value>
        public int ExtraYield { get; set; }

        /// <summary>
        ///     Gets or sets the interest.
        /// </summary>
        /// <value>The interest.</value>
        public long Interest { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is repaid.
        /// </summary>
        /// <value><c>true</c> if this instance is repaid; otherwise, <c>false</c>.</value>
        public bool IsRepaid { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public Guid OrderId { get; set; }

        /// <summary>
        ///     Gets or sets the order no.
        /// </summary>
        /// <value>The order no.</value>
        public string OrderNo { get; set; }

        /// <summary>
        ///     Gets or sets the order time.
        /// </summary>
        /// <value>The order time.</value>
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     Gets or sets the principal.
        /// </summary>
        /// <value>The principal.</value>
        public long Principal { get; set; }

        /// <summary>
        ///     Gets or sets the product category.
        /// </summary>
        /// <value>The product category.</value>
        public long ProductCategory { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the product snapshot.
        /// </summary>
        /// <value>The product snapshot.</value>
        public RegularProductInfo ProductSnapshot { get; set; }

        /// <summary>
        ///     Gets or sets the repaid time.
        /// </summary>
        /// <value>The repaid time.</value>
        public DateTime? RepaidTime { get; set; }

        /// <summary>
        ///     Gets or sets the result code.
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode { get; set; }

        /// <summary>
        ///     Gets or sets the result time.
        /// </summary>
        /// <value>The result time.</value>
        public DateTime? ResultTime { get; set; }

        /// <summary>
        ///     Gets or sets the settle date.
        /// </summary>
        /// <value>The settle date.</value>
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        ///     Gets or sets the value date.
        /// </summary>
        /// <value>The value date.</value>
        public DateTime ValueDate { get; set; }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        public int Yield { get; set; }
    }
}
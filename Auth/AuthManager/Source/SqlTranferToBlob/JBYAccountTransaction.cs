using Moe.Lib;
using System;
using System.Collections.Generic;

namespace SqlTranferToBlob
{
    /// <summary>
    ///     JBYAccountTransaction.
    /// </summary>
    public class JBYAccountTransaction
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public long Amount { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the predetermined result date.
        /// </summary>
        /// <value>The predetermined result date.</value>
        public DateTime? PredeterminedResultDate { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductId { get; set; }

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
        ///     Gets or sets the settle account transaction identifier.
        /// </summary>
        /// <value>The settle account transaction identifier.</value>
        public Guid SettleAccountTransactionId { get; set; }

        /// <summary>
        ///     Gets or sets the trade.
        /// </summary>
        /// <value>The trade.</value>
        public Trade Trade { get; set; }

        /// <summary>
        ///     Gets or sets the trade code.
        /// </summary>
        /// <value>The trade code.</value>
        public int TradeCode { get; set; }

        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        public Guid TransactionId { get; set; }

        /// <summary>
        ///     Gets or sets the transaction time.
        /// </summary>
        /// <value>The transaction time.</value>
        public DateTime TransactionTime { get; set; }

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
        ///     Determines whether this instance is resulted.
        /// </summary>
        /// <returns><c>true</c> if this instance is resulted; otherwise, <c>false</c>.</returns>
        public bool IsCompleted()
        {
            return this.ResultCode > 0;
        }

        /// <summary>
        ///     Determines whether this instance is failed.
        /// </summary>
        /// <returns><c>true</c> if this instance is failed; otherwise, <c>false</c>.</returns>
        public bool IsFailed()
        {
            return this.ResultCode < 0;
        }

        /// <summary>
        ///     Determines whether [is not failed].
        /// </summary>
        /// <returns><c>true</c> if [is not failed]; otherwise, <c>false</c>.</returns>
        public bool IsNotFailed()
        {
            return !this.IsFailed();
        }

        /// <summary>
        ///     Determines whether this instance is processing.
        /// </summary>
        /// <returns>System.Boolean.</returns>
        public bool IsProcessing()
        {
            return this.ResultCode == 0;
        }

        /// <summary>
        ///     Determines whether [is today resulted].
        /// </summary>
        /// <returns><c>true</c> if [is today resulted]; otherwise, <c>false</c>.</returns>
        public bool IsResultedOnTheDay(DateTime theDate)
        {
            return this.ResultTime.GetValueOrDefault().Date == theDate;
        }

        /// <summary>
        ///     Determines whether [is today resulted].
        /// </summary>
        /// <returns><c>true</c> if [is today resulted]; otherwise, <c>false</c>.</returns>
        public bool IsTodayResulted()
        {
            return this.IsResultedOnTheDay(DateTime.UtcNow.ToChinaStandardTime().Date);
        }

        /// <summary>
        ///     Determines whether [is today transacted].
        /// </summary>
        /// <returns><c>true</c> if [is today transacted]; otherwise, <c>false</c>.</returns>
        public bool IsTodayTransacted()
        {
            return this.IsTransactedOnTheDay(DateTime.UtcNow.ToChinaStandardTime().Date);
        }

        /// <summary>
        ///     Determines whether [is transacted on the day] [the specified the date].
        /// </summary>
        /// <param name="theDate">The date.</param>
        /// <returns><c>true</c> if [is transacted on the day] [the specified the date]; otherwise, <c>false</c>.</returns>
        public bool IsTransactedOnTheDay(DateTime theDate)
        {
            return this.TransactionTime.Date == theDate;
        }
    }
}
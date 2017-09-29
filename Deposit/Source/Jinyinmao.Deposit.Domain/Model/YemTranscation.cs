// *********************************************************************** Project :
// Jinyinmao.Tirisfal File : YemTranscation.cs Created : 2016-05-17 10:40
//
// Last Modified By : 余忠宪 Last Modified On : 2016-05-30 11:31 ***********************************************************************
// <copyright file="YemTranscation.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Jinyinmao.Deposit.Domain
{
    /// <summary>
    /// YemTranscation.
    /// </summary>
    public class YemTransaction
    {
        /// <summary>
        /// Gets or sets the account transaction identifier.
        /// </summary>
        /// <value>The account transaction identifier.</value>
        public string AccountTransactionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public long Amount { get; set; }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Args { get; set; }

        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string Info { get; set; }

        /// <summary>
        ///     Gets or sets the is signature.
        /// </summary>
        /// <value>The is signature.</value>
        public bool IsSignature { get; set; }

        /// <summary>
        ///     Gets or sets the jby product identifier.
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the result time.
        /// </summary>
        /// <value>The result time.</value>
        public DateTime? ResultTime { get; set; }

        /// <summary>
        /// Gets or sets the trade code.
        /// </summary>
        /// <value>The trade code.</value>
        public int TradeCode { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        public string TransactionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the transaction time.
        /// </summary>
        /// <value>The transaction time.</value>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public string UserInfo { get; set; }

        /// <summary>
        /// Gets or sets the yem product identifier.
        /// </summary>
        /// <value>The yem product identifier.</value>
        public string YemProductIdentifier { get; set; }
    }
}
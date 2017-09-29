// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : PollXianFengPaymentEntities.cs
// Created          : 2015-08-29  16:13
//
// Last Modified By : 王兵
// Last Modified On : 2015-09-08  21:07
// ***********************************************************************
// <copyright file="PollXianFengPaymentEntities.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Jinyinmao.Daemon.Models
{
    /// <summary>
    ///     AccountTransactionResult.
    /// </summary>
    public class AccountTransactionResult
    {
        [JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }

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

        [JsonProperty("yiLianVerified")]
        public int YiLianVerified { get; set; }
    }

    public class PollJytPaymentInfoLog
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
        public List<JytTransactionResult> Data { get; set; }

        /// <summary>
        ///     Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }
    }

    /// <summary>
    ///     PollXianFengPaymentInfoLog.
    /// </summary>
    public class PollXianFengPaymentInfoLog
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
        public List<AccountTransactionResult> Data { get; set; }

        /// <summary>
        ///     Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }
    }

    public class XianFengRequest
    {
        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        [JsonProperty("transactionIdentifier")]
        public string transactionIdentifier { get; set; }

        [JsonProperty("userIdentifier")]
        public string userIdentifier { get; set; }
    }

    public class YiLianRequest
    {
        [JsonProperty("accNo")]
        public string accNo { get; set; }

        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        [JsonProperty("transactionIdentifier")]
        public string transactionIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        public string userIdentifier { get; set; }

        [JsonProperty("yiLianVerified")]
        public int yiLianVerified { get; set; }
    }
}
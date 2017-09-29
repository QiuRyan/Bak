// ***********************************************************************
// Project          : Jinyinmao.Daemon
// File             : InsertSettleAccountTransactionEntities.cs
// Created          : 2016-08-31  13:40
//
// Last Modified By : 杨亮
// Last Modified On : 2016-08-31  16:56
// ***********************************************************************
// <copyright file="InsertSettleAccountTransactionEntities.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Jinyinmao.Daemon.Models
{
    public class InsertSettleAccountTransactionEntities
    {
        /// <summary>
        ///     流水金额
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        ///     流水对应的银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     渠道号.0=>金银猫，10000=>金银猫，10010=>易连，10020=>连连，10030=>先锋，10040=>盛付通，0为默认值.
        /// </summary>
        /// <value>The channel code.</value>
        public int ChannelCode { get; set; }

        /// <summary>
        ///     流水对应的订单唯一标识
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        ///     流水状态.
        /// </summary>
        /// <value>The result.</value>
        public int? ResultCode { get; set; }

        /// <summary>
        ///     流水交易类型
        /// </summary>
        public int Trade { get; set; }

        /// <summary>
        ///     流水交易类型
        /// </summary>
        public int TradeCode { get; set; }

        /// <summary>
        ///     流水唯一标识.
        /// </summary>
        /// <value>The transaction identifier.</value>
        public string TransactionIdentifier { get; set; }

        /// <summary>
        ///     交易描述
        /// </summary>
        public string TransDesc { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        public string UserIdentifier { get; set; }
    }

    public class PollCallPaymentOrderSynchroLog
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
        public List<InsertSettleAccountTransactionEntities> Data { get; set; }

        /// <summary>
        ///     Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }
    }
}
// ***********************************************************************
// Project          : Jinyinmao.Daemon
// File             : CheckProductSaleStatusEntities.cs
// Created          : 2016-05-30  14:56
//
//
// Last Modified On : 2016-08-01  18:01
// ***********************************************************************
// <copyright file="CheckProductSaleStatusEntities.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Jinyinmao.Daemon.Models
{
    /// <summary>
    ///     PollCheckProductSaleStatusInfoLog.
    /// </summary>
    public class PollCheckProductSaleStatusInfoLog
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
        public List<RegularProductResult> Data { get; set; }

        /// <summary>
        /// </summary>
        public string ResultString { get; set; }

        /// <summary>
        ///     Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }
    }

    /// <summary>
    ///     RegularProductResult.
    /// </summary>
    public class RegularProductResult
    {
        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductIdentifier { get; set; }
    }
}
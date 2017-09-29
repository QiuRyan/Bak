// ******************************************************************************************************
// Project : Jinyinmao.Tirisfal File : YemProduct.cs Created : 2017-03-14 11:31
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-07-17 20:33 ******************************************************************************************************
// <copyright file="YemProduct.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;

namespace Jinyinmao.Deposit.Domain
{
    /// <summary>
    /// Class YemProduct.
    /// </summary>
    public class YemProduct
    {
        /// <summary>
        /// Gets or sets the end sell time.
        /// </summary>
        /// <value>The end sell time.</value>
        public DateTime EndSellTime { get; set; }

        /// <summary>
        /// Gets or sets the financing sum count.
        /// </summary>
        /// <value>The financing sum count.</value>
        public long FinancingSumAmount { get; set; }

        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string Info { get; set; }

        /// <summary>
        /// Gets or sets the issue no.
        /// </summary>
        /// <value>The issue no.</value>
        public int IssueNo { get; set; }

        /// <summary>
        /// Gets or sets the issue time.
        /// </summary>
        /// <value>The issue time.</value>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// Gets or sets the product category.
        /// </summary>
        /// <value>The product category.</value>
        public long ProductCategory { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the product no.
        /// </summary>
        /// <value>The product no.</value>
        public string ProductNo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [sold out].
        /// </summary>
        /// <value><c>true</c> if [sold out]; otherwise, <c>false</c>.</value>
        public bool SoldOut { get; set; }

        /// <summary>
        /// Gets or sets the sold out time.
        /// </summary>
        /// <value>The sold out time.</value>
        public DateTime? SoldOutTime { get; set; }

        /// <summary>
        /// Gets or sets the start sell time.
        /// </summary>
        /// <value>The start sell time.</value>
        public DateTime StartSellTime { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        /// <value>The unit price.</value>
        public long UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the value date mode.
        /// </summary>
        /// <value>The value date mode.</value>
        public int ValueDateMode { get; set; }

        /// <summary>
        /// Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        public int Yield { get; set; }
    }
}
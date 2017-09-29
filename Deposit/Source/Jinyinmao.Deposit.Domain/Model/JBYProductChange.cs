// ***********************************************************************
// Project          : Jinyinmao.Tirisfal
// File             : ProductChange.cs
// Created          : 2016-12-05  09:42
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-05  09:42
// ***********************************************************************
// <copyright file="ProductChange.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Jinyinmao.Deposit.Domain
{
    /// <summary>
    ///     Class ProductChange.
    /// </summary>
    public class JBYProductChange
    {
        /// <summary>
        ///     Gets or sets the asset identifier.
        /// </summary>
        /// <value>The asset identifier.</value>
        public string AssetInfo { get; set; }

        /// <summary>
        ///     Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string Info { get; set; }

        /// <summary>
        ///     Gets or sets the orgigin product information.
        /// </summary>
        /// <value>The orgigin product information.</value>
        public string OrgiginProductInfo { get; set; }

        /// <summary>
        ///     Gets or sets the product identifer.
        /// </summary>
        /// <value>The product identifer.</value>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the change code.
        /// </summary>
        /// <value>The change code.</value>
        public long TradeCode { get; set; }
    }
}
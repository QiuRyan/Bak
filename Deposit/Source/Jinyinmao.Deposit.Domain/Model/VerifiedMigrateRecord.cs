// ******************************************************************************************************
// Project : Jinyinmao.Tirisfal File : VerifiedMigrateRecord.cs Created : 2017-07-31 14:13
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-07-31 14:13 ******************************************************************************************************
// <copyright file="VerifiedMigrateRecord.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;

namespace Jinyinmao.Deposit.Domain
{
    /// <summary>
    /// Class VerifiedMigrateRecord.
    /// </summary>
    public class VerifiedMigrateRecord
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public long Amount { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the last update time.
        /// </summary>
        /// <value>The last update time.</value>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// Gets or sets the migrate time.
        /// </summary>
        /// <value>The migrate time.</value>
        public DateTime MigrateTime { get; set; }

        /// <summary>
        /// Gets or sets the record identfier.
        /// </summary>
        /// <value>The record identfier.</value>
        public string RecordIdentfier { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime VerifiedTime { get; set; }
    }
}
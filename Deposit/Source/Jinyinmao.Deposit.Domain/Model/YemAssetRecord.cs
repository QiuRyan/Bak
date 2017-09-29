// ******************************************************************************************************
// Project : Jinyinmao.Tirisfal File : YemAssetRecord.cs Created : 2017-06-06 11:52
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-07-17 19:22 ******************************************************************************************************
// <copyright file="YemAssetRecord.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using Newtonsoft.Json;

namespace Jinyinmao.Deposit.Domain
{
    /// <summary>
    ///     Class OffsetInfo.
    /// </summary>
    public class OffsetInfo
    {
        /// <summary>
        ///     偏差金额
        /// </summary>
        /// <value>The offet amount.</value>
        [JsonProperty("offetAmount")]
        public long OffetAmount
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the type of the offet.
        /// </summary>
        /// <value>The type of the offet.</value>
        [JsonProperty("offetType")]
        public int OffetType
        {
            get; set;
        }
    }

    /// <summary>
    ///     Class YemAssetRecord.
    /// </summary>
    public class YemAssetRecord
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public long Amount
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Args
        {
            get; set;
        }

        /// <summary>
        ///     资产ID
        /// </summary>
        /// <value>The asset identifier.</value>
        public string AssetIdentifier
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime
        {
            get; set;
        }

        /// <summary>
        ///     描述
        /// </summary>
        /// <value>The transaction amount.</value>
        public string Description
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string Info
        {
            get; set;
        }

        /// <summary>
        ///     是否已经生成协议
        /// </summary>
        /// <value>The is signature.</value>
        public bool IsSignature
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductIdentifier
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the result code.
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the result time.
        /// </summary>
        /// <value>The result time.</value>
        public DateTime? ResultTime
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the sequence no.
        /// </summary>
        /// <value>The sequence no.</value>
        public string SequenceNo
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the source yem asset record identifier.
        /// </summary>
        /// <value>The source yem asset record identifier.</value>
        public string SourceYemAssetRecordIdentifier
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the change code.
        /// </summary>
        /// <value>The change code.</value>
        public long TradeCode
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the transaction amount.
        /// </summary>
        /// <value>The transaction amount.</value>
        public long TransactionAmount
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public string UserInfo
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the yem order identifier.
        /// </summary>
        /// <value>The yem order identifier.</value>
        public string YemOrderIdentifier
        {
            get; set;
        }
    }
}
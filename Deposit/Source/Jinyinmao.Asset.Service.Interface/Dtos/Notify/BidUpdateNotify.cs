// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BidUpdateNotify.cs
// Created          : 2017-08-10  14:30
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  14:31
// ******************************************************************************************************
// <copyright file="BidUpdateNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Asset.Service.Interface.Dtos
{
    /// <summary>
    ///     标的修改
    /// </summary>
    public class BidUpdateNotify
    {
        /// <summary>
        ///     审核状态
        /// </summary>
        [JsonProperty("bankAuditStatus")]
        public string BankAuditStatus { get; set; }

        /// <summary>
        ///     标的ID
        /// </summary>
        [JsonProperty("bidId")]
        public string BidId { get; set; }

        /// <summary>
        ///     标的状态
        /// </summary>
        [JsonProperty("bidStatus")]
        public string BidStatus { get; set; }

        /// <summary>
        ///     由存管银行分配给网贷平台的唯一的商户编码
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        ///     网贷平台唯一请求流水号
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        ///     签名，对业务参数进行签名的结果
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : FinishCreateAccountNotify.cs
// Created          : 2017-08-10  13:56
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  14:00
// ******************************************************************************************************
// <copyright file="FinishCreateAccountNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     开户成功回调
    /// </summary>
    public class FinishCreateAccountNotify
    {
        /// <summary>
        ///     银行卡号
        /// </summary>
        [JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }

        [JsonProperty("bankCode")]
        public string BankCode { get; set; }

        /// <summary>
        ///     证件号码
        /// </summary>
        [JsonProperty("certNo")]
        public string CertNo { get; set; }

        /// <summary>
        ///     失败原因
        /// </summary>
        [JsonProperty("failReason")]
        public string FailReason { get; set; }

        /// <summary>
        ///     商户编号
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        ///     银行预留手机号
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>
        ///     姓名
        /// </summary>
        [JsonProperty("realName")]
        public string RealName { get; set; }

        /// <summary>
        ///     开户状态(S-开户成功;F-开户失败)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
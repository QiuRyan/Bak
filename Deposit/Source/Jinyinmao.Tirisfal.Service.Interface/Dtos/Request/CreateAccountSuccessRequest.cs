// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : CreateAccountSuccessRequest.cs
// Created          : 2017-08-10  13:55
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:56
// ******************************************************************************************************
// <copyright file="CreateAccountSuccessRequest.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    public class CreateAccountSuccessRequest
    {
        /// <summary>
        ///     手机号
        /// </summary>
        [JsonProperty("bankCardCellphone")]
        public string BankCardCellphone { get; set; }

        /// <summary>
        ///     银行卡号
        /// </summary>
        [JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行编码.
        /// </summary>
        /// <value>The bank code.</value>
        [JsonProperty("bankCode")]
        public string BankCode { get; set; }

        /// <summary>
        ///     用户唯一标示
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}
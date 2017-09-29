// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : RebateNotify.cs
// Created          : 2017-08-10  15:24
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  15:24
// ******************************************************************************************************
// <copyright file="RebateNotify.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using Jinyinmao.Deposit.Domain;
using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    /// <summary>
    ///     返利回调
    /// </summary>
    /// <seealso cref="Jinyinmao.Deposit.Domain.BasicBusinessNotify" />
    public class RebateNotify : BasicBusinessNotify
    {
        /// <summary>
        ///     返利渠道  01-签到返利，02-打赏返利，03-投资购买卡券返利，04-偏差补贴返利
        /// </summary>
        /// <value>The type of the rebate.</value>
        [JsonProperty("rebateType")]
        public string RebateType { get; set; }
    }
}
// ***********************************************************************
// Project          : MessageManager
// File             : SmsGateway.cs
// Created          : 2015-12-12  15:45
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-12  21:56
// ***********************************************************************
// <copyright file="SmsGateway.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Moe.Lib;
using System.Collections.Generic;
using System.ComponentModel;

namespace Jinyinmao.Application.Constants
{
    /// <summary>
    ///     Enum SmsGateway
    /// </summary>
    public enum SmsGateway
    {
        /// <summary>
        ///     The default
        /// </summary>
        Default = 0,

        /// <summary>
        ///     创蓝
        /// </summary>
        [Description("创蓝")]
        ChuangLan = 10,

        /// <summary>
        ///     助通
        /// </summary>
        [Description("助通")]
        ZhuTong = 20
    }

    public static class SmsGatewayEx
    {
        private static readonly Dictionary<int, SmsGateway> smsGateways = new Dictionary<int, SmsGateway>
        {
            { 0, SmsGateway.Default },
            { 10, SmsGateway.ChuangLan },
            { 20, SmsGateway.ZhuTong }
        };

        public static SmsGateway ToSmsGateway(this int code)
        {
            return smsGateways.GetOrDefault(code);
        }
    }
}
// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : LoginType.cs
// Created          : 2016-12-14  20:15
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:17
// ***********************************************************************
// <copyright file="LoginType.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

namespace Jinyinmao.AuthManager.Libraries.Parameter
{
    /// <summary>
    ///     Class LoginType.
    /// </summary>
    public class LoginType
    {
        public static readonly LoginType Basic = new LoginType("Basic");

        public static readonly LoginType Quick = new LoginType("JYMQuick");

        public static readonly LoginType WeChat = new LoginType("JYMWeChat");

        private static readonly Dictionary<string, LoginType> Dic = new Dictionary<string, LoginType>
        {
            { "Basic", Basic },
            { "Quick", Quick },
            { "WeChat", WeChat }
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public LoginType(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }

        public static string GeLoginTypetValue(string key)
        {
            return Dic.Where(x => x.Key == key).Select(x => x.Value.Value).FirstOrDefault();
        }

        public static LoginType GetLoginType(string key)
        {
            return Dic.Where(item => item.Key == key).Select(item => item.Value).FirstOrDefault();
        }
    }
}
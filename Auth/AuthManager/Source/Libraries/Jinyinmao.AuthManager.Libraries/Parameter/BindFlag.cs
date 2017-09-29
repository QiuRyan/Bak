// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : BindFlag.cs
// Created          : 2016-12-14  20:15
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:17
// ***********************************************************************
// <copyright file="BindFlag.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

namespace Jinyinmao.AuthManager.Libraries.Parameter
{
    public class BindFlag
    {
        public static readonly BindFlag No = new BindFlag(0);

        public static readonly BindFlag Yes = new BindFlag(1);

        private static readonly Dictionary<string, BindFlag> Dic = new Dictionary<string, BindFlag>
        {
            { "Yes", Yes },
            { "No", No }
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public BindFlag(int code)
        {
            this.Code = code;
        }

        public int Code { get; set; }

        public BindFlag GetBindFlag(string key)
        {
            return Dic.Where(t => t.Key == key).Select(t => t.Value).FirstOrDefault();
        }

        public int GetBindFlagCode(string key)
        {
            return Dic.Where(t => t.Key == key).Select(t => t.Value.Code).FirstOrDefault();
        }
    }
}
// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : StringHelper.cs
// Created          : 2016-12-14  20:14
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:14
// ***********************************************************************
// <copyright file="StringHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Globalization;
using System.Linq;

namespace Jinyinmao.AuthManager.Libraries.Helper
{
    public static class StringHelper
    {
        public static readonly string X36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string X10ToX36(this string str)
        {
            string result = "";
            long value1 = Convert.ToInt64(str);
            while (value1 >= 36)
            {
                int value2 = (int)(value1 % 36);
                result = X36[value2] + result;
                value1 /= 36;
            }
            if (value1 >= 0) result = X36[(int)value1] + result;
            return result;
        }

        public static string X36ToX10(this string str)
        {
            double result = str.Select((t, i) => Math.Pow(36, str.Length - 1 - i) * X36.IndexOf(t)).Sum();
            return result.ToString(CultureInfo.InvariantCulture);
        }
    }
}
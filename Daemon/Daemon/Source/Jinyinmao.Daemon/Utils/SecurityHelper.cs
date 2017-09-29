// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : SecurityHelper.cs
// Created          : 2017-08-29  17:37
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:38
// ******************************************************************************************************
// <copyright file="SecurityHelper.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using J.Base.Lib.Security;

namespace Jinyinmao.Daemon.Utils
{
    public static class SecurityHelper
    {
        /// <summary>
        ///     解密
        /// </summary>
        /// <param name="import">The import.</param>
        /// <param name="cryptoKey"></param>
        /// <returns></returns>
        public static string Decrypt(this string import, string cryptoKey)
        {
            try
            {
                return Crypto.DecryptString(import, cryptoKey);
            }
            catch (Exception)
            {
                return import;
            }
        }
    }
}
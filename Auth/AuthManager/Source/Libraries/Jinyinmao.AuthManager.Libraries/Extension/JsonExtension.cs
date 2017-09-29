// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : JsonExtension.cs
// Created          : 2016-12-14  20:14
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:14
// ***********************************************************************
// <copyright file="JsonExtension.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;

namespace Jinyinmao.AuthManager.Libraries.Extension
{
    public static class JsonExtension
    {
        /// <summary>
        ///     Dates the time json format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">The t.</param>
        /// <param name="format">指定时间格式</param>
        /// <returns>System.String.</returns>
        public static string DateTimeJsonFormat<T>(this T t, string format = "yyyy-MM-dd HH:mm:ss") where T : class
        {
            return JsonConvert.SerializeObject(t, Formatting.Indented, new JsonSerializerSettings { DateFormatString = format });
        }
    }
}
// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : Credential.cs
// Created          : 2016-12-14  20:15
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:17
// ***********************************************************************
// <copyright file="Credential.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

namespace Jinyinmao.AuthManager.Libraries.Parameter
{
    public class Credential
    {
        /// <summary>
        ///     身份证
        /// </summary>
        public static readonly Credential IdCard = new Credential(10);

        /// <summary>
        ///     军官证
        /// </summary>
        public static readonly Credential Junguan = new Credential(40);

        /// <summary>
        ///     无
        /// </summary>
        public static readonly Credential None = new Credential(0);

        /// <summary>
        ///     护照
        /// </summary>
        public static readonly Credential Passport = new Credential(20);

        /// <summary>
        ///     台湾
        /// </summary>
        public static readonly Credential Taiwan = new Credential(30);

        /// <summary>
        ///     The codes
        /// </summary>
        private static readonly Dictionary<string, Credential> Dic = new Dictionary<string, Credential>
        {
            { "None", None },
            { "IdCard", IdCard },
            { "Passport", Passport },
            { "Taiwan", Taiwan },
            { "Junguan", Junguan }
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Credential(int code)
        {
            this.Code = code;
        }

        public int Code { get; set; }

        public static Credential GetCredential(string key)
        {
            return Dic.Where(t => t.Key == key).Select(t => t.Value).FirstOrDefault();
        }

        public static int GetCredentialCode(string key)
        {
            return Dic.Where(t => t.Key == key).Select(t => t.Value.Code).FirstOrDefault();
        }
    }
}
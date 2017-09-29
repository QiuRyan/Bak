// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : VeriCodeType.cs
// Created          : 2016-12-14  20:15
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:17
// ***********************************************************************
// <copyright file="VeriCodeType.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

namespace Jinyinmao.AuthManager.Libraries.Parameter
{
    /// <summary>
    ///     Class VeriCodeType.
    /// </summary>
    public class VeriCodeType
    {
        /// <summary>
        ///     快速登录
        /// </summary>
        public static readonly VeriCodeType QuickSignIn = new VeriCodeType(40);

        /// <summary>
        ///     重置登录密码
        /// </summary>
        public static readonly VeriCodeType ResetLoginPassword = new VeriCodeType(20);

        /// <summary>
        ///     重置支付密码
        /// </summary>
        public static readonly VeriCodeType ResetPaymentPassword = new VeriCodeType(30);

        /// <summary>
        ///     注册
        /// </summary>
        public static readonly VeriCodeType SignUp = new VeriCodeType(10);

        /// <summary>
        ///     新手机号码验证
        /// </summary>
        public static readonly VeriCodeType VarifyNewCellphone = new VeriCodeType(90);

        /// <summary>
        ///     老手机号码验证
        /// </summary>
        public static readonly VeriCodeType VarifyOldCellphone = new VeriCodeType(80);

        /// <summary>
        ///     微信绑定
        /// </summary>
        public static readonly VeriCodeType WeChatBind = new VeriCodeType(60);

        /// <summary>
        ///     微信注册
        /// </summary>
        public static readonly VeriCodeType WeChatSignUp = new VeriCodeType(50);

        private static readonly Dictionary<string, VeriCodeType> VeriCodes = new Dictionary<string, VeriCodeType>
        {
            { "QuickSignIn", QuickSignIn },
            { "ResetLoginPassword", ResetLoginPassword },
            { "ResetPaymentPassword", ResetPaymentPassword },
            { "SignUp", SignUp },
            { "WeChatSignUp", WeChatSignUp },
            { "WeChatBind", WeChatBind }
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public VeriCodeType(int code)
        {
            this.Code = code;
        }

        public int Code { get; set; }

        public static VeriCodeType GetVeriCodeType(string key)
        {
            return VeriCodes.Where(t => t.Key == key).Select(t => t.Value).FirstOrDefault();
        }

        public static int GetVeriCodeTypeCode(string key)
        {
            return VeriCodes.Where(t => t.Key == key).Select(t => t.Value.Code).FirstOrDefault();
        }
    }
}
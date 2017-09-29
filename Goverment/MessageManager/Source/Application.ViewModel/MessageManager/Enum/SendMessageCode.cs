// ***********************************************************************
// Project          : MessageManager
// File             : SendMessageCode.cs
// Created          : 2015-12-06  16:48
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-06  16:51
// ***********************************************************************
// <copyright file="SendMessageCode.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel;

namespace Jinyinmao.Application.ViewModel.MessageManager.Enum
{
    /// <summary>
    ///     返回错误或正确信息
    /// </summary>
    public enum SendMessageCode
    {
        /// <summary>
        ///     通过
        /// </summary>
        [Description("通过")]
        Pass = 1,

        /// <summary>
        ///     错误
        /// </summary>
        [Description("错误")]
        Error = 4
    }
}
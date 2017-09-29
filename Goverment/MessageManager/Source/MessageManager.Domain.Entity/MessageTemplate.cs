// ***********************************************************************
// Project          : MessageManager
// File             : MessageTemplate.cs
// Created          : 2015-11-28  15:33
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-11-28  15:35
// ***********************************************************************
// <copyright file="MessageTemplate.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Jinyinmao.MessageManager.Domain.Entity
{
    /// <summary>
    /// MessageTemplate.
    /// </summary>
    public partial class MessageTemplate
    {
        /// <summary>
        /// Fills the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string Fill(Dictionary<string, string> args)
        {
            throw new NotImplementedException();
        }
    }
}
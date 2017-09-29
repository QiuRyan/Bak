// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : AdminCancelAccountCommand.cs
// Created          : 2016-08-22  4:59 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-22  5:07 PM
// ***********************************************************************
// <copyright file="AdminCancelAccountCommand.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;
using System;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class AdminCancelAccount : Command
    {
        public string Cellphone { get; set; }

        public Guid UserId { get; set; }
    }
}
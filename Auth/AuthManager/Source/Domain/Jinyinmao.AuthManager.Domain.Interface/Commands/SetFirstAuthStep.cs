// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : SetFirstAuthStepCommand.cs
// Created          : 2016-08-16  2:49 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-16  2:50 PM
// ***********************************************************************
// <copyright file="SetFirstAuthStepCommand.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;
using System;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class SetFirstAuthStep : Command
    {
        public string Cellphone { get; set; }

        public string SMSToken { get; set; }

        public string Token { get; set; }

        public Guid UserId { get; set; }

        public string ValidateMessage { get; set; }
    }
}
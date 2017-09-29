// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : SetThirdAuthStepCommand.cs
// Created          : 2016-08-15  6:30 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-16  2:58 PM
// ***********************************************************************
// <copyright file="SetThirdAuthStepCommand.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;
using System;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class SetThirdAuthStep : Command
    {
        public string NewCellphone { get; set; }

        public string PreviousToken { get; set; }

        public string SMSToken { get; set; }

        public string Token { get; set; }

        public Guid UserId { get; set; }

        public string ValidateMessage { get; set; }
    }
}
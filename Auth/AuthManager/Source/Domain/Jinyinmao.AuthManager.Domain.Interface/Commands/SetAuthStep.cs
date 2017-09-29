// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : SetAuthStepCommand.cs
// Created          : 2016-08-13  2:20 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-15  6:42 PM
// ***********************************************************************
// <copyright file="SetAuthStepCommand.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;
using System;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class SetAuthStep : Command
    {
        public string CredentialNo { get; set; }

        public string Name { get; set; }

        public string NewCellphone { get; set; }

        public string OldSMSToken { get; set; }

        public string SMSToken { get; set; }

        public string Token { get; set; }

        public Guid UserId { get; set; }

        public string ValidateMessage { get; set; }
    }
}
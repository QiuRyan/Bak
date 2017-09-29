// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : SetSecondAuthStepCommand.cs
// Created          : 2016-08-15  6:30 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-16  2:56 PM
// ***********************************************************************
// <copyright file="SetSecondAuthStepCommand.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;
using System;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class SetSecondAuthStep : Command
    {
        public string CredentialNo { get; set; }

        public string Name { get; set; }

        public string PreviousToken { get; set; }
        public string SMSToken { get; set; }
        public string Token { get; set; }

        public Guid UserId { get; set; }

        public string ValidateMessage { get; set; }
    }
}
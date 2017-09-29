// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : AdminModifyCellphone.cs
// Created          : 2016-08-22  3:40 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-22  4:03 PM
// ***********************************************************************
// <copyright file="AdminModifyCellphone.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;
using System;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class AdminModifyCellphoneCommand : Command
    {
        public string NewCellphone { get; set; }

        public Guid UserId { get; set; }
    }
}
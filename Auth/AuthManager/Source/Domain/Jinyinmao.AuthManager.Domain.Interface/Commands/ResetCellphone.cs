// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ResetCellphone.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-14  20:21
// ***********************************************************************
// <copyright file="ResetCellphone.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class ResetCellphone : Command
    {
        public string Messager { get; set; }

        public string NewCellphone { get; set; }
    }
}
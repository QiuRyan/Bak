// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : SetLoginCellphoneToTirisfer.cs
// Created          : 2016-08-15  1:19 PM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-15  1:20 PM
// ***********************************************************************
// <copyright file="SetLoginCellphoneToTirisfer.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.Commands;
using Orleans.Concurrency;

namespace Jinyinmao.AuthManager.Domain.Interface.Commands
{
    [Immutable]
    public class SetLoginCellphoneToTirisfer : Command
    {
        public string Cellphone { get; set; }

        public string UserIdentifier { get; set; }
    }
}
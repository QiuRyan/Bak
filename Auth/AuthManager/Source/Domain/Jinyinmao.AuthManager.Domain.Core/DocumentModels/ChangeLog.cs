// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ChangeLog.cs
// Created          : 2016-08-16  10:32 AM
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-08-16  10:33 AM
// ***********************************************************************
// <copyright file="ChangeLog.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2016 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Jinyinmao.AuthManager.Libraries;

namespace Jinyinmao.AuthManager.Domain.Core.DocumentModels
{
    public class ChangeLog : DocumentBase
    {
        public string ChangeIdentifier { get; set; }

        public DateTime CreateTime { get; set; }
        

        public string Info { get; set; }

        public string NewCellphone { get; set; }

        public string OldCellphone { get; set; }

        public string UserIdentifier { get; set; }
    }
}
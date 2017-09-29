// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ChangeLog.cs
// Created          : 2016-12-28  13:49
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-02-03  14:07
// ***********************************************************************
// <copyright file="ChangeLog.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Jinyinmao.AuthManager.Domain.Core.SQLDB.Model
{
    public class ChangeLog
    {
        public string ChangeIdentifier { get; set; }
        public DateTime CreateTime { get; set; }
        public int Id { get; set; }
        public string Info { get; set; }

        public string NewCellphone { get; set; }

        public string OldCellphone { get; set; }

        public string UserIdentifier { get; set; }
    }
}
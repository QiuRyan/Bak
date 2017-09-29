// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : ChangeLogMap.cs
// Created          : 2016-12-28  11:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-28  11:50
// ***********************************************************************
// <copyright file="ChangeLogMap.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.SQLDB.Model;
using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.AuthManager.Domain.Core.SQLDB.Mapping
{
    public class ChangeLogMap : EntityTypeConfiguration<ChangeLog>
    {
        public ChangeLogMap()
        {
            this.HasKey(p => p.Id);
            this.ToTable("ChangeLog");
        }
    }
}
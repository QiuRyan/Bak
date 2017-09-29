// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : UserRelationMap.cs
// Created          : 2016-12-28  11:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-28  11:51
// ***********************************************************************
// <copyright file="UserRelationMap.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.SQLDB.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.AuthManager.Domain.Core.SQLDB.Mapping
{
    internal class UserRelationMap : EntityTypeConfiguration<UserRelation>
    {
        public UserRelationMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .IsRequired();

            // Properties
            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(32);

            this.Property(t => t.AccountType)
                .IsRequired();

            this.Property(t => t.CreateTime)
                .IsRequired();

            this.Property(t => t.IsAlive)
                .IsRequired();

            this.Property(t => t.LastModified);

            this.Property(t => t.LoginName)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("UserRelation");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.LoginName).HasColumnName("LoginName");
            this.Property(t => t.AccountType).HasColumnName("AccountType");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.IsAlive).HasColumnName("IsAlive");
            this.Property(t => t.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.LastModified).HasColumnName("LastModified");
        }
    }
}
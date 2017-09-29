// ***********************************************************************
// Project          : MessageManager
// File             : UserInfoMap.cs
// Created          : 2015-11-30  17:33
//
// Last Modified By : 张政平
// Last Modified On : 2015-11-30  17:33
// ***********************************************************************
// <copyright file="UserInfoMap.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.MessageManager.Domain.Entity.DbMap
{
    internal class UserInfoMap : EntityTypeConfiguration<UserInfo>
    {
        public UserInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.UId);

            this.Property(t => t.UserKey)
               .IsRequired()
               .IsUnicode(false)
               .HasMaxLength(32);

            // Properties
            this.Property(t => t.UId)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(32);

            this.Property(t => t.UAppId)
                //.IsRequired() //是否选填
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(t => t.PhoneNum)
                //.IsRequired() //是否选填
                .HasMaxLength(500);

            this.Property(t => t.WeChatNum)
                //.IsRequired() //是否选填
                .HasMaxLength(30);

            this.Property(t => t.Remark)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("UserInfo");
            this.Property(t => t.UId).HasColumnName("UId");
            this.Property(t => t.UAppId).HasColumnName("UAppId");
            this.Property(t => t.PhoneNum).HasColumnName("PhoneNum");
            this.Property(t => t.WeChatNum).HasColumnName("WeChatNum");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
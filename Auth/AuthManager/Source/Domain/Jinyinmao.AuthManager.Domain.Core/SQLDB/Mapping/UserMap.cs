// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : UserMap.cs
// Created          : 2016-12-28  11:43
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-28  11:50
// ***********************************************************************
// <copyright file="UserMap.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.SQLDB.Model;
using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.AuthManager.Domain.Core.SQLDB.Mapping
{
    /// <summary>
    ///     UserMap.
    /// </summary>
    public class UserMap : EntityTypeConfiguration<DBUser>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserMap" /> class.
        /// </summary>
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserIdentifier);

            // Properties
            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(32);

            this.Property(t => t.Cellphone)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(t => t.ClientType)
                .IsRequired();

            this.Property(t => t.Closed)
                .IsRequired();

            this.Property(t => t.ContractId)
                .IsRequired();

            this.Property(t => t.EncryptedPassword)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(200);

            this.Property(t => t.Info)
                .IsUnicode()
                .IsRequired();

            this.Property(t => t.InviteBy)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(t => t.InviteFor)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(t => t.LastModified)
                .IsRequired();

            this.Property(t => t.LoginNames)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255);

            this.Property(t => t.OutletCode)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(t => t.RegisterTime)
                .IsRequired();

            this.Property(t => t.Salt)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.ClientType).HasColumnName("ClientType");
            this.Property(t => t.Closed).HasColumnName("Closed");
            this.Property(t => t.ContractId).HasColumnName("ContractId");
            this.Property(t => t.EncryptedPassword).HasColumnName("EncryptedPassword");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.InviteBy).HasColumnName("InviteBy");
            this.Property(t => t.InviteFor).HasColumnName("InviteFor");
            this.Property(t => t.LastModified).HasColumnName("LastModified");
            this.Property(t => t.LoginNames).HasColumnName("LoginNames");
            this.Property(t => t.OutletCode).HasColumnName("OutletCode");
            this.Property(t => t.RegisterTime).HasColumnName("RegisterTime");
            this.Property(t => t.Salt).HasColumnName("Salt");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
        }
    }
}
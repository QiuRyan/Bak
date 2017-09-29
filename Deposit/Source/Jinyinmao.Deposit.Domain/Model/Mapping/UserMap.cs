// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  5:29 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  6:05 PM
// ***********************************************************************
// <copyright file="UserMap.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Deposit.Domain.Mapping
{
    /// <summary>
    ///     UserMap.
    /// </summary>
    public class UserMap : EntityTypeConfiguration<User>
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
                .HasMaxLength(50);

            this.Property(t => t.Cellphone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.LoginNames)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.RealName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CredentialNo)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.VerifiedKey)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OutletCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.InviteBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Info)
                .IsRequired();

            this.Property(t => t.Args)
                .IsRequired();

            this.Property(t => t.ActivationTime);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.RegisterTime).HasColumnName("RegisterTime");
            this.Property(t => t.LoginNames).HasColumnName("LoginNames");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.Credential).HasColumnName("Credential");
            this.Property(t => t.CredentialNo).HasColumnName("CredentialNo");
            this.Property(t => t.VerifiedKey).HasColumnName("VerifiedKey");
            this.Property(t => t.Verified).HasColumnName("Verified");
            this.Property(t => t.VerifiedTime).HasColumnName("VerifiedTime");
            this.Property(t => t.OutletCode).HasColumnName("OutletCode");
            this.Property(t => t.ClientType).HasColumnName("ClientType");
            this.Property(t => t.InviteBy).HasColumnName("InviteBy");
            this.Property(t => t.ContractId).HasColumnName("ContractId");
            this.Property(t => t.Closed).HasColumnName("Closed");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.Args).HasColumnName("Args");

            this.Property(t => t.LastModified).HasColumnName("LastModified");
            this.Property(t => t.AuthWithdraw).HasColumnName("AuthWithdraw");
            this.Property(t => t.EntrustedPay).HasColumnName("EntrustedPay");
            this.Property(t => t.IsActivation).HasColumnName("IsActivation");
            this.Property(t => t.CgBankCode).HasColumnName("CgBankCode");
            this.Property(t => t.CgBankCardCellphone).HasColumnName("CgBankCardCellphone");
            this.Property(t => t.CgBankCardNo).HasColumnName("CgBankCardNo");
            this.Property(t => t.ActivationTime).HasColumnName("ActivationTime");
        }
    }
}
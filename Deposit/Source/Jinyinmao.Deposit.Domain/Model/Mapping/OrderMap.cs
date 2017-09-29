// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  5:29 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-08  1:01 PM
// ***********************************************************************
// <copyright file="OrderMap.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Deposit.Domain.Mapping
{
    /// <summary>
    ///     OrderMap.
    /// </summary>
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderMap" /> class.
        /// </summary>
        public OrderMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderIdentifier);

            // Properties
            this.Property(t => t.OrderIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AccountTransactionIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OrderNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ProductIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProductSnapshot)
                .IsRequired();

            this.Property(t => t.TransDesc)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Cellphone)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.UserInfo)
                .IsRequired();

            this.Property(t => t.Info)
                .IsRequired();

            this.Property(t => t.Args)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Orders");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.AccountTransactionIdentifier).HasColumnName("AccountTransactionIdentifier");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.OrderTime).HasColumnName("OrderTime");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.ProductCategory).HasColumnName("ProductCategory");
            this.Property(t => t.ProductSnapshot).HasColumnName("ProductSnapshot");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.ExtraYield).HasColumnName("ExtraYield");
            this.Property(t => t.Interest).HasColumnName("Interest");
            this.Property(t => t.ExtraInterest).HasColumnName("ExtraInterest");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.SettleDate).HasColumnName("SettleDate");
            this.Property(t => t.ResultCode).HasColumnName("ResultCode");
            this.Property(t => t.ResultTime).HasColumnName("ResultTime");
            this.Property(t => t.IsRepaid).HasColumnName("IsRepaid");
            this.Property(t => t.RepaidTime).HasColumnName("RepaidTime");
            this.Property(t => t.TransDesc).HasColumnName("TransDesc");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.UserInfo).HasColumnName("UserInfo");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.Args).HasColumnName("Args");
        }
    }
}
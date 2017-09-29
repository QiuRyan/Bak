// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : RegularProductMap.cs
// Created          : 2015-04-29  5:29 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-02  11:37 AM
// ***********************************************************************
// <copyright file="RegularProductMap.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Deposit.Domain.Mapping
{
    /// <summary>
    ///     RegularProductMap.
    /// </summary>
    public class RegularProductMap : EntityTypeConfiguration<RegularProduct>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegularProductMap" /> class.
        /// </summary>
        public RegularProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);

            // Properties
            this.Property(t => t.ProductIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ProductNo)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PledgeNo)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Info)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("RegularProducts");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.ProductCategory).HasColumnName("ProductCategory");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.IssueNo).HasColumnName("IssueNo");
            this.Property(t => t.PledgeNo).HasColumnName("PledgeNo");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.FinancingSumAmount).HasColumnName("FinancingSumAmount");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.IssueTime).HasColumnName("IssueTime");
            this.Property(t => t.StartSellTime).HasColumnName("StartSellTime");
            this.Property(t => t.EndSellTime).HasColumnName("EndSellTime");
            this.Property(t => t.SoldOut).HasColumnName("SoldOut");
            this.Property(t => t.SoldOutTime).HasColumnName("SoldOutTime");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.ValueDateMode).HasColumnName("ValueDateMode");
            this.Property(t => t.SettleDate).HasColumnName("SettleDate");
            this.Property(t => t.RepaymentDeadline).HasColumnName("RepaymentDeadline");
            this.Property(t => t.Repaid).HasColumnName("Repaid");
            this.Property(t => t.RepaidTime).HasColumnName("RepaidTime");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.IsLoans).HasColumnName("IsLoans");
        }
    }
}
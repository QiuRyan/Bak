// *********************************************************************** Project :
// io.yuyi.jinyinmao.server File : YemProductMap.cs Created : 2015-04-29 5:29 PM
//
// Last Modified By : Siqi Lu Last Modified On : 2015-08-02 11:37 AM ***********************************************************************
// <copyright file="YemProductMap.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Deposit.Domain.Mapping
{
    /// <summary>
    /// YemProductMap.
    /// </summary>
    public class YemProductMap : EntityTypeConfiguration<YemProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YemProductMap"/> class.
        /// </summary>
        public YemProductMap()
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

            this.Property(t => t.Info)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("JBYProducts");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.ProductCategory).HasColumnName("ProductCategory");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.IssueNo).HasColumnName("IssueNo");
            this.Property(t => t.FinancingSumAmount).HasColumnName("FinancingSumAmount");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.IssueTime).HasColumnName("IssueTime");
            this.Property(t => t.StartSellTime).HasColumnName("StartSellTime");
            this.Property(t => t.EndSellTime).HasColumnName("EndSellTime");
            this.Property(t => t.SoldOut).HasColumnName("SoldOut");
            this.Property(t => t.SoldOutTime).HasColumnName("SoldOutTime");
            this.Property(t => t.ValueDateMode).HasColumnName("ValueDateMode");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.Info).HasColumnName("Info");
        }
    }
}
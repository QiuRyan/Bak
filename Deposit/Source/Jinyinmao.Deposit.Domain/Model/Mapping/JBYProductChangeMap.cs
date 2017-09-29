// ***********************************************************************
// Project          : Jinyinmao.Tirisfal
// File             : JBYProductChangeMap.cs
// Created          : 2017-01-18  09:24
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-01-18  17:33
// ***********************************************************************
// <copyright file="JBYProductChangeMap.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Deposit.Domain.Mapping
{
    /// <summary>
    ///     Class JBYProductChangeMap.
    /// </summary>
    public class JBYProductChangeMap : EntityTypeConfiguration<JBYProductChange>
    {
        /// <summary>
        ///     Initializes a new instance of the class.
        /// </summary>
        public JBYProductChangeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.ProductIdentifier).IsRequired().HasMaxLength(50);

            this.Property(t => t.AssetInfo).IsRequired();

            this.Property(t => t.OrgiginProductInfo).IsRequired();

            this.Property(t => t.TradeCode).IsRequired();

            this.Property(t => t.CreateTime).IsRequired();

            this.Property(t => t.Info).IsRequired();

            // Table & Column Mappings
            this.ToTable("JBYProductChange");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.AssetInfo).HasColumnName("AssetInfo");
            this.Property(t => t.OrgiginProductInfo).HasColumnName("OrgiginProductInfo");
            this.Property(t => t.TradeCode).HasColumnName("TradeCode");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Info).HasColumnName("Info");
        }
    }
}
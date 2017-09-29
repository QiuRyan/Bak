// ******************************************************************************************************
// Project : Jinyinmao.Tirisfal File : YemAssetRecordMap.cs Created : 2017-06-06 11:52
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-07-17 20:17 ******************************************************************************************************
// <copyright file="YemAssetRecordMap.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Deposit.Domain.Mapping
{
    /// <summary>
    ///     Class YemAssetRecordMap.
    /// </summary>
    public class YemAssetRecordMap : EntityTypeConfiguration<YemAssetRecord>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="YemAssetRecordMap" /> class.
        /// </summary>
        public YemAssetRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserIdentifier).IsRequired().HasMaxLength(50);
            this.Property(t => t.Amount).IsRequired();
            this.Property(t => t.Args).IsRequired();
            this.Property(t => t.CreateTime).IsRequired();
            this.Property(t => t.Info).IsRequired();
            this.Property(t => t.ResultCode).IsRequired();
            this.Property(t => t.ResultTime).IsRequired();
            this.Property(t => t.TradeCode).IsRequired();
            this.Property(t => t.ProductIdentifier).IsRequired();
            this.Property(t => t.SourceYemAssetRecordIdentifier).IsRequired();
            this.Property(t => t.UserInfo).IsRequired();
            this.Property(t => t.YemOrderIdentifier).IsRequired().HasMaxLength(50);
            this.Property(t => t.AssetIdentifier).HasMaxLength(50);
            this.Property(t => t.IsSignature);
            this.Property(t => t.Description);

            // Table & Column Mappings
            this.ToTable("JBYAssetRecord");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.YemOrderIdentifier).HasColumnName("JBYOrderIdentifier");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.TradeCode).HasColumnName("TradeCode");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.ResultCode).HasColumnName("ResultCode");
            this.Property(t => t.ResultTime).HasColumnName("ResultTime");
            this.Property(t => t.UserInfo).HasColumnName("UserInfo");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.Args).HasColumnName("Args");
            this.Property(t => t.SequenceNo).HasColumnName("SequenceNo");
            this.Property(t => t.TransactionAmount).HasColumnName("TransactionAmount");
            this.Property(t => t.SourceYemAssetRecordIdentifier).HasColumnName("SourceJBYAssetRecordIdentifier");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.AssetIdentifier).HasColumnName("AssetIdentifier");
            this.Property(t => t.IsSignature).HasColumnName("IsSignature");
        }
    }
}
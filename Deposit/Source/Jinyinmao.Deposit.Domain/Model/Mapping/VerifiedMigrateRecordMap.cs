// ******************************************************************************************************
// Project : Jinyinmao.Tirisfal File : VerifiedMigrateRecordMap.cs Created : 2017-07-31 14:33
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn) Last Modified On : 2017-07-31 14:33 ******************************************************************************************************
// <copyright file="VerifiedMigrateRecordMap.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright © 2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Deposit.Domain.Mapping
{
    /// <summary>
    ///     Class VerifiedMigrateRecordMap.
    /// </summary>
    public class VerifiedMigrateRecordMap : EntityTypeConfiguration<VerifiedMigrateRecord>
    {
        /// <summary>
        ///     Initializes a new instance of the   class.
        /// </summary>
        public VerifiedMigrateRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            // Properties
            this.Property(t => t.RecordIdentfier)
                .IsRequired()
                .HasMaxLength(50);

            // Properties
            this.Property(t => t.LastUpdateTime)
                .IsRequired();

            this.Property(t => t.Amount)
                .IsRequired();

            this.Property(t => t.MigrateTime)
                .IsRequired();

            this.Property(t => t.Status)
                .IsRequired();

            this.Property(t => t.VerifiedTime)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VerifiedMigrateRecord");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.MigrateTime).HasColumnName("MigrateTime");
            this.Property(t => t.RecordIdentfier).HasColumnName("RecordIdentfier");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.VerifiedTime).HasColumnName("VerifiedTime");
        }
    }
}
// ******************************************************************************************************
// Project          : Jinyinmao.Tirisfal
// File             : YemTranscationMap.cs
// Created          : 2017-03-14  11:31
// 
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-07-18  10:41
// ******************************************************************************************************
// <copyright file="YemTranscationMap.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Deposit.Domain.Mapping
{
    /// <summary>
    ///     YemTranscationMap.
    /// </summary>
    public class YemTranscationMap : EntityTypeConfiguration<YemTransaction>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="YemTranscationMap" /> class.
        /// </summary>
        public YemTranscationMap()
        {
            // Primary Key
            this.HasKey(t => t.TransactionIdentifier);

            // Properties
            this.Property(t => t.TransactionIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AccountTransactionIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.YemProductIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TransDesc)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.UserInfo)
                .IsRequired();

            this.Property(t => t.Info)
                .IsRequired();

            this.Property(t => t.Args)
                .IsRequired();
            this.Property(t => t.IsSignature);

            // Table & Column Mappings
            this.ToTable("JBYTransactions");
            this.Property(t => t.TransactionIdentifier).HasColumnName("TransactionIdentifier");
            this.Property(t => t.AccountTransactionIdentifier).HasColumnName("AccountTransactionIdentifier");
            this.Property(t => t.YemProductIdentifier).HasColumnName("JBYProductIdentifier");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.TradeCode).HasColumnName("TradeCode");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.TransactionTime).HasColumnName("TransactionTime");
            this.Property(t => t.ResultCode).HasColumnName("ResultCode");
            this.Property(t => t.ResultTime).HasColumnName("ResultTime");
            this.Property(t => t.TransDesc).HasColumnName("TransDesc");
            this.Property(t => t.UserInfo).HasColumnName("UserInfo");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.Args).HasColumnName("Args");
            this.Property(t => t.IsSignature).HasColumnName("IsSignature");
        }
    }
}
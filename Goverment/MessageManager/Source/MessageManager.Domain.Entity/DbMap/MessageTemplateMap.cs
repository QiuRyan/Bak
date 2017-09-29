// ***********************************************************************
// Project          : MessageManager
// File             : MessageTemplateMap.cs
// Created          : 2015-12-01  11:56
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-01  11:57
// ***********************************************************************
// <copyright file="MessageTemplateMap.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.MessageManager.Domain.Entity.DbMap
{
    internal class MessageTemplateMap : EntityTypeConfiguration<MessageTemplate>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageTemplateMap" /> class.
        /// </summary>
        public MessageTemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.TemplateId);

            // Properties
            this.Property(t => t.TemplateId)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(32);

            this.Property(t => t.BizCode)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(t => t.TemplateContent)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(500);

            this.Property(t => t.TemplateTitle)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(30);

            this.Property(t => t.Remark)
                .IsOptional()
                .HasMaxLength(300);

            this.Property(t => t.IsValid)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("MessageTemplates");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.BizCode).HasColumnName("BizCode");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.TemplateContent).HasColumnName("TemplateContent");
            this.Property(t => t.TemplateTitle).HasColumnName("TemplateTitle");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.IsValid).HasColumnName("IsValid");
        }
    }
}
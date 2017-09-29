using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Jinyinmao.ValidateCode.Domain.Entity;

namespace ValidateCode.Domain.Entity.DbMap
{
    internal class VeriCodeMap : EntityTypeConfiguration<VeriCode>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VeriCodeMap" /> class.
        /// </summary>
        public VeriCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.BuildAt)
                .IsRequired();

            this.Property(t => t.Cellphone)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(t => t.Code)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(200);

            this.Property(t => t.ErrorCount)
                .IsRequired();

            this.Property(t => t.Times)
                .IsRequired();

            this.Property(t => t.Token)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(32);

            this.Property(t => t.Type)
                .IsRequired();

            this.Property(t => t.Used)
                .IsRequired();

            this.Property(t => t.Verified)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VeriCodes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BuildAt).HasColumnName("BuildAt");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ErrorCount).HasColumnName("ErrorCount");
            this.Property(t => t.Times).HasColumnName("Times");
            this.Property(t => t.Token).HasColumnName("Token");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Used).HasColumnName("Used");
            this.Property(t => t.Verified).HasColumnName("Verified");
        }
    }
}
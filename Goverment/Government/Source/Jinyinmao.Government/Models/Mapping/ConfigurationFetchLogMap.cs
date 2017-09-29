using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Government.Models.Mapping
{
    internal class ConfigurationFetchLogMap : EntityTypeConfiguration<ConfigurationFetchLog>
    {
        internal ConfigurationFetchLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.FetchedVersion)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.SourceRole)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.SourceRoleInstance)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.SourceIP)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.SourceVersion)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.Time)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ConfigurationFetchLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FetchedVersion).HasColumnName("FetchedVersion");
            this.Property(t => t.SourceRole).HasColumnName("SourceRole");
            this.Property(t => t.SourceRoleInstance).HasColumnName("SourceRoleInstance");
            this.Property(t => t.SourceIP).HasColumnName("SourceIP");
            this.Property(t => t.SourceVersion).HasColumnName("SourceVersion");
            this.Property(t => t.Time).HasColumnName("Time");
        }
    }
}
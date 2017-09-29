using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Government.Models.Mapping
{
    internal class ApplicationMap : EntityTypeConfiguration<Application>
    {
        internal ApplicationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Configurations)
                .IsRequired()
                .IsUnicode();

            this.Property(t => t.ConfigurationVersion)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.KeyId)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(32);

            this.Property(t => t.Keys)
                .IsRequired()
                .IsUnicode(false);

            this.Property(t => t.Role)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.ServiceEndpoint)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255);

            this.Property(t => t.CreatedTime)
                .IsRequired();

            this.Property(t => t.LastModifiedBy)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255);

            this.Property(t => t.LastModifiedTime)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Applications");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Configurations).HasColumnName("Configurations");
            this.Property(t => t.ConfigurationVersion).HasColumnName("ConfigurationVersion");
            this.Property(t => t.KeyId).HasColumnName("KeyId");
            this.Property(t => t.Keys).HasColumnName("Keys");
            this.Property(t => t.Role).HasColumnName("Role");
            this.Property(t => t.ServiceEndpoint).HasColumnName("ServiceEndpoint");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.LastModifiedTime).HasColumnName("LastModifiedTime");
        }
    }
}
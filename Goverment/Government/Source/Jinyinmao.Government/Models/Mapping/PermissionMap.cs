using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Government.Models.Mapping
{
    internal class PermissionMap : EntityTypeConfiguration<Permission>
    {
        internal PermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Expiry)
                .IsRequired();

            this.Property(t => t.ObjectApplicationId)
                .IsRequired();

            this.Property(t => t.PermissionLevel)
                .IsRequired();

            this.Property(t => t.SubjectApplicationId)
                .IsRequired();

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
            this.ToTable("Permissions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Expiry).HasColumnName("Expiry");
            this.Property(t => t.ObjectApplicationId).HasColumnName("ObjectApplicationId");
            this.Property(t => t.PermissionLevel).HasColumnName("PermissionLevel");
            this.Property(t => t.SubjectApplicationId).HasColumnName("SubjectApplicationId");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.LastModifiedTime).HasColumnName("LastModifiedTime");
        }
    }
}
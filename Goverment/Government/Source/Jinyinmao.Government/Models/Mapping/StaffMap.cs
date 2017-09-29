using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Government.Models.Mapping
{
    internal class StaffMap : EntityTypeConfiguration<Staff>
    {
        internal StaffMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Cellphone)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.Key)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.Name)
                .IsRequired()
                .IsUnicode()
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
            this.ToTable("Staves");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Key).HasColumnName("Key");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.LastModifiedTime).HasColumnName("LastModifiedTime");
        }
    }
}
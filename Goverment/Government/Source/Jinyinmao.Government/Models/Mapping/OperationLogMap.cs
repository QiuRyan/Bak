using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.Government.Models.Mapping
{
    internal class OperationLogMap : EntityTypeConfiguration<OperationLog>
    {
        internal OperationLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Operation)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.Operator)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255);

            this.Property(t => t.Parameters)
                .IsRequired()
                .IsUnicode();

            this.Property(t => t.Time)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("OperationLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Operation).HasColumnName("Operation");
            this.Property(t => t.Operator).HasColumnName("Operator");
            this.Property(t => t.Parameters).HasColumnName("Parameters");
            this.Property(t => t.Time).HasColumnName("Time");
        }
    }
}
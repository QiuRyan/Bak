using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.MessageWorker.Domain.Entity
{
    internal class SmsMessageMap : EntityTypeConfiguration<SmsMessage>
    {
        public SmsMessageMap()
        {
            // Primary Key
            this.Property(t => t.AppId)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);

            this.Property(t => t.Cellphones)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(t => t.Gateway)
                .IsRequired();

            this.Property(t => t.Message)
                .IsRequired()
                .IsUnicode();

            this.Property(t => t.Response)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255);

            this.Property(t => t.Time)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("SmsMessages");
            this.Property(t => t.AppId).HasColumnName("AppId");
            this.Property(t => t.Cellphones).HasColumnName("Cellphones");
            this.Property(t => t.Gateway).HasColumnName("Gateway");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Response).HasColumnName("Response");
            this.Property(t => t.Time).HasColumnName("Time");
        }
    }
}
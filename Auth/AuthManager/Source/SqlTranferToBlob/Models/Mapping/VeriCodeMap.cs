using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.AuthManager.Domain.Core.Models.Mapping
{
    /// <summary>
    ///     VeriCodeMap.
    /// </summary>
    public class VeriCodeMap : EntityTypeConfiguration<VeriCode>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VeriCodeMap" /> class.
        /// </summary>
        public VeriCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Cellphone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(32);

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
            this.Property(t => t.Args).HasColumnName("Args");
        }
    }
}
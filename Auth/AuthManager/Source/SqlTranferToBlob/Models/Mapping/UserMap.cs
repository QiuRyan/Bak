using System.Data.Entity.ModelConfiguration;

namespace Jinyinmao.AuthManager.Domain.Core.Models.Mapping
{
    /// <summary>
    ///     UserMap.
    /// </summary>
    public class UserMap : EntityTypeConfiguration<User>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserMap" /> class.
        /// </summary>
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserIdentifier);

            // Properties
            this.Property(t => t.Args)
                .IsRequired();

            this.Property(t => t.Cellphone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ClientType);

            this.Property(t => t.Closed);

            this.Property(t => t.ContractId);

            this.Property(t => t.EncryptedPassword)
                //.IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Info)
                .IsRequired();

            this.Property(t => t.InviteBy)
                .IsRequired()
                .HasMaxLength(50);

            //this.Property(t => t.InviteFor)
            //    .IsRequired()
            //    .HasMaxLength(50);

            this.Property(t => t.LoginNames)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.OutletCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.RegisterTime)
                .IsRequired();

            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.Args).HasColumnName("Args");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.ClientType).HasColumnName("ClientType");
            this.Property(t => t.Closed).HasColumnName("Closed");
            this.Property(t => t.ContractId).HasColumnName("ContractId");
            this.Property(t => t.EncryptedPassword).HasColumnName("EncryptedPassword");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.InviteBy).HasColumnName("InviteBy");
            this.Property(t => t.LoginNames).HasColumnName("LoginNames");
            this.Property(t => t.OutletCode).HasColumnName("OutletCode");
            this.Property(t => t.RegisterTime).HasColumnName("RegisterTime");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
        }
    }
}
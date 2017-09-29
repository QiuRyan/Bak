using Jinyinmao.AuthManager.Domain.Core.Models.Mapping;
using Moe.Lib.Data;
using System.Data.Entity;

namespace Jinyinmao.AuthManager.Domain.Core.Models
{
    /// <summary>
    ///     JYMDBContext.
    /// </summary>
    public sealed class JYMDBContext : DbContextBase
    {
        /// <summary>
        ///     Initializes static members of the <see cref="JYMDBContext" /> class.
        /// </summary>
        static JYMDBContext()
        {
            Database.SetInitializer<JYMDBContext>(null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JYMDBContext" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public JYMDBContext(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        ///     Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public DbSet<User> Users { get; set; }

        /// <summary>
        ///     Gets or sets the veri codes.
        /// </summary>
        /// <value>The veri codes.</value>
        public DbSet<VeriCode> VeriCodes { get; set; }

        /// <summary>
        ///     Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new VeriCodeMap());
        }
    }
}
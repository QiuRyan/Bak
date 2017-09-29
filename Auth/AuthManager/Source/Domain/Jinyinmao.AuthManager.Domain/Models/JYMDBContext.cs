using System.Data.Entity;
using Jinyinmao.AuthManager.Domain.Models.Mapping;
using Microsoft.Azure;
using Moe.EntityFramework;

namespace Jinyinmao.AuthManager.Domain.Models
{
    /// <summary>
    ///     JYMDBContext.
    /// </summary>
    public sealed class JYMDBContext : DbContextBase
    {
        private static readonly string ConnectionString;

        /// <summary>
        ///     Initializes static members of the <see cref="JYMDBContext" /> class.
        /// </summary>
        static JYMDBContext()
        {
            Database.SetInitializer<JYMDBContext>(null);
            ConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JYMDBContext" /> class.
        /// </summary>
        public JYMDBContext()
            : base(ConnectionString)
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
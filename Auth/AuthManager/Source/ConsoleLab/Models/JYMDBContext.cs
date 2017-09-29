using Moe.Lib.Data;
using System.Data.Entity;

namespace ConsoleLab
{
    /// <summary>
    ///     JYMDBContext.
    /// </summary>
    internal sealed class JYMDBContext : DbContextBase
    {
        /// <summary>
        ///     Initializes static members of the <see cref="JYMDBContext" /> class.
        /// </summary>
        static JYMDBContext()
        {
            Database.SetInitializer<JYMDBContext>(null);
        }

        public JYMDBContext(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        ///     Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public DbSet<User> Users { get; set; }

        /// <summary>
        ///     Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
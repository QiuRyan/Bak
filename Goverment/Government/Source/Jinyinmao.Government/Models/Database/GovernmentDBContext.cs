using System.Data.Entity;
using Jinyinmao.Government.Models.Mapping;
using Moe.Lib.Data;

#if CLOUD

using Microsoft.Azure;

#else

using System.Configuration;

#endif

namespace Jinyinmao.Government.Models
{
    /// <summary>
    ///     GovernmentDbContext.
    /// </summary>
    public class GovernmentDbContext : DbContextBase
    {
#if CLOUD
        private static readonly string GovernmentDatabaseConnectionString = CloudConfigurationManager.GetSetting("GovernmentDatabaseConnectionString");
#else
        private static readonly string GovernmentDatabaseConnectionString = ConfigurationManager.ConnectionStrings["GovernmentDatabaseConnectionString"].ConnectionString;
#endif

        /// <summary>
        ///     Initializes static members of the <see cref="GovernmentDbContext" /> class.
        /// </summary>
        static GovernmentDbContext()
        {
            Database.SetInitializer<GovernmentDbContext>(null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GovernmentDbContext" /> class.
        /// </summary>
        public GovernmentDbContext() : base(GovernmentDatabaseConnectionString)
        {
        }

        public DbSet<Application> Applications { get; set; }

        public DbSet<ConfigurationFetchLog> ConfigurationFetchLogs { get; set; }

        public DbSet<OperationLog> OperationLogs { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Staff> Staves { get; set; }

        /// <summary>
        ///     This method is called when the model for a derived context has been initialized, but
        ///     before the model has been locked down and used to initialize the context.  The default
        ///     implementation of this method does nothing, but it can be overridden in a derived class
        ///     such that the model can be further configured before it is locked down.
        /// </summary>
        /// <remarks>
        ///     Typically, this method is called only once when the first instance of a derived context
        ///     is created.  The model for that context is then cached and is for all further instances of
        ///     the context in the app domain.  This caching can be disabled by setting the ModelCaching
        ///     property on the given ModelBuidler, but note that this can seriously degrade performance.
        ///     More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        ///     classes directly.
        /// </remarks>
        /// <param name="modelBuilder">The builder that defines the model for the context being created. </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ApplicationMap());
            modelBuilder.Configurations.Add(new ConfigurationFetchLogMap());
            modelBuilder.Configurations.Add(new OperationLogMap());
            modelBuilder.Configurations.Add(new PermissionMap());
            modelBuilder.Configurations.Add(new StaffMap());
        }
    }
}
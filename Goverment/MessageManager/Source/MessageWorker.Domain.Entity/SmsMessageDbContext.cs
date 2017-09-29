using System.Data.Entity;
using Moe.Lib.Data;

namespace Jinyinmao.MessageWorker.Domain.Entity
{
    /// <summary>
    /// SmsMessageDbContext. This class cannot be inherited.
    /// </summary>
    public sealed class SmsMessageDbContext : DbContextBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmsMessageDbContext"/> class.
        /// </summary>
        public SmsMessageDbContext()
            : base("name=SmsMessageDbContext")
        {
        }

        /// <summary>
        /// Gets or sets the SMS messages.
        /// </summary>
        /// <value>The SMS messages.</value>
        public DbSet<SmsMessage> SmsMessages { get; set; }

        /// <summary>
        /// Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SmsMessageMap());
        }
    }
}
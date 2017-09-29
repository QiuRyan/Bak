using System.Data.Entity;
using Jinyinmao.Deposit.Domain.Mapping;
using Moe.Lib.Data;

namespace Jinyinmao.Deposit.Domain
{
    public sealed class CouponContext : DbContextBase
    {
        /// <summary>
        ///     Initializes static members of the <see cref="BizContext" /> class.
        /// </summary>
        static CouponContext()
        {
            Database.SetInitializer<CouponContext>(null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BizContext" /> class.
        /// </summary>
        public CouponContext(string connectionString)
            : base(connectionString)
        {
            this.Database.CommandTimeout = 60;
        }

        /// <summary>
        ///     Gets or sets the account transactions.
        /// </summary>
        /// <value>The account transactions.</value>
        public DbSet<CouponAccountTransaction> AccountTransactions
        {
            get; set;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CouponAccountTransactionMap()); 
        }

    }

    public class CouponAccountTransaction
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public long Amount{get; set;}
          
        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        public string TransactionIdentifier{get; set;}
         

        /// <summary>
        ///     Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc{get; set;}

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier{get; set;}
         
    }
}
using System.Data.Entity;
using Moe.Lib.Data;

namespace Jinyinmao.Deposit.Domain
{
    public sealed class CouponDBContext : DbContextBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BizContext" /> class.
        /// </summary>
        public CouponDBContext(string connectionString)
            : base(connectionString)
        {
            this.Database.CommandTimeout = 60;
        }

        public DbSet<AccountTransaction> AccountTransactions { get; set; }
    }
}
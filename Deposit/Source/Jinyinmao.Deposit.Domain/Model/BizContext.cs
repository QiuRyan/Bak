// ******************************************************************************************************
// Project          : Jinyinmao.Deposit
// File             : BizContext.cs
// Created          : 2017-08-10  13:21
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-10  13:24
// ******************************************************************************************************
// <copyright file="BizContext.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Data.Entity;
using Jinyinmao.Deposit.Domain.Mapping;
using Moe.Lib.Data;

namespace Jinyinmao.Deposit.Domain
{
    /// <summary>
    ///     JYMDBContext.
    /// </summary>
    public sealed class BizContext : DbContextBase
    {
        /// <summary>
        ///     Initializes static members of the <see cref="BizContext" /> class.
        /// </summary>
        static BizContext()
        {
            Database.SetInitializer<BizContext>(null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BizContext" /> class.
        /// </summary>
        public BizContext(string connectionString)
            : base(connectionString)
        {
            this.Database.CommandTimeout = 60;
        }

        /// <summary>
        ///     Gets or sets the account transactions.
        /// </summary>
        /// <value>The account transactions.</value>
        public DbSet<AccountTransaction> AccountTransactions { get; set; }

        /// <summary>
        ///     Gets or sets the bank cards.
        /// </summary>
        /// <value>The bank cards.</value>
        public DbSet<BankCard> BankCards { get; set; }

        /// <summary>
        ///     Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        ///     Gets or sets the regular products.
        /// </summary>
        /// <value>The regular products.</value>
        public DbSet<RegularProduct> RegularProducts { get; set; }

        /// <summary>
        ///     Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public DbSet<User> Users { get; set; }

        /// <summary>
        ///     Gets or sets the verified migrate records.
        /// </summary>
        /// <value>The verified migrate records.</value>
        public DbSet<VerifiedMigrateRecord> VerifiedMigrateRecords { get; set; }

        /// <summary>
        ///     Gets or sets the veri codes.
        /// </summary>
        /// <value>The veri codes.</value>
        public DbSet<YemAssetRecord> YemAssetRecord { get; set; }

        /// <summary>
        ///     Gets or sets the yem products.
        /// </summary>
        /// <value>The yem products.</value>
        public DbSet<YemProduct> YemProducts { get; set; }

        /// <summary>
        ///     Gets or sets the yem transactions.
        /// </summary>
        /// <value>The yem transactions.</value>
        public DbSet<YemTransaction> YemTransactions { get; set; }

        /// <summary>
        ///     Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccountTransactionMap());
            modelBuilder.Configurations.Add(new YemProductMap());
            modelBuilder.Configurations.Add(new YemTranscationMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new RegularProductMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new YemAssetRecordMap());
            modelBuilder.Configurations.Add(new VerifiedMigrateRecordMap());
        }
    }
}
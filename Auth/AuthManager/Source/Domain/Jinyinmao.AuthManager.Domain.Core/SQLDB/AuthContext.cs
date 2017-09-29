// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : AuthContext.cs
// Created          : 2016-12-28  11:41
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-28  11:52
// ***********************************************************************
// <copyright file="AuthContext.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Core.SQLDB.Mapping;
using Jinyinmao.AuthManager.Domain.Core.SQLDB.Model;
using Jinyinmao.AuthManager.Libraries;
using Moe.Lib.Data;
using Moe.Lib.Jinyinmao;
using System.Data.Entity;

namespace Jinyinmao.AuthManager.Domain.Core.SQLDB
{
    /// <summary>
    ///     JYMDBContext.
    /// </summary>
    public sealed class AuthContext : DbContextBase
    {
        /// <summary>
        ///     Initializes static members of the <see cref="AuthContext" /> class.
        /// </summary>
        static AuthContext()
        {
            Database.SetInitializer<AuthContext>(null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthContext" /> class.
        /// </summary>
        public AuthContext()
            : base(App.Configurations.GetConfig<AuthSiloConfig>().JYMAuthDBContextConnectionString)
        {
        }

        public AuthContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<ChangeLog> ChangeLog { get; set; }

        public DbSet<DBUser> User { get; set; }

        public DbSet<UserRelation> UserRelation { get; set; }

        /// <summary>
        ///     Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new ChangeLogMap());
            modelBuilder.Configurations.Add(new UserRelationMap());
        }
    }
}
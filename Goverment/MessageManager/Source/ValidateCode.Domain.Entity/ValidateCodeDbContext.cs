// ***********************************************************************
// Project          : MessageManager
// File             : ValidateCodeDbContext.cs
// Created          : 2015-12-06  12:52
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-06  18:35
// ***********************************************************************
// <copyright file="ValidateCodeDbContext.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using Jinyinmao.ValidateCode.Api.Config;
using Moe.Lib.Data;
using ValidateCode.Domain.Entity.DbMap;

namespace Jinyinmao.ValidateCode.Domain.Entity
{
    /// <summary>
    ///     ValidateCodeDbContext. This class cannot be inherited.
    /// </summary>
    public sealed class ValidateCodeDbContext : DbContextBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidateCodeDbContext" /> class.
        /// </summary>
        public ValidateCodeDbContext()
            : base(ConfigsManager.MessageManagerDbContext)
        {
            Database.SetInitializer<ValidateCodeDbContext>(null);
        }

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
            modelBuilder.Configurations.Add(new VeriCodeMap());
        }
    }
}
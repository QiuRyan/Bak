// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : Startup.cs
// Created          : 2016-10-13  16:30
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:31
// ******************************************************************************************************
// <copyright file="Startup.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Data.SqlClient;
using Exceptionless;
using Hangfire;
using Jinyinmao.Daemon.App_Start;
using Jinyinmao.Daemon.Filters;
using Jinyinmao.Daemon.Services;
using Microsoft.Azure;
using Microsoft.Owin;
using Owin;
using Pysco68.Owin.Logging.NLogAdapter;

[assembly: OwinStartup(typeof(Startup))]

namespace Jinyinmao.Daemon.App_Start
{
    public class DbConfig
    {
        /// <summary>
        ///     金银猫交易数据库连接
        /// </summary>
        public static SqlConnection Connection;

        /// <summary>
        ///     好友系统连接字符串
        /// </summary>
        public static SqlConnection MemberConnection;
    }

    public class Startup
    {
        /// <summary>
        ///     Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            ExceptionlessClient.Default.Configuration.ApiKey = CloudConfigurationManager.GetSetting("ExceptionlessKey");
            ExceptionlessClient.Default.Configuration.ServerUrl = CloudConfigurationManager.GetSetting("ExceptionlessServerUrl");

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            DbConfig.Connection = new SqlConnection(CloudConfigurationManager.GetSetting("JYMDBContextConnectionString"));
            DbConfig.MemberConnection = new SqlConnection(CloudConfigurationManager.GetSetting("JYMDBMemberContextConnectionString"));

            GlobalConfiguration.Configuration.UseSqlServerStorage(CloudConfigurationManager.GetSetting("DaemonDBConnectionString"));

            app.UseNLog();

            BackgroundJobServerOptions options = new BackgroundJobServerOptions { WorkerCount = Environment.ProcessorCount * 50 };
            app.UseHangfireServer(options);

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new IpAuthorizationFilter() }
            });

            TirisfalRecurringJob();

            CouponRecurringJob();

            MemberRecurringJob();
        }

        private static void CouponRecurringJob()
        {
            CouponAccountTransactionsServices couponAccountTransactionsServices = new CouponAccountTransactionsServices();
            RecurringJob.AddOrUpdate(
                "CouponAccountTransactionsServices",
                () => couponAccountTransactionsServices.Work(),
                "*/5 * * * *",
                TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

            JYMPartnerService jymPartnerService = new JYMPartnerService();
            RecurringJob.AddOrUpdate(
                "JYMPartnerService",
                () => jymPartnerService.Work(),
                "30 2,3 * * *",
                TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

            JYMSpecialPartnerService specialPartnerService = new JYMSpecialPartnerService();
            RecurringJob.AddOrUpdate(
                "JYMSpecialPartnerService",
                () => specialPartnerService.Work(),
                "30 2,3 * * *",
                TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

            JYMPartnerSyncService jymPartnerSyncService = new JYMPartnerSyncService();
            RecurringJob.AddOrUpdate(
                "JYMPartnerSyncService",
                () => jymPartnerSyncService.Work(),
                "*/10 3,4 * * *",
                TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

            CouponDistributeInvitedCashService couponDistributeInvitedCashService = new CouponDistributeInvitedCashService();
            RecurringJob.AddOrUpdate(
                "CouponDistributeInvitedCashService",
                () => couponDistributeInvitedCashService.Work(),
                "0 4-9 * * *",
                TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
        }

        private static void MemberRecurringJob()
        {
            MemberServices memberServices = new MemberServices();
            RecurringJob.AddOrUpdate("SendCouponScheduleJobs",
                () => memberServices.SendCouponWork(),
                "0 10 2 * *",
                TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

            RecurringJob.AddOrUpdate("BathSysTransactionsScheduleJobs",
                () => memberServices.InsertAccountransactionsWork(),
                "30 0 * * *",
                TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

            RecurringJob.AddOrUpdate("BathMergeTransactionScheduleJobs",
                () => memberServices.Work(),
                "* 2-4 * * *",
                TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
        }

        private static void TirisfalRecurringJob()
        {
            CheckProductSoldOutStatusService checkProductSoldOutStatusService = new CheckProductSoldOutStatusService();
            RecurringJob.AddOrUpdate(
                "CheckProductSoldOutStatusService",
                () => checkProductSoldOutStatusService.Work(),
                "*/5 * * * *",
                TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

            DoDailyWorkByStorageService doDailyWorkByStorageService = new DoDailyWorkByStorageService();
            RecurringJob.AddOrUpdate(
                "DoDailyWork",
                () => doDailyWorkByStorageService.Work(),
                "30 1,4,11 * * *", TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

            SyncCGVerifiedUserRebateService syncCgVerifiedUserRebateService = new SyncCGVerifiedUserRebateService();
            RecurringJob.AddOrUpdate(
                "SyncCGVerifiedUserRebateService",
                () => syncCgVerifiedUserRebateService.Work(),
                "*/5 * * * *",
                TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
        }
    }
}
// ***********************************************************************
// Project          : Jinyinmao.Daemon
// File             : DoDailyUserInfoSyncService.cs
// Created          : 2016-08-01  18:06
//
//
// Last Modified On : 2016-08-03  13:35
// ***********************************************************************
// <copyright file="DoDailyUserInfoSyncService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jinyinmao.Daemon.Services
{
    /// <summary>
    ///     Class PollCheckProductSaleStatusService.
    /// </summary>
    public class DoDailyUserInfoSyncService
    {
        private static readonly string DbConnectionString;
        private static readonly string RequestUri;
        private static readonly RetryPolicy RetryPolicy;
        private readonly Lazy<HttpClient> client;
        private readonly List<RegularProductResult> list;

        static DoDailyUserInfoSyncService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
            RequestUri = "/SyncUser";

            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
            //CRetryStrategy cRetry = new CRetryStrategy("cReEntry", false);
            //RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(cRetry);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DoDailyUserInfoSyncService" /> class.
        /// </summary>
        public DoDailyUserInfoSyncService()
        {
            this.list = new List<RegularProductResult>();
            this.client = new Lazy<HttpClient>(HttpClientHelper.InitHttpClient);
        }

        /// <summary>
        ///     Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public HttpClient Client => this.client.Value;

        /// <summary>
        ///     Works this instance.
        /// </summary>
        public void Work()
        {
            try
            {
                this.QueryRegularPorductDb();
                this.CallDoDailyUserInfoSyncServiceAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollDoDailyUserInfoSyncServiceException {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CallDoDailyUserInfoSyncServiceAsync()
        {
            //log info
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            int count = this.list.Count;
            if (count <= 0) return;

            PollCheckProductSaleStatusInfoLog log = new PollCheckProductSaleStatusInfoLog
            {
                Time = now,
                Count = count,
                Data = this.list
            };

            await StorageHelper.InsertDataIntoAzureBlobAsync("PollDoDailyUserInfoSyncService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));

            foreach (RegularProductResult item in this.list)
            {
                HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(RequestUri + "/{0}".FormatWith(item.ProductIdentifier), new ByteArrayContent(Encoding.UTF8.GetBytes(""))));
                msg.EnsureSuccessStatusCode();
            }
        }

        private void QueryRegularPorductDb()
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                string strSql = "SELECT [UserIdentifier] as pid  FROM [dbo].[Users]";
                SqlDataAdapter adapter = new SqlDataAdapter(strSql, con);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRowView drv in ds.Tables[0].DefaultView)
                {
                    this.list.Add(new RegularProductResult
                    {
                        ProductIdentifier = drv["pid"].ToString()
                    });
                }
            }
        }
    }
}
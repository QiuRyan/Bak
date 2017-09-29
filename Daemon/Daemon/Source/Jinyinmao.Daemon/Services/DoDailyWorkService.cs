// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : PollDoDailyWorkService.cs
// Created          : 2015-09-04  9:52 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-09-11  6:59 PM
// ***********************************************************************
// <copyright file="PollDoDailyWorkService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;

namespace Jinyinmao.Daemon.Services
{
    /// <summary>
    ///     Class PollDoDailyWorkService.
    /// </summary>
    public class DoDailyWorkService
    {
        private static readonly string DbConnectionString;
        private static readonly string RequestUri;
        private static readonly RetryPolicy RetryPolicy;
        private static readonly string TableTemplate;
        private readonly Lazy<HttpClient> client;

        static DoDailyWorkService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("StorageProviderConnectionString");
            RequestUri = "/DoDailyWork";
            TableTemplate = "Grains_{0}";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DoDailyWorkService" /> class.
        /// </summary>
        public DoDailyWorkService()
        {
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
                this.QueryRecentUserDb();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollDoDailyWorkException {0}".FormatWith(e.Message), e);
            }
        }

        /// <summary>
        ///     Works the with grain.
        /// </summary>
        /// <param name="grainId">The grain identifier.</param>
        public void WorkWithGrain(int grainId)
        {
            try
            {
                int nowTicks = (int)DateTime.UtcNow.AddDays(-3).Ticks;
                this.DealTableWithGrainId(grainId, nowTicks);
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollDoDailyGrainWorkException with Grain: {0} {1}".FormatWith(grainId, e.Message), e);
            }
        }

        private async Task CallDoDailyWorkAsync(string userId)
        {
            HttpResponseMessage msg = await this.Client.PostAsync(RequestUri + "/{0}".FormatWith(userId), new ByteArrayContent(Encoding.UTF8.GetBytes("")));
            msg.EnsureSuccessStatusCode();
        }

        private void DealTableWithGrainId(int i, int nowTicks)
        {
            //Multi Task version
            string connectionStr = string.Format(DbConnectionString, i);
            List<Task> list = new List<Task>();

            for (int j = 0; j < 3; j++)
            {
                int i1 = i;
                int j1 = j;
                Task task = Task.Run(() => this.DealWithTable(j1, nowTicks, connectionStr, i1));
                list.Add(task);
            }
            Task.WhenAll(list).Wait();
            list.Clear();

            for (int j = 3; j < 6; j++)
            {
                int i1 = i;
                int j1 = j;
                Task task = Task.Run(() => this.DealWithTable(j1, nowTicks, connectionStr, i1));
                list.Add(task);
            }
            Task.WhenAll(list).Wait();
            list.Clear();

            for (int j = 6; j < 9; j++)
            {
                int i1 = i;
                int j1 = j;
                Task task = Task.Run(() => this.DealWithTable(j1, nowTicks, connectionStr, i1));
                list.Add(task);
            }
            Task.WhenAll(list).Wait();
        }

        private async Task DealWithTable(int j, int nowTicks, string connectionStr, int i)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionStr))
                {
                    string strSql = "select distinct Id as id from " + TableTemplate.FormatWith(j) + " where Type = 'Yuyi.Jinyinmao.Domain.User' and TimeStamp > " + nowTicks;
                    SqlDataAdapter adapter = new SqlDataAdapter(strSql, con);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    string id = string.Empty;
                    foreach (DataRowView drv in ds.Tables[0].DefaultView)
                    {
                        try
                        {
                            id = drv["id"].ToString();
                            string id1 = id;
                            await RetryPolicy.ExecuteAsync(() => this.CallDoDailyWorkAsync(id1));
                        }
                        catch (Exception e)
                        {
                            LogHelper.LogError("DoDailyWorkUserException {0}".FormatWith("Current DB,Table,userId: {0} {1} {2} ".FormatWith(i, j, id) + e.Message), e);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.LogError("DoDailyWorkTableException {0}".FormatWith("Current DB,Table: {0} {1} ".FormatWith(i, j) + e.Message), e);
            }
        }

        //[AutomaticRetry]
        private void QueryRecentUserDb()
        {
            //one db a time, 9 tables together
            int nowTicks = (int)DateTime.UtcNow.AddDays(-3).Ticks;
            for (int i = 0; i < 6; i++)
            {
                this.DealTableWithGrainId(i, nowTicks);
            }
        }
    }
}
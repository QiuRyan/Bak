// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : PollXianFengPaymentService.cs
// Created          : 2015-09-13  17:46
//
// Last Modified By :
// Last Modified On : 2015-09-13  17:59
// ***********************************************************************
// <copyright file="PollXianFengPaymentService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Jinyinmao.Daemon.App_Start;
using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;
using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Services
{
    /// <summary>
    ///     PollXianFengPaymentService.
    /// </summary>
    public class XianFengPaymentService
    {
        /// <summary>
        ///     The API host
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private static readonly string DbConnectionString;

        /// <summary>
        ///     The request URI
        /// </summary>
        private static readonly string RequestUri;

        /// <summary>
        ///     The retry policy
        /// </summary>
        private static readonly RetryPolicy RetryPolicy;

        private static readonly string XianFengPaymentApiHost;

        /// <summary>
        ///     The client
        /// </summary>
        private readonly Lazy<HttpClient> client;

        /// <summary>
        ///     The list
        /// </summary>
        private readonly List<AccountTransactionResult> list;

        /// <summary>
        ///     Initializes static members of the <see cref="XianFengPaymentService" /> class.
        /// </summary>
        static XianFengPaymentService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
            XianFengPaymentApiHost = CloudConfigurationManager.GetSetting("ApiHost");
            RequestUri = "BackOffice/QueryOrderByXianFeng";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(1, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XianFengPaymentService" /> class.
        /// </summary>
        public XianFengPaymentService()
        {
            this.list = new List<AccountTransactionResult>();
            this.client = new Lazy<HttpClient>(InitHttpClient);
        }

        /// <summary>
        ///     The client
        /// </summary>
        public HttpClient Client => this.client.Value;

        public static HttpClient InitHttpClient()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(XianFengPaymentApiHost) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            return httpClient;
        }

        /// <summary>
        ///     查询数据库流水表并发出请求
        /// </summary>
        public void Work()
        {
            try
            {
                this.QueryTransactionDb();
                this.CallQueryOrderByXianFengBatchAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollXianFengPaymentException {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CallQueryOrderByXianFengBatchAsync()
        {
            //log info
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            int count = this.list.Count;
            if (count <= 0) return;

            PollXianFengPaymentInfoLog log = new PollXianFengPaymentInfoLog
            {
                Time = now,
                Count = count,
                Data = this.list
            };

            await StorageHelper.InsertDataIntoAzureBlobAsync("PollXianFengPaymentService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));

            foreach (AccountTransactionResult item in this.list)
            {
                HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsJsonAsync(RequestUri, new XianFengRequest { transactionIdentifier = item.TransactionIdentifier, userIdentifier = item.UserIdentifier }));
                //msg.EnsureSuccessStatusCode();
                // ReSharper disable once UnusedVariable
                string responseString = await msg.Content.ReadAsStringAsync();
            }
        }

        private void QueryTransactionDb()
        {
            try
            {
                this.list.Clear();
                if (DbConfig.Connection.State == ConnectionState.Closed)
                {
                    DbConfig.Connection.Open();
                }
                string strSql = "select top 400 UserIdentifier as uid,TransactionIdentifier as tid from dbo.AccountTransactions where TradeCode = '1005051001' and ResultCode = 0 and ChannelCode = 10030 AND DATEDIFF(DAY,TransactionTime,GETDATE())<=7 order by TransactionTime desc";
                SqlDataAdapter adapter = new SqlDataAdapter(strSql, DbConfig.Connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRowView drv in ds.Tables[0].DefaultView)
                {
                    this.list.Add(new AccountTransactionResult
                    {
                        UserIdentifier = drv["uid"].ToString(),
                        TransactionIdentifier = drv["tid"].ToString()
                    });
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
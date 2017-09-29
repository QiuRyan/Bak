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
using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public class YiLianPaymentService
    {
        private static readonly string BankCardsDbConnectionString;

        /// <summary>
        ///     The API host
        /// </summary>
        private static readonly string DbConnectionString;

#pragma warning disable 414
        private static readonly string DepositRequestUri;
#pragma warning restore 414

        /// <summary>
        ///     The retry policy
        /// </summary>
        private static readonly RetryPolicy RetryPolicy;

        /// <summary>
        ///     The request URI
        /// </summary>
        private static readonly string VeryfyRequestUri;

        private static readonly string YiLianPaymentApiHost;

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
        static YiLianPaymentService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
            BankCardsDbConnectionString = CloudConfigurationManager.GetSetting("BankCardsDBConnectionString");
            //VeryfyRequestUri = "Payment/YiLian/VerifyQuery?userIdentifier=";
            //DepositRequestUri = "Payment/YiLian/DepositQuery?userIdentifier=";
            VeryfyRequestUri = "api/V1/UserAuth/YiLian/QueryAuthenticatedResult";
            DepositRequestUri = "api/V1/User/Settle/YiLian/QueryDepositedResult/";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(1, TimeSpan.FromSeconds(5));
            YiLianPaymentApiHost = CloudConfigurationManager.GetSetting("ApiHost");
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XianFengPaymentService" /> class.
        /// </summary>
        public YiLianPaymentService()
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
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(YiLianPaymentApiHost) };
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
                this.CallQueryOrderByYiLianBatchAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollYiLianPaymentException {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CallQueryOrderByYiLianBatchAsync()
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

            await StorageHelper.InsertDataIntoAzureBlobAsync("PollYiLianPaymentService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));

            foreach (AccountTransactionResult item in this.list)
            {
                //if (item.YiLianVerified == 0)
                //{
                HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsJsonAsync(VeryfyRequestUri, new YiLianRequest { userIdentifier = item.UserIdentifier, transactionIdentifier = item.TransactionIdentifier }));
                //Debug.WriteLine(item.ToJson());
                // ReSharper disable once UnusedVariable
                string responseString = await msg.Content.ReadAsStringAsync();
                //msg.EnsureSuccessStatusCode();
                //}
                //else if (item.YiLianVerified == 1)
                //{
                //    HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsJsonAsync(DepositRequestUri + item.TransactionIdentifier + "/" + item.UserIdentifier, ""));
                //    //Debug.WriteLine(item.ToJson());
                //    var responseString = await msg.Content.ReadAsStringAsync();

                //    //msg.EnsureSuccessStatusCode();
                //}
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
                string strSql = "select top 400 UserIdentifier as uid,TransactionIdentifier as tid,BankCardNo as bid from dbo.AccountTransactions where TradeCode = '1005051001' and ResultCode = 0 and ChannelCode = 10010 AND DATEDIFF(DAY,TransactionTime,GETDATE())<=7 order by TransactionTime desc;";
                SqlDataAdapter adapter = new SqlDataAdapter(strSql, DbConfig.Connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRowView drv in ds.Tables[0].DefaultView)
                {
                    this.list.Add(new AccountTransactionResult
                    {
                        UserIdentifier = drv["uid"].ToString(),
                        TransactionIdentifier = drv["tid"].ToString(),
                        BankCardNo = drv["bid"].ToString()
                    });
                }
            }
            catch (Exception)
            {
                // ignored
            }

            //if (this.list.Count == 0) return;
            //using (SqlConnection con = new SqlConnection(BankCardsDbConnectionString))
            //{
            //    var bankCards = new List<string>();

            //    foreach (var item in this.list)
            //    {
            //        bankCards.Add("'" + item.BankCardNo + "'");
            //    }

            //    string strSql = "select YiLianVerified,BankCardNo from dbo.BankCards where BankCardNo in(" + string.Join(",", bankCards) + ")";
            //    SqlDataAdapter adapter = new SqlDataAdapter(strSql, con);
            //    DataSet ds = new DataSet();
            //    adapter.Fill(ds);
            //    foreach (DataRowView drv in ds.Tables[0].DefaultView)
            //    {
            //        this.list.Find(a => a.BankCardNo == drv["BankCardNo"].ToString()).YiLianVerified = drv["YiLianVerified"].ToString().ToInt();
            //    }
            //}
        }
    }
}
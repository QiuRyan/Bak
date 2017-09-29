// ***********************************************************************
// Project          : Jinyinmao.Daemon
// File             : PaymentOrderSynchro.cs
// Created          : 2016-08-31  13:22
//
// Last Modified By : 杨亮
// Last Modified On : 2016-08-31  13:22
// ***********************************************************************
// <copyright file="PaymentOrderSynchro.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.Daemon.App_Start;
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
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Jinyinmao.Daemon.Services
{
    public class PaymentOrderSynchroService
    {
        /// <summary>
        ///     AES加密Key
        /// </summary>
        private static readonly string CryptoKey;

        /// <summary>
        ///     请求路径
        /// </summary>
        private static readonly string RequestUri;

        /// <summary>
        ///     The retry policy
        /// </summary>
        private static readonly RetryPolicy RetryPolicy;

        /// <summary>
        ///     交易系统根地址
        /// </summary>
        private static readonly string TirisfalApiHost;

        /// <summary>
        ///     The client
        /// </summary>
        private readonly Lazy<HttpClient> client;

        /// <summary>
        ///     The list
        /// </summary>
        private readonly List<InsertSettleAccountTransactionEntities> list;

        /// <summary>
        ///     Initializes the <see cref="PaymentOrderSynchroService"/> class.
        /// </summary>
        static PaymentOrderSynchroService()
        {
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
            TirisfalApiHost = CloudConfigurationManager.GetSetting("ApiHost");
            RequestUri = "/InsertSettleAccountTransaction";
            HistoryHours = CloudConfigurationManager.GetSetting("HistoryHours").ToInt();
            CryptoKey = CloudConfigurationManager.GetSetting("CryptoKey");
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XianFengPaymentService" /> class.
        /// </summary>
        public PaymentOrderSynchroService()
        {
            this.list = new List<InsertSettleAccountTransactionEntities>();
            this.client = new Lazy<HttpClient>(this.InitHttpClient);
        }

        /// <summary>
        ///     同步历史时间
        /// </summary>
        public static int HistoryHours { get; set; }

        /// <summary>
        ///     The client
        /// </summary>
        public HttpClient Client => this.client.Value;

        public void Work()
        {
            try
            {
                this.QueryTransactionDb();
                this.CallPaymentOrderSynchroBatchAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollPaymentOrderSynchroException {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CallPaymentOrderSynchroBatchAsync()
        {
            //log info
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            int count = this.list.Count;
            if (count <= 0) return;

            PollCallPaymentOrderSynchroLog log = new PollCallPaymentOrderSynchroLog
            {
                Time = now,
                Count = count,
                Data = this.list
            };

            await StorageHelper.InsertDataIntoAzureBlobAsync("PollPaymentOrderSynchroService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));

            foreach (InsertSettleAccountTransactionEntities item in this.list)
            {
                string responseString = string.Empty;
                try
                {
                    HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsJsonAsync(RequestUri, item));
                    responseString = await msg.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    LogHelper.LogError("PollPaymentOrderSynchro PostAsJsonAsync Exception {0}-{1} ".FormatWith(e.Message, responseString), e);
                }
            }
        }

        private HttpClient InitHttpClient()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(TirisfalApiHost) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            return httpClient;
        }

        private void QueryTransactionDb()
        {
            try
            {
                this.list.Clear();
                if (DbConfig.PaymentConnection.State == ConnectionState.Closed)
                {
                    DbConfig.PaymentConnection.Open();
                }
                DateTime utcNow = DateTime.UtcNow.ToChinaStandardTime().AddHours(-HistoryHours);
                DateTime historyTime = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second);
                string strSql = $"SELECT a.Amount,a.BankCardNoEncrypt,a.ChannelCode,a.ResultCode,a.TradeCode,a.TransactionIdentifier,a.TransDesc,a.UserIdentifier,a.TransactionTime  FROM dbo.AccountTransactions a WHERE  a.TradeCode= 1005051001 AND a.Amount > 0 AND  a.TransactionTime >= '{historyTime}'";
                SqlDataAdapter adapter = new SqlDataAdapter(strSql, DbConfig.PaymentConnection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRowView drv in ds.Tables[0].DefaultView)
                {
                    try
                    {
                        this.list.Add(new InsertSettleAccountTransactionEntities
                        {
                            Amount = drv["Amount"].ToString().ToLong(),
                            BankCardNo = drv["BankCardNoEncrypt"].ToString().Decrypt(CryptoKey),
                            ChannelCode = drv["ChannelCode"].ToString().ToInt(),
                            OrderId = Guid.Empty.ToGuidString(),
                            ResultCode = drv["ResultCode"].ToString().ToInt(),
                            Trade = 0,
                            TradeCode = drv["TradeCode"].ToString().ToInt(),
                            TransactionIdentifier = drv["TransactionIdentifier"].ToString(),
                            TransDesc = drv["TransDesc"].ToString(),
                            UserIdentifier = drv["UserIdentifier"].ToString()
                        });
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
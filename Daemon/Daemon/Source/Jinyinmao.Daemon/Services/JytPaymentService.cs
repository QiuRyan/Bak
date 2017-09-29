using Jinyinmao.Daemon.App_Start;
using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
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
    /// <summary>
    ///     金运通支付结果查询定时任务服务类
    /// </summary>
    public class JytPaymentService
    {
        /// <summary>
        ///     The API host
        /// </summary>
        private static readonly string ApiHost;

        /// <summary>
        ///     The request URI
        /// </summary>
        private static readonly string JytRequestUri;

        /// <summary>
        ///     The client
        /// </summary>
        private readonly Lazy<HttpClient> client;

        /// <summary>
        ///     The list
        /// </summary>
        private readonly List<JytTransactionResult> jytPays;

        /// <summary>
        ///     Initializes the <see cref="JytPaymentService" /> class.
        /// </summary>
        static JytPaymentService()
        {
            ApiHost = CloudConfigurationManager.GetSetting("ApiHost");
            JytRequestUri = "/api/V1/User/JytSettle/DepositQueryByJyt/{0}/{1}";
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JytPaymentService" /> class.
        /// </summary>
        public JytPaymentService()
        {
            this.jytPays = new List<JytTransactionResult>();
            this.client = new Lazy<HttpClient>(InitHttpClient);
        }

        /// <summary>
        ///     Gets the client.
        /// </summary>
        public HttpClient Client => this.client.Value;

        /// <summary>
        ///     Initializes the HTTP client.
        /// </summary>
        /// <returns></returns>
        public static HttpClient InitHttpClient()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(ApiHost) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            return httpClient;
        }

        /// <summary>
        ///     Works this instance.
        /// </summary>
        public void Work()
        {
            try
            {
                this.QueryJytTransactionDb();
                this.CallQueryOrderByJytBatchAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollJytPaymentException {0}".FormatWith(e.Message), e);
            }
        }

        /// <summary>
        ///     Calls the query order by jyt batch asynchronous.
        /// </summary>
        /// <returns></returns>
        private async Task CallQueryOrderByJytBatchAsync()
        {
            //log info
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            int count = this.jytPays.Count;
            if (count <= 0) return;

            PollJytPaymentInfoLog log = new PollJytPaymentInfoLog
            {
                Time = now,
                Count = count,
                Data = this.jytPays
            };

            await StorageHelper.InsertDataIntoAzureBlobAsync("PollJytPaymentService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));

            //this.jytPays.RemoveAll(x => x.ChannelCode.Equals("10060") == false);

            foreach (JytTransactionResult pay in this.jytPays)
            {
                try
                {
                    HttpResponseMessage msg = await this.Client.PostAsJsonAsync(JytRequestUri.FormatWith(pay.TransactionIdentifier, pay.UserIdentifier), "");
                    string result = await msg.Content.ReadAsStringAsync();
                    //Debug
                    await StorageHelper.InsertDataIntoAzureBlobAsync("PollJytPaymentService_DeBug" + "/" + now.ToString("yyyyMMddHH") + "/" + pay.TransactionIdentifier, JsonConvert.SerializeObject(new
                    {
                        pay.TransactionIdentifier,
                        pay.UserIdentifier,
                        pay.ChannelCode,
                        RequestUri = JytRequestUri.FormatWith(pay.TransactionIdentifier, pay.UserIdentifier),
                        Result = result
                    }));
                }
                catch (Exception)
                {
                    // ignored
                }

            }
        }

        /// <summary>
        ///     Queries the transaction database.
        /// </summary>
        private void QueryJytTransactionDb()
        {
            try
            {
                this.jytPays.Clear();
                if (DbConfig.Connection.State == ConnectionState.Closed)
                {
                    DbConfig.Connection.Open();
                }
                string strjytSql = "SELECT TOP 400 UserIdentifier AS [uid],TransactionIdentifier AS tid,ChannelCode AS ccid FROM dbo.AccountTransactions WHERE ChannelCode=10060 AND TradeCode='1005051001' AND ResultCode=0 AND DATEDIFF(DAY,TransactionTime,GETDATE()) <= 7 ORDER BY TransactionTime DESC;";
                SqlDataAdapter adapter = new SqlDataAdapter(strjytSql, DbConfig.Connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRowView drv in ds.Tables[0].DefaultView)
                {
                    this.jytPays.Add(new JytTransactionResult
                    {
                        ChannelCode = drv["ccid"].ToString(),
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
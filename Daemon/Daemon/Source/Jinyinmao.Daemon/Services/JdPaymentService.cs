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
    ///     jindong query payment results
    /// </summary>
    public class JdPaymentService
    {
        /// <summary>
        ///     The API host
        /// </summary>
        private static readonly string ApiHost;

        /// <summary>
        ///     The request URI
        /// </summary>
        private static readonly string JdRequestUri;

        /// <summary>
        ///     The client
        /// </summary>
        private readonly Lazy<HttpClient> client;

        /// <summary>
        ///     The list
        /// </summary>
        private readonly List<JytTransactionResult> jdPays;

        /// <summary>
        ///     Initializes the <see cref="JdPaymentService" /> class.
        /// </summary>
        static JdPaymentService()
        {
            ApiHost = CloudConfigurationManager.GetSetting("ApiHost");
            JdRequestUri = "/api/V1/User/JdSettle/DepositQueryByJd/{0}/{1}";
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JdPaymentService" /> class.
        /// </summary>
        public JdPaymentService()
        {
            this.jdPays = new List<JytTransactionResult>();
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
            httpClient.Timeout = new TimeSpan(0, 0, 0, 30);
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
                this.CallQueryOrderByJdBatchAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollJdPaymentException {0}".FormatWith(e.Message), e);
            }
        }

        /// <summary>
        ///     Calls the query order by jd batch asynchronous.
        /// </summary>
        /// <returns></returns>
        private async Task CallQueryOrderByJdBatchAsync()
        {
            //log info
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            int count = this.jdPays.Count;
            if (count <= 0) return;

            PollJytPaymentInfoLog log = new PollJytPaymentInfoLog
            {
                Time = now,
                Count = count,
                Data = this.jdPays
            };

            await StorageHelper.InsertDataIntoAzureBlobAsync("PollJdPaymentService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));

            foreach (JytTransactionResult pay in this.jdPays)
            {
                try
                {
                    HttpResponseMessage msg = await this.Client.PostAsJsonAsync(JdRequestUri.FormatWith(pay.TransactionIdentifier, pay.UserIdentifier), "");
                    await msg.Content.ReadAsStringAsync();
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
                this.jdPays.Clear();
                if (DbConfig.Connection.State == ConnectionState.Closed)
                {
                    DbConfig.Connection.Open();
                }
                string strjytSql = "SELECT TOP 400 UserIdentifier AS [uid],TransactionIdentifier AS tid,ChannelCode AS ccid FROM dbo.AccountTransactions WHERE ChannelCode=10070 AND TradeCode='1005051001' AND ResultCode=0 AND DATEDIFF(DAY,TransactionTime,GETDATE()) <= 7 ORDER BY TransactionTime DESC;";
                SqlDataAdapter adapter = new SqlDataAdapter(strjytSql, DbConfig.Connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRowView drv in ds.Tables[0].DefaultView)
                {
                    this.jdPays.Add(new JytTransactionResult
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
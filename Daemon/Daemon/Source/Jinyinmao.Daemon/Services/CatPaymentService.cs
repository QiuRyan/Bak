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
    public class CatPaymentService
    {
        /// <summary>
        ///     The PaymentApiHost
        /// </summary>
        private static readonly string CatPaymentApiHost;

        /// <summary>
        ///     The DbConnectionString
        /// </summary>
        private static readonly string DbConnectionString;

        /// <summary>
        ///     The Request Uri
        /// </summary>
        private static readonly string RequestUri;

        /// <summary>
        ///     The retry policy
        /// </summary>
        private static readonly RetryPolicy RetryPolicy;

        /// <summary>
        ///     The client
        /// </summary>
        private readonly Lazy<HttpClient> client;

        /// <summary>
        ///     The list
        /// </summary>
        private readonly List<AccountTransactionResult> list;

        static CatPaymentService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("JYMPaymentConnectionString");
            RequestUri = "api/V1/User/CatSettle/Cat/DepositQuery/";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
            CatPaymentApiHost = CloudConfigurationManager.GetSetting("ApiHost");
        }

        public CatPaymentService()
        {
            this.list = new List<AccountTransactionResult>();
            this.client = new Lazy<HttpClient>(this.InitHttpClient);
        }

        /// <summary>
        ///     The client
        /// </summary>
        public HttpClient Client => this.client.Value;

        public void Work()
        {
            try
            {
                this.QueryTransactionDb();
                this.CallQueryOrderByCatBatchAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollCatPaymentException {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CallQueryOrderByCatBatchAsync()
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

            await StorageHelper.InsertDataIntoAzureBlobAsync("PollCatPaymentService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));

            foreach (AccountTransactionResult item in this.list)
            {
                HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsJsonAsync(RequestUri + item.TransactionIdentifier + "/" + item.UserIdentifier, ""));
                string responseString = await msg.Content.ReadAsStringAsync();
            }
        }

        private HttpClient InitHttpClient()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(CatPaymentApiHost) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            return httpClient;
        }

        private void QueryTransactionDb()
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                string strSql = "select top 400 UserIdentifier as uid,TransactionIdentifier as tid from dbo.AccountTransactions where TradeCode = '1005051001' and ResultCode = 0 and ChannelCode = 10050 order by TransactionTime desc";
                SqlDataAdapter adapter = new SqlDataAdapter(strSql, con);
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
        }
    }
}
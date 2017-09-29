using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;
using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Services
{
    public class CheckTransactionStatusService
    {
        private static readonly string DbConnectionString;
        private static readonly string GatewayClientUri;
        private static readonly string RequestUri;
        private static readonly RetryPolicy RetryPolicy;
        private readonly Lazy<HttpClient> client;
        private readonly Lazy<HttpClient> gatewayClient;
        private readonly List<CheckTransactionStatus> list;

        static CheckTransactionStatusService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
            RequestUri = "/User/Payment/SetRechargeResult";
            GatewayClientUri = "gateway/api/business/ordersearch";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckTransactionStatusService" /> class.
        /// </summary>
        public CheckTransactionStatusService()
        {
            this.list = new List<CheckTransactionStatus>();
            this.client = new Lazy<HttpClient>(HttpClientHelper.InitHttpClient);
            this.gatewayClient = new Lazy<HttpClient>(() => HttpClientHelper.InitHttpClient(GatewayClientUri));
        }

        /// <summary>
        ///     Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public HttpClient Client => this.client.Value;

        public HttpClient GatewayClient => this.gatewayClient.Value;

        /// <summary>
        ///     Works this instance.
        /// </summary>
        public void Work()
        {
            try
            {
                this.QueryTransactiontDb();
                this.CallCheckTransactionStatusAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollCheckProductSaleStatusException {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CallCheckTransactionStatusAsync()
        {
            if (this.list.Count <= 0) return;

            await StorageHelper.InsertDataIntoAzureBlobAsync("PollCheckProductSaleStatusService" + "/" + DateTime.UtcNow.ToChinaStandardTime().ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(this.list));

            foreach (CheckTransactionStatus item in this.list)
            {
                HttpResponseMessage gatewayMessage = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsJsonAsync(RequestUri, item));
                OrderSearchResponse result = await gatewayMessage.Content.ReadAsAsync<OrderSearchResponse>();
                if (result.BizType != "1000" && result.BizType != "2000")
                {
                    return;
                }

                if (result.RespSubCode == "200005")
                {
                    HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsJsonAsync(RequestUri, item));
                    msg.EnsureSuccessStatusCode();
                }
            }
        }

        private void QueryTransactiontDb()
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                DateTime now = DateTime.UtcNow.ToChinaStandardTime();
                int duringMinutes = Convert.ToInt32(CloudConfigurationManager.GetSetting("DuringMinutes"));
                string beginTime = now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                string endTime = now.AddMinutes(-duringMinutes).ToString("yyyy-MM-dd HH:mm:ss");

                string strSql = "SELECT TransactionIdentifier,UserIdentifier,TradeCode FROM AccountTransactions WHERE TransactionTime < '" + endTime + "' AND  TransactionTime > '" + beginTime + "' AND TradeCode IN (1005051001,1005052001) AND ResultCode = 0";
                SqlDataAdapter adapter = new SqlDataAdapter(strSql, con);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRowView drv in ds.Tables[0].DefaultView)
                {
                    int tradeCode = Convert.ToInt32(drv["TradeCode"]);
                    this.list.Add(new CheckTransactionStatus
                    {
                        TransactionIdentifier = Convert.ToString(drv["TransactionIdentifier"]),
                        Notifytype = tradeCode == 1005051001 ? 0 : 1,
                        UserIdentifier = Convert.ToString(drv["UserIdentifier"]),
                        ResultCode = -1
                    });
                }
            }
        }
    }
}
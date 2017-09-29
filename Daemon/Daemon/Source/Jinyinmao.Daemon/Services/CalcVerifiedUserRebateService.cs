using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Jinyinmao.Daemon.App_Start;
using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Moe.Lib;

namespace Jinyinmao.Daemon.Services
{
    public class CalcVerifiedUserRebateService
    {
        /// <summary>
        /// The xian feng payment API host
        /// </summary>
        private static readonly string ApiHost;

        /// <summary>
        /// The API host
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private static readonly string DbConnectionString;

        /// <summary>
        /// The client
        /// </summary>
        private readonly Lazy<HttpClient> client;

        /// <summary>
        /// Initializes static members of the <see cref="XianFengPaymentService"/> class.
        /// </summary>
        static CalcVerifiedUserRebateService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
            ApiHost = CloudConfigurationManager.GetSetting("ApiHost");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XianFengPaymentService"/> class.
        /// </summary>
        public CalcVerifiedUserRebateService()
        {
            this.client = new Lazy<HttpClient>(InitHttpClient);
        }

        /// <summary>
        /// The client
        /// </summary>
        public HttpClient Client => this.client.Value;

        /// <summary>
        /// Initializes the HTTP client.
        /// </summary>
        /// <returns>HttpClient.</returns>
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
        /// 查询数据库流水表并发出请求
        /// </summary>
        public void Work()
        {
            try
            {
                this.CalcMigrateAmountAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("CalcVerifiedUserRebateService {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CalcMigrateAmountAsync()
        {
            try
            {
                int pageSize = 100;
                DataSet ds;
                do
                {
                    if (DbConfig.Connection.State == ConnectionState.Closed)
                    {
                        DbConfig.Connection.Open();
                    }

                    string selectSql = $"SELECT TOP {pageSize} * from dbo.VerifiedMigrateRecord WHERE Status = 0 order by Id desc";
                    SqlDataAdapter adapter = new SqlDataAdapter(selectSql, DbConfig.Connection);
                    ds = new DataSet();
                    adapter.Fill(ds);

                    SqlCommand updateCommand = new SqlCommand("UPDATE VerifiedMigrateRecord SET Amount = @Amount, Status = @Status WHERE Id = @Id", DbConfig.Connection);
                    updateCommand.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Int, int.MaxValue, "Amount"));
                    updateCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, int.MaxValue, "Status"));
                    updateCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, int.MaxValue, "Id"));
                    foreach (DataRowView drv in ds.Tables[0].DefaultView)
                    {
                        string userIdentifier = Convert.ToString(drv["UserIdentifier"]);
                        long amount = await this.GetUserBalanceAsync(userIdentifier);
                        drv["Amount"] = amount;
                        drv["Status"] = amount == 0 ? 1 : 10;
                        drv["MigrateTime"] = DateTime.UtcNow.ToChinaStandardTime();
                        drv["LastUpdateTime"] = DateTime.UtcNow.ToChinaStandardTime();
                    }

                    adapter.UpdateCommand = updateCommand;
                    adapter.Update(ds.Tables[0]);
                } while (pageSize == ds.Tables[0].Rows.Count);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private async Task<long> GetUserBalanceAsync(string userIdentifier)
        {
            HttpResponseMessage response = await this.Client.GetAsync("UserBalanceBeforeVerified/{0}".FormatWith(userIdentifier));
            UserBalanceResponse userBalance = await response.Content.ReadAsAsync<UserBalanceResponse>();

            return userBalance.result ? userBalance.responseData : 0;
        }
    }
}
// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : SyncCGVerifiedUserRebateService.cs
// Created          : 2017-08-05  14:15
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:31
// ******************************************************************************************************
// <copyright file="SyncCGVerifiedUserRebateService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// <summary>
    ///     Class SyncCGVerifiedUserRebateService.
    /// </summary>
    public class SyncCGVerifiedUserRebateService
    {
        /// <summary>
        ///     The API host
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private static readonly string DbConnectionString;

        /// <summary>
        ///     The jymcg identifier
        /// </summary>
        private static readonly string JYMMarketingIdentifier;

        /// <summary>
        ///     The jymcg maximum amount
        /// </summary>
        private static readonly long JYMMigrateMaxAmount;

        private readonly Lazy<HttpClient> apiClient;

        /// <summary>
        ///     The client
        /// </summary>
        private readonly Lazy<HttpClient> gatewayClient;

        /// <summary>
        ///     Initializes static members of the <see cref="SyncCGVerifiedUserRebateService" /> class.
        /// </summary>
        static SyncCGVerifiedUserRebateService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
            JYMMarketingIdentifier = CloudConfigurationManager.GetSetting("JYMMarketingIdentifier");
            JYMMigrateMaxAmount = Convert.ToInt64(CloudConfigurationManager.GetSetting("JYMMigrateMaxAmount"));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SyncCGVerifiedUserRebateService" /> class.
        /// </summary>
        public SyncCGVerifiedUserRebateService()
        {
            this.gatewayClient = new Lazy<HttpClient>(() => InitHttpClient(CloudConfigurationManager.GetSetting("GatewayHost")));
            this.apiClient = new Lazy<HttpClient>(() => InitHttpClient(CloudConfigurationManager.GetSetting("ApiHost")));
        }

        public HttpClient ApiClient => this.apiClient.Value;

        /// <summary>
        ///     The client
        /// </summary>
        /// <value>The client.</value>
        public HttpClient GatewayClient => this.gatewayClient.Value;

        /// <summary>
        ///     Initializes the HTTP client.
        /// </summary>
        /// <returns>HttpClient.</returns>
        public static HttpClient InitHttpClient(string host)
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(host) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            return httpClient;
        }

        /// <summary>
        ///     查询数据库流水表并发出请求
        /// </summary>
        [DisplayName("用户开通存管后,流水同步到存管")]
        public void Work()
        {
            try
            {
                this.CalcMigrateAmountAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.Error(e, $"用户开通存管后,流水同步到存管{DateTime.UtcNow.ToChinaStandardTime()}", e.ToJson(), "SyncCGVerifiedUserRebateService");
            }
        }

        /// <summary>
        ///     Builds the rebate request.
        /// </summary>
        /// <param name="drv">The DRV.</param>
        /// <param name="amount"></param>
        /// <returns>RebateRequest.</returns>
        private RebateRequest BuildRebateRequest(DataRowView drv, long amount)
        {
            return new RebateRequest
            {
                Currency = "CNY",
                OrderId = Guid.NewGuid().ToGuidString(),
                OrderList = new List<OrderInfoRequest>
                {
                    new OrderInfoRequest
                    {
                        Amount = amount,
                        BizType = "1902",
                        PayUserIdentifier = JYMMarketingIdentifier,
                        TransactionIdentifier = Convert.ToString(drv["RecordIdentfier"]),
                        UserIdentifier = Convert.ToString(drv["UserIdentifier"])
                    }
                },
                RebateType = "05"
            };
        }

        /// <summary>
        ///     calculate migrate amount as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
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

                    string selectSql = $"SELECT TOP {pageSize} * from dbo.VerifiedMigrateRecord WHERE Status = 0 AND Amount <= {JYMMigrateMaxAmount} order by Id desc";
                    SqlDataAdapter adapter = new SqlDataAdapter(selectSql, DbConfig.Connection);
                    ds = new DataSet();
                    adapter.Fill(ds);

                    SqlCommand updateCommand = new SqlCommand("UPDATE VerifiedMigrateRecord SET LastUpdateTime = @LastUpdateTime, Status = @Status , Amount = @Amount WHERE Id = @Id", DbConfig.Connection);
                    updateCommand.Parameters.Add(new SqlParameter("@LastUpdateTime", SqlDbType.DateTime2, int.MaxValue, "LastUpdateTime"));
                    updateCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, int.MaxValue, "Status"));
                    updateCommand.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Int, int.MaxValue, "Amount"));
                    updateCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, int.MaxValue, "Id"));

                    foreach (DataRowView drv in ds.Tables[0].DefaultView)
                    {
                        HttpResponseMessage apiResponse = await this.ApiClient.GetAsync("UserBalanceBeforeVerified/{0}".FormatWith(Convert.ToString(drv["UserIdentifier"])));
                        BasicResponse result = await apiResponse.Content.ReadAsAsync<BasicResponse>();
                        if (result.responseData > 0)
                        {
                            RebateRequest rebateRequest = this.BuildRebateRequest(drv, result.responseData);
                            HttpResponseMessage response = await this.GatewayClient.PostAsJsonAsync("gateway/api/business/rebate", rebateRequest);

                            LogHelper.Log($"用户开通存管后,流水同步到存管{DateTime.UtcNow.ToChinaStandardTime()}", rebateRequest.ToJson(), Convert.ToString(drv["UserIdentifier"]));

                            if (response.IsSuccessStatusCode)
                            {
                                drv["LastUpdateTime"] = DateTime.UtcNow.ToChinaStandardTime();
                                drv["Status"] = 1;
                                drv["Amount"] = result.responseData;
                            }
                        }
                        else
                        {
                            drv["LastUpdateTime"] = DateTime.UtcNow.ToChinaStandardTime();
                            drv["Status"] = 1;
                        }
                    }

                    adapter.UpdateCommand = updateCommand;
                    adapter.Update(ds.Tables[0]);
                } while (pageSize == ds.Tables[0].Rows.Count);
            }
            catch (Exception e)
            {
                LogHelper.Error(e, $"用户开通存管后,流水同步到存管{DateTime.UtcNow.ToChinaStandardTime()}异常", e.ToJson(), "SyncCGVerifiedUserRebateService");
            }
        }
    }
}
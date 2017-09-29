// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : MemberServices.cs
// Created          : 2016-11-15  18:23
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:17
// ******************************************************************************************************
// <copyright file="MemberServices.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
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
using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Services
{
    public class MemberServices
    {
        /// <summary>
        ///     The insert accountransaction request URI
        /// </summary>
        private static readonly string InsertAccountransactionRequestUri;

        /// <summary>
        ///     The member API host
        /// </summary>
        private static readonly string memberApiHost;

        /// <summary>
        ///     The merge relationship URI
        /// </summary>
        private static readonly string MergeRelationshipUri;

        /// <summary>
        ///     The request URI
        /// </summary>
        private static readonly string SendCouponsRequestUri;

        /// <summary>
        ///     The client
        /// </summary>
        private readonly Lazy<HttpClient> client;

        private readonly List<MergeTransactionEntities> merges;

        /// <summary>
        ///     Initializes the <see cref="MemberServices" /> class.
        /// </summary>
        static MemberServices()
        {
            memberApiHost = CloudConfigurationManager.GetSetting("MemberApiHost");
            SendCouponsRequestUri = "/Member/BathSendCoupons";
            InsertAccountransactionRequestUri = "/Merge/BatchInsertAccountransactions";
            MergeRelationshipUri = "/Merge/MergeRelationshipTransaction/{0}/{1}";
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemberServices" /> class.
        /// </summary>
        public MemberServices()
        {
            this.client = new Lazy<HttpClient>(InitHttpClient);
            this.merges = new List<MergeTransactionEntities>();
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
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(memberApiHost) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = new TimeSpan(0, 0, 0, 30);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            return httpClient;
        }

        /// <summary>
        ///     Inserts the accountransactions.
        /// </summary>
        public async Task InsertAccountransactions()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, InsertAccountransactionRequestUri);
                await this.Client.SendAsync(request);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Inserts the accountransactions work.
        /// </summary>
        [DisplayName("归并好友流水")]
        public void InsertAccountransactionsWork()
        {
            this.InsertAccountransactions().Wait();
        }

        /// <summary>
        ///     Sends the coupons.
        /// </summary>
        public async Task SendCoupons()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, SendCouponsRequestUri);
                await this.Client.SendAsync(request);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Sends the coupon work.
        /// </summary>
        [DisplayName("发放生日卡券")]
        public void SendCouponWork()
        {
            this.SendCoupons().Wait();
        }

        /// <summary>
        ///     Works this instance.
        /// </summary>
        [DisplayName("好友流水同步到卡券系统")]
        public void Work()
        {
            try
            {
                this.QueryMergeTransactionDb();
                this.CallMergeTransactionBatchAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("MergeTransactionException {0}".FormatWith(e.Message), e);
            }
        }

        /// <summary>
        ///     Calls the merge transaction batch asynchronous.
        /// </summary>
        /// <returns></returns>
        private async Task CallMergeTransactionBatchAsync()
        {
            //log info
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            int count = this.merges.Count;
            if (count <= 0) return;

            PollMergeTransactionInfoLog log = new PollMergeTransactionInfoLog
            {
                Data = this.merges,
                Count = this.merges.Count,
                Time = DateTime.UtcNow.ToChinaStandardTime()
            };
            await StorageHelper.InsertDataIntoAzureBlobAsync("PollMergeTransaction" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));

            foreach (MergeTransactionEntities merge in this.merges)
            {
                try
                {
                    HttpResponseMessage msg = await this.Client.PostAsJsonAsync(MergeRelationshipUri.FormatWith(merge.TransactionIdentifier, merge.UserIdentifier), "");
                    await msg.Content.ReadAsStringAsync();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        ///     Queries the merge transaction database.
        /// </summary>
        private void QueryMergeTransactionDb()
        {
            try
            {
                this.merges.Clear();
                using (SqlCommand cmd = DbConfig.MemberConnection.CreateCommand())
                {
                    if (DbConfig.MemberConnection.State == ConnectionState.Closed)
                    {
                        DbConfig.MemberConnection.Open();
                    }
                    cmd.CommandText = "SELECT TOP 100 TransactionIdentifier AS [tid],UserIdentifier AS [uid] FROM MergeTransactions WHERE IsJoin=0 AND Date > @Date;";
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date)).Value = DateTime.UtcNow.ToChinaStandardTime().AddDays(-10);
                    SqlDataAdapter adapter = new SqlDataAdapter { SelectCommand = cmd };
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    foreach (DataRowView drv in ds.Tables[0].DefaultView)
                    {
                        this.merges.Add(new MergeTransactionEntities
                        {
                            UserIdentifier = drv["uid"].ToString(),
                            TransactionIdentifier = drv["tid"].ToString()
                        });
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
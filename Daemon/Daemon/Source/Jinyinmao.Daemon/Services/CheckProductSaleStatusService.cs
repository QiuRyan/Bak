// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : CheckProductSaleStatusService.cs
// Created          : 2016-10-13  16:30
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:12
// ******************************************************************************************************
// <copyright file="CheckProductSaleStatusService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;
using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Services
{
    /// <summary>
    ///     Class PollCheckProductSaleStatusService.
    /// </summary>
    public class CheckProductSaleStatusService
    {
        private static readonly string DbConnectionString;
        private static readonly string RequestUri;
        private static readonly RetryPolicy RetryPolicy;
        private readonly Lazy<HttpClient> client;
        private readonly List<RegularProductResult> list;

        static CheckProductSaleStatusService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
            RequestUri = "/CheckProductSaleStatus";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckProductSaleStatusService" /> class.
        /// </summary>
        public CheckProductSaleStatusService()
        {
            this.list = new List<RegularProductResult>();
            this.client = new Lazy<HttpClient>(HttpClientHelper.InitHttpClient);
        }

        /// <summary>
        ///     Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public HttpClient Client => this.client.Value;

        /// <summary>
        ///     Works this instance.
        /// </summary>
        [DisplayName("查看定期产品状态")]
        public void Work()
        {
            try
            {
                this.QueryRegularPorductDb();
                this.CallCheckProductSaleStatusAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollCheckProductSaleStatusException {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CallCheckProductSaleStatusAsync()
        {
            //log info
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            int count = this.list.Count;
            if (count <= 0) return;

            PollCheckProductSaleStatusInfoLog log = new PollCheckProductSaleStatusInfoLog
            {
                Time = now,
                Count = count,
                Data = this.list
            };

            await StorageHelper.InsertDataIntoAzureBlobAsync("PollCheckProductSaleStatusService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));

            foreach (RegularProductResult item in this.list)
            {
                HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(RequestUri + "/{0}".FormatWith(item.ProductIdentifier), new ByteArrayContent(Encoding.UTF8.GetBytes(""))));
                msg.EnsureSuccessStatusCode();
            }
        }

        private void QueryRegularPorductDb()
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                string strSql = "select ProductIdentifier as pid from dbo.RegularProducts where SoldOut = 0";
                SqlDataAdapter adapter = new SqlDataAdapter(strSql, con);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRowView drv in ds.Tables[0].DefaultView)
                {
                    this.list.Add(new RegularProductResult
                    {
                        ProductIdentifier = drv["pid"].ToString()
                    });
                }
            }
        }
    }
}
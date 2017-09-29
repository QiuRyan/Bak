// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : CheckProductSoldOutStatusService.cs
// Created          : 2016-10-13  16:33
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:03
// ******************************************************************************************************
// <copyright file="CheckProductSoldOutStatusService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;

namespace Jinyinmao.Daemon.Services
{
    /// <summary>
    ///     Class PollCheckProductSaleStatusService.
    /// </summary>
    public class CheckProductSoldOutStatusService
    {
        private static readonly string DbConnectionString;
        private static readonly string RequestUri;
        private static readonly RetryPolicy RetryPolicy;
        private readonly Lazy<HttpClient> client;

        static CheckProductSoldOutStatusService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
            RequestUri = "/CheckProductSaleStatus";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckProductSoldOutStatusService" /> class.
        /// </summary>
        public CheckProductSoldOutStatusService()
        {
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
        [DisplayName("查看定期产品售罄状态")]
        public void Work()
        {
            try
            {
                this.QueryRegularPorductDb();
                this.CallCheckProductSaleStatusAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.Error(e, "查看定期产品售罄状态异常", e.ToJson(), "CheckProductSoldOutStatusService");
            }
        }

        private async Task CallCheckProductSaleStatusAsync()
        {
            //log info
            List<RegularProductResult> list = this.QueryRegularPorductDb();

            int count = list.Count;
            if (count <= 0) return;

            foreach (RegularProductResult item in list)
            {
                HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(RequestUri + "/{0}".FormatWith(item.ProductIdentifier), new ByteArrayContent(Encoding.UTF8.GetBytes(""))));
                msg.EnsureSuccessStatusCode();

                LogHelper.Log($"Daemon查看定期产品售罄状态{DateTime.UtcNow.ToChinaStandardTime()}", item.ToJson(), item.ProductIdentifier);
            }
        }

        private List<RegularProductResult> QueryRegularPorductDb()
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                string strSql = "select ProductIdentifier as pid from dbo.RegularProducts where SoldOut = 1 and datediff(day,[SoldOutTime],getdate())<=3";
                SqlDataAdapter adapter = new SqlDataAdapter(strSql, con);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return (from DataRowView drv in ds.Tables[0].DefaultView
                        select new RegularProductResult
                        {
                            ProductIdentifier = drv["pid"].ToString()
                        }).ToList();
            }
        }
    }
}
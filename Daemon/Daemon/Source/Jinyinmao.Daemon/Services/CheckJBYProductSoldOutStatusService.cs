// ***********************************************************************
// Project          : Jinyinmao.Daemon
// File             : CheckJBYProductSoldOutStatusService.cs
// Created          : 2016-07-27  15:38
//
//
// Last Modified On : 2016-08-03  14:32
// ***********************************************************************
// <copyright file="CheckJBYProductSoldOutStatusService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;

namespace Jinyinmao.Daemon.Services
{
    /// <summary>
    ///     Class PollCheckJBYProductSoldOutStatusService.
    /// </summary>
    public class CheckJBYProductSoldOutStatusService
    {
        private static readonly string DbConnectionString;
        private static readonly string RequestUri;
        private static readonly RetryPolicy RetryPolicy;
        private readonly Lazy<HttpClient> client;

        static CheckJBYProductSoldOutStatusService()
        {
            DbConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
            RequestUri = "/CheckJBYProductSoldOutStatus";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckJBYProductSoldOutStatusService" /> class.
        /// </summary>
        public CheckJBYProductSoldOutStatusService()
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
        public void Work()
        {
            try
            {
                this.CallCheckProductSaleStatusAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("PollCheckJBYProductSoldOutStatusException {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CallCheckProductSaleStatusAsync()
        {
            List<RegularProductResult> list = this.QueryRegularPorductDb();
            //log info
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            int count = list.Count;
            if (count <= 0) return;

            PollCheckProductSaleStatusInfoLog log = new PollCheckProductSaleStatusInfoLog
            {
                Time = now,
                Count = count,
                Data = list
            };

            await StorageHelper.InsertDataIntoAzureBlobAsync("PollCheckProductSoldOutStatusService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));

            foreach (RegularProductResult item in list)
            {
                HttpResponseMessage msg = await RetryPolicy.ExecuteAction(() => this.Client.PostAsync(RequestUri + "/{0}".FormatWith(item.ProductIdentifier), new ByteArrayContent(Encoding.UTF8.GetBytes(""))));
                // ReSharper disable once UnusedVariable
                string result = await msg.Content.ReadAsStringAsync();
            }
        }

        private List<RegularProductResult> QueryRegularPorductDb()
        {
            using (SqlConnection con = new SqlConnection(DbConnectionString))
            {
                string strSql = "select ProductIdentifier as pid from dbo.JBYProducts where SoldOut = 1 and datediff(day,[SoldOutTime], getdate())<=3";
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
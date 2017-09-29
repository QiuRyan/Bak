// ******************************************************************************************************
// Project          : Jinyinmao.Daemon
// File             : DoDailyWorkByStorageService.cs
// Created          : 2016-10-13  16:30
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-08-29  17:13
// ******************************************************************************************************
// <copyright file="DoDailyWorkByStorageService.cs" company="Shanghai JYM Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai JYM Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;

namespace Jinyinmao.Daemon.Services
{
    public class DoDailyWorkByStorageService
    {
        private static readonly string RequestUri;
        private static readonly RetryPolicy RetryPolicy;
        private static readonly int totalThreadNum;
        private static List<string> userIds;
        private readonly Lazy<IEnumerable<HttpClient>> clients;

        static DoDailyWorkByStorageService()
        {
            RequestUri = "/DoDailyWork";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(10));
            userIds = new List<string>();
            totalThreadNum = CloudConfigurationManager.GetSetting("TotalThreadNum").AsInt(10);
        }

        public DoDailyWorkByStorageService()
        {
            this.clients = new Lazy<IEnumerable<HttpClient>>(HttpClientHelper.InitHttpClientWithIpList);
        }

        public List<HttpClient> Clients => this.clients.Value.ToList();

        [DisplayName("余额猫每日跑批")]
        public void Work()
        {
            try
            {
                this.QueryRecentUserFromStorage();
                //original
                this.DealAllUser().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("DoDailyWorkException {0}".FormatWith(e.Message), e);
            }
        }

        private async Task CallDoDailyWorkAsync(string userId)
        {
            HttpResponseMessage msg = await this.GetRandomClient().PostAsync(RequestUri + "/{0}".FormatWith(userId), new ByteArrayContent(Encoding.UTF8.GetBytes("")));
            msg.EnsureSuccessStatusCode();
        }

        private async Task DealAllUser()
        {
            List<Task> taskList = new List<Task>();
            for (int i = 0; i < totalThreadNum; i++)
            {
                List<string> users = this.GetPartitionOfList(userIds, i, totalThreadNum);
                Task task = this.DealUserList(users);
                taskList.Add(task);
            }

            await Task.WhenAll(taskList);
        }

        private async Task DealUserList(IEnumerable<string> list)
        {
            foreach (string id in list)
            {
                try
                {
                    await RetryPolicy.ExecuteAsync(() => this.CallDoDailyWorkAsync(id));
                }
                catch (Exception e)
                {
                    LogHelper.LogError("DoDailyWorkUserException {0}".FormatWith("Current userId: {0}".FormatWith(id) + e.Message), e);
                }
            }
        }

        private List<T> GetPartitionOfList<T>(List<T> userList, int index, int total)
        {
            int section = (userList.Count / total) + 1;
            int end = (index + 1) * section > userList.Count ? userList.Count - index * section : section;
            if (end < 0) end = 0;
            int start = index * section < userList.Count ? index * section : userList.Count - 1;
            if (start < 0) start = 0;
            return userList.GetRange(start, end);
        }

        private HttpClient GetRandomClient()
        {
            int count = this.Clients.Count();
            Random r = new Random();
            int i = r.Next(0, count);
            return this.Clients[i];
        }

        private void QueryRecentUserFromStorage()
        {
            Random r = new Random();
            int order = r.Next(0, 10);

            ConfigEntity config = StorageHelper.QueryConfigTable<ConfigEntity>().Where(c => c.IsWorkday).OrderByDescending(c => c.RowKey).Take(4).LastOrDefault();
            string dateStr = DateTime.UtcNow.ToChinaStandardTime().AddDays(-3).ToString("yyyyMMdd");

            IEnumerable<string> list = StorageHelper.QueryRecentActiveUsersByKey<TableEntity>(config != null ? config.RowKey : dateStr);
            userIds = order > 5 ? list.OrderByDescending(u => u).ToList() : list.OrderBy(u => u).ToList();
        }
    }
}
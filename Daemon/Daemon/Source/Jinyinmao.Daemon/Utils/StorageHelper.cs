// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : StorageHelper.cs
// Created          : 2015-08-30  11:38 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-09-01  2:30 PM
// ***********************************************************************
// <copyright file="StorageHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;

namespace Jinyinmao.Daemon.Utils
{
    /// <summary>
    ///     StorageHelper.
    /// </summary>
    public class StorageHelper
    {
        /// <summary>
        ///     The account
        /// </summary>
        private static readonly CloudStorageAccount Account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

        /// <summary>
        ///     The active user table
        /// </summary>
        private static readonly CloudTable ActiveUserTable;

        /// <summary>
        ///     The BLOB client
        /// </summary>
        private static readonly CloudBlobClient BlobClient = Account.CreateCloudBlobClient();

        /// <summary>
        ///     The container
        /// </summary>
        private static readonly CloudBlobContainer BlobLogContainer;

        /// <summary>
        ///     The configuration table
        /// </summary>
        private static readonly CloudTable ConfigTable;

        /// <summary>
        ///     The table client
        /// </summary>
        private static readonly CloudTableClient TableClient = Account.CreateCloudTableClient();

        /// <summary>
        ///     Initializes static members of the <see cref="StorageHelper" /> class.
        /// </summary>
        static StorageHelper()
        {
            BlobLogContainer = BlobClient.GetContainerReference("jymdaemonlogs");
            BlobLogContainer.CreateIfNotExists();

            ActiveUserTable = TableClient.GetTableReference("JYMActiveUsersTable");
            ActiveUserTable.CreateIfNotExists();

            ConfigTable = TableClient.GetTableReference("Config");
            ConfigTable.CreateIfNotExists();
        }

        /// <summary>
        ///     Finds the entities by string condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="partionKey"></param>
        /// <returns>T.</returns>
        public static IEnumerable<T> FindEntitiesByKeys<T>(string tableName, string partionKey) where T : ITableEntity, new()
        {
            CloudTable table = TableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

            TableQuery<T> rangeQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThanOrEqual, partionKey));

            return table.ExecuteQuery(rangeQuery).ToList();
        }

        /// <summary>
        ///     insert data into azure BLOB as an asynchronous operation.
        /// </summary>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <param name="uploadText">The upload text.</param>
        /// <returns>Task.</returns>
        public static async Task InsertDataIntoAzureBlobAsync(string blobName, string uploadText)
        {
            CloudBlockBlob blockBlob = BlobLogContainer.GetBlockBlobReference(blobName);
            await blockBlob.UploadTextAsync(uploadText);
        }

        public static List<T> QueryConfigTable<T>() where T : ITableEntity, new()
        {
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            string rowKeyDownLimit = now.AddDays(-15).ToString("yyyyMMdd");
            string rowKeyUpLimit = now.ToString("yyyyMMdd");
            TableQuery<T> rangeQuery = new TableQuery<T>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, rowKeyDownLimit), TableOperators.And, TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, rowKeyUpLimit)));
            return ConfigTable.ExecuteQuery(rangeQuery).ToList();
        }

        /// <summary>
        ///     Queries the recent active users.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="time">The time.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<string> QueryRecentActiveUsers<T>(DateTimeOffset time) where T : ITableEntity, new()
        {
            TableQuery<T> rangeQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, time));
            IEnumerable<T> result = ActiveUserTable.ExecuteQuery(rangeQuery);
            return result.Select(u => u.RowKey).Distinct().ToList();
        }

        public static List<string> QueryRecentActiveUsersByKey<T>() where T : ITableEntity, new()
        {
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            string partitionKeyLimit = now.AddDays(-3).ToString("yyMMddHH");
            TableQuery<T> rangeQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThanOrEqual, partitionKeyLimit));
            IEnumerable<T> result = ActiveUserTable.ExecuteQuery(rangeQuery);
            return result.Select(u => u.RowKey).Distinct().ToList();
        }

        public static IEnumerable<string> QueryRecentActiveUsersByKey<T>(string dateLimit) where T : ITableEntity, new()
        {
            string partitionKeyLimit = dateLimit.GetLast(6).PadRight(8, '0');
            TableQuery<T> rangeQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThanOrEqual, partitionKeyLimit));
            IEnumerable<T> result = ActiveUserTable.ExecuteQuery(rangeQuery);
            return result.Select(u => u.RowKey).Distinct().ToList();
        }
    }
}
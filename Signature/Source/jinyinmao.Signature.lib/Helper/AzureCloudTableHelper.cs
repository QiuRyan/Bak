using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;

namespace jinyinmao.Signature.lib
{
    public static class AzureCloudTableHelper
    {
        /// <summary>
        ///     从CloudTable读取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">The client.</param>
        /// <param name="tablename">The tablename.</param>
        /// <param name="fieldname">The fieldname.</param>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        /// <exception cref="System.ArgumentNullException">table is null or empty </exception>
        public static T ReadFromCloudTable<T>(this CloudTableClient client, string tablename, string fieldname, string value) where T : ITableEntity, new()
        {
            if (string.IsNullOrWhiteSpace(tablename))
            {
                throw new ArgumentNullException($"tablename is null or empty ");
            }
            CloudTable table = client.GetTableReference(tablename);
            if (table == null)
            {
                throw new ArgumentNullException($"table is null ");
            }

            return table.ExecuteQuery(new TableQuery<T>().Where(TableQuery.GenerateFilterCondition(fieldname, QueryComparisons.Equal, value))).FirstOrDefault();
        }

        /// <summary>
        ///     从CloudTable读取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">The client.</param>
        /// <param name="tablename">The tablename.</param>
        /// <param name="query">The query.</param>
        /// <returns>T.</returns>
        /// <exception cref="System.ArgumentNullException">table is null or empty </exception>
        public static T ReadSingleRecordFromCloudTable<T>(this CloudTableClient client, string tablename, TableQuery<T> query) where T : ITableEntity, new()
        {
            if (string.IsNullOrWhiteSpace(tablename))
            {
                throw new ArgumentNullException($"tablename is null or empty ");
            }
            return ReadFromCloudTable(client, tablename, query).FirstOrDefault();
        }

        /// <summary>
        ///     保存数据到CloudTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">The client.</param>
        /// <param name="tablename">The tablename.</param>
        /// <param name="value">The value.</param>
        /// <param name="partitionkey">The partitionkey.</param>
        public static void SaveToCloudTable<T>(this CloudTableClient client, string tablename, T value, string partitionkey) where T : ITableEntity, new()
        {
            CloudTable table = client.GetTableReference(tablename);
            table.CreateIfNotExists();
            value.ETag = value.Timestamp.ToString();
            value.PartitionKey = partitionkey;
            value.Timestamp = DateTime.UtcNow.ToChinaStandardTime();
            table.Execute(TableOperation.InsertOrReplace(value));
        }

        /// <summary>
        ///     从CloudTable读取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">The client.</param>
        /// <param name="tablename">The tablename.</param>
        /// <param name="query">The query.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">table is null or empty </exception>
        private static IEnumerable<T> ReadFromCloudTable<T>(this CloudTableClient client, string tablename, TableQuery<T> query) where T : ITableEntity, new()
        {
            if (string.IsNullOrWhiteSpace(tablename))
            {
                throw new ArgumentNullException($"tablename is null or empty ");
            }
            CloudTable table = client.GetTableReference(tablename);
            if (table == null)
            {
                throw new ArgumentNullException($"table is null ");
            }

            return table.ExecuteQuery(query);
        }
    }
}
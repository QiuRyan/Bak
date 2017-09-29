using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using System.Configuration;

namespace DataTransfer
{
    /// <summary>
    ///     Class AzureStorageHelper.
    /// </summary>
    public class AzureStorageHelper
    {
        /// <summary>
        ///     Gets the user cloud table.
        /// </summary>
        /// <value>The user cloud table.</value>
        public static CloudTable UserCloudTable
        {
            get
            {
                CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("DataConnectingString"));
                CloudTableClient tableClient = account.CreateCloudTableClient();
                tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(2.Seconds(), 6);
                tableClient.DefaultRequestOptions.MaximumExecutionTime = 2.Minutes();
                tableClient.DefaultRequestOptions.ServerTimeout = 2.Minutes();
                CloudTable table = tableClient.GetTableReference("JYMUser");
                table.CreateIfNotExists();
                return table;
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using jinyinmao.MessageWorker.Config;
using Jinyinmao.Application.Constants;
using Jinyinmao.MessageWorker.Domain.Entity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;

namespace Jinyinmao.MessageWorker.Domain.Bll.Impl
{
    /// <summary>
    /// SmsServiceBase.
    /// </summary>
    public class SmsServiceBase
    {
        private static readonly Lazy<CloudTableClient> cloudTableClient = new Lazy<CloudTableClient>(() => InitCloudTableClient());
        private static readonly CloudStorageAccount StorageAccount = ConfigsManager.StorageAccount;
        private readonly ISmsService lSmsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmsServiceBase"/> class.
        /// </summary>
        /// <param name="lSmsService">The l SMS service.</param>
        public SmsServiceBase(ISmsService lSmsService)
        {
            this.lSmsService = lSmsService;
        }

        private CloudTableClient CloudTableClient
        {
            get { return cloudTableClient.Value; }
        }

        /// <summary>
        /// Store SMS message as an asynchronous operation.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="message">The message.</param>
        /// <param name="responseMessage">The response message.</param>
        /// <param name="gateway">The gateway.</param>
        /// <returns>Task.</returns>
        public async Task StoreSmsMessageAsync(string args, string cellphone, string message, string responseMessage, SmsGateway gateway)
        {
            await this.CloudTableClient.GetTableReference("Sms").ExecuteAsync(TableOperation.Insert(new SmsMessage
            {
                AppId = args,
                Cellphones = cellphone ?? string.Empty,
                Message = message,
                Notes = this.lSmsService.ProductId,
                PartitionKey = "jinyinmao-api-sms",
                RowKey = GuidUtility.NewSequentialGuid().ToGuidString(),
                Response = responseMessage,
                Time = DateTime.UtcNow.ToChinaStandardTime(),
                Gateway = (int)gateway
            }));
        }

        private static CloudTableClient InitCloudTableClient()
        {
            CloudTableClient client = StorageAccount.CreateCloudTableClient();
            client.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(5.Seconds(), 5);
            client.DefaultRequestOptions.MaximumExecutionTime = 2.Minutes();
            client.DefaultRequestOptions.ServerTimeout = 2.Minutes();
            return client;
        }
    }
}
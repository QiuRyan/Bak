using Application.ViewModel.MessageWorker;
using MessageWorker.Domain.Entity;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Moe.Lib;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MessageWorker.Domain.Bll.Impl
{
    public class KtSmsService : ISmsService
    {
        private static readonly string GetBalanceUrl;
        private static readonly string MessageTemplate;
        private static readonly string Password;
        private static readonly string SendMessageUrl;
        private static readonly CloudStorageAccount StorageAccount;
        private static readonly string UserName;
        private readonly string productId;
        private readonly ISmsMessageService smsMessageService = new SmsMessageService();

        static KtSmsService()
        {
            SendMessageUrl = "http://114.215.196.145/sendSmsApi?";
            GetBalanceUrl = "http://api.ktsms.cn/smsBalance?";
            string userNameConfig = CloudConfigurationManager.GetSetting("SmsServiceUserName");
            string passwordConfig = CloudConfigurationManager.GetSetting("SmsServicePassword");
            UserName = userNameConfig.IsNotNullOrEmpty() ? userNameConfig : "jinyinmaoyx";
            Password = passwordConfig.IsNotNullOrEmpty() ? passwordConfig : "qwe123456";
            MessageTemplate = "username={0}&password={1}&mobile={2}&content={3}【{4}】";
            StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }

        /// <summary>
        /// </summary>
        /// <param name="priority"></param>
        public KtSmsService(int priority)
        {
            switch (priority)
            {
                case 0:
                    this.productId = "676767";
                    break;

                case 1:
                    this.productId = "48661";
                    break;

                default:
                    this.productId = "251503";
                    break;
            }
        }

        #region ISmsService Members

        /// <summary>
        ///     Gets or sets the name of the SMS gateway.
        /// </summary>
        /// <value>The name of the SMS gateway.</value>
        public SmsGateway SmsGatewayName { get; set; } = SmsGateway.KeXinTong;

        public async Task<int?> GetBalanceAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string getBalanceRequstUrl = GetBalanceUrl + "username={0}&password={1}".FormatWith(UserName, Password);
                HttpResponseMessage response = await client.GetAsync(getBalanceRequstUrl);
                return (await response.Content.ReadAsStringAsync()).ToInt();
            }
        }

        public async Task SendMessageAsync(string appId, string cellphones, string message, string signature)
        {
            string responseMessage = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(SendMessageUrl + MessageTemplate.FormatWith(
                        UserName, Password, cellphones, message, signature));
                    responseMessage = await response.Content.ReadAsStringAsync();
                    //保存到本地数据库
                    SmsMessage smsEntity = new SmsMessage
                    {
                        SmsId = Guid.NewGuid().ToGuidString(),
                        ClientId = appId,
                        Cellphones = cellphones,
                        Message = message,
                        Notes = this.productId,
                        Response = responseMessage,
                        Time = DateTime.UtcNow.ToChinaStandardTime(),
                        Gateway = (int)SmsGateway.KeXinTong,
                        Signature = signature
                    };
                    //插入数据库
                    SmsMessage smsMessage = await this.smsMessageService.CreateAsync(smsEntity);
                }
            }
            catch (Exception e)
            {
                responseMessage = e.Message;
                throw;
            }
            finally
            {
                //CloudTableClient client = StorageAccount.CreateCloudTableClient();
                //client.GetTableReference("Sms").Execute(TableOperation.Insert(new SmsMessage
                //{
                //    AppId = appId,
                //    Cellphones = cellphones,
                //    Message = message,
                //    Notes = this.productId,
                //    PartitionKey = "jinyinmao-api-sms",
                //    RowKey = GuidUtility.NewSequentialGuid().ToGuidString(),
                //    Response = responseMessage,
                //    Time = DateTime.UtcNow.ToChinaStandardTime(),
                //    Gateway = (int)SmsGateway.KeXinTong
                //}));
            }

            if (!responseMessage.StartsWith("1,", StringComparison.Ordinal))
            {
                throw new ApplicationException("Sending message failed. " + responseMessage);
            }
        }

        #endregion ISmsService Members
    }
}
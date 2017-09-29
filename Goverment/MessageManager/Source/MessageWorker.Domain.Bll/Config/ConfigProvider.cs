using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Moe.Lib;

namespace Jinyinmao.MessageWorker.Domain.Bll.Config
{
    internal class ConfigProvider : IConfigProvider
    {
        #region IConfigProvider Members

        public string ClSendMessageUrl { get; private set; }
        public string ZtSendMessageUrl { get; private set; }

        public string ClSmsServicePassword { get; private set; }
        public string ClSmsServiceUserName { get; private set; }

        public bool SmsEnable { get; private set; }
        public CloudStorageAccount StorageAccount { get; private set; }
        public string ZtSmsServicePassword { get; private set; }
        public string ZtSmsServiceUserName { get; private set; }

        public void CheckConfig()
        {
        }

        public void InitConfig()
        {
            this.ZtSendMessageUrl = CloudConfigurationManager.GetSetting("ZtSmsService");
            this.ClSendMessageUrl = CloudConfigurationManager.GetSetting("ClSmsService");
            this.StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            this.ClSmsServicePassword = CloudConfigurationManager.GetSetting("ClSmsServicePassword");
            this.ClSmsServiceUserName = CloudConfigurationManager.GetSetting("ClSmsServiceUserName");
            this.ZtSmsServicePassword = CloudConfigurationManager.GetSetting("ZtSmsServicePassword");
            this.ZtSmsServiceUserName = CloudConfigurationManager.GetSetting("ZtSmsServiceUserName");
            this.SmsEnable = CloudConfigurationManager.GetSetting("SmsEnable").AsBoolean(false);
        }

        #endregion IConfigProvider Members
    }
}
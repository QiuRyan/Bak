using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Moe.Lib;

namespace Jinyinmao.Daemon.Services
{
    public class LockYemRedeemService
    {
        /// <summary>
        ///     分组,根据用户
        /// </summary>
        /// <value>The data list.</value>
        private Dictionary<string, List<LockYemEntity>> DataList
        {
            get
            {
                string nowday = DateTime.UtcNow.ToChinaStandardTime().AddDays(-5).ToString("yyyyMMdd");

                IEnumerable<LockYemEntity> result = StorageHelper.FindEntitiesByKeys<LockYemEntity>("LockYemRedeem", nowday);

                return result.Where(p => !p.IsSync).GroupBy(p => p.UserId).ToDictionary(p => p.Key, p => p.ToList());
            }
        }

        [DisplayName("余额猫赎回锁定")]
        public void Work()
        {
            try
            {
                this.CheckAllOrdersAsync().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("DoDailyWorkException {0}".FormatWith(e.Message), e);
            }
        }

        private List<MessageWrapper<LockYemEntity>> BuildMessageWrapper(LockYemEntity lockYemEntity)
        {
            return new List<MessageWrapper<LockYemEntity>>
            {
                new MessageWrapper<LockYemEntity>
                {
                    MessageData = lockYemEntity,
                    MessageType = 8
                }
            };
        }

        private async Task<BoolResponse> CheckAllOrdersAllotAsync(LockYemEntity lockYem)
        {
            try
            {
                LogHelper.Log($"该赎回订单{lockYem.TransactionIdentifier}调用交易系统检查之前的申购是否已经全部完成", lockYem.ToJson(), lockYem.UserId);

                HttpClient client = HttpClientHelper.InitHttpClient();
                HttpResponseMessage responseMessage = await client.GetAsync($"CheckAllOrdersAllot/{lockYem.UserId}/{lockYem.TransactionIdentifier}");
                return await responseMessage.Content.ReadAsAsync<BoolResponse>();
            }
            catch (Exception e)
            {
                LogHelper.Error(e, $"该赎回订单{lockYem.TransactionIdentifier}调用交易系统出现异常,默认返回[False]", lockYem.ToJson(), lockYem.UserId);
                return new BoolResponse { Result = false };
            }
        }

        private async Task CheckAllOrdersAsync()
        {
            List<LockYemEntity> updateList = new List<LockYemEntity>();

            foreach (List<LockYemEntity> lockYemList in this.DataList.Select(keyValuePair => keyValuePair.Value.OrderBy(p => p.OrderTime).ToList()))
            {
                foreach (LockYemEntity lockYemEntity in lockYemList)
                {
                    BoolResponse response = await this.CheckAllOrdersAllotAsync(lockYemEntity);
                    if (!response.Result)
                    {
                        LogHelper.Log($"该赎回订单{lockYemEntity.TransactionIdentifier}之前尚有申购订单未处理完成", lockYemEntity.ToJson(), lockYemEntity.UserId);
                        break;
                    }

                    //写赎回队列
                    this.YemRedeemServiceBus(this.BuildMessageWrapper(lockYemEntity));

                    lockYemEntity.IsSync = true;
                    updateList.Add(lockYemEntity);
                }
            }

            await this.ExcuteReplaceAsync(updateList);
        }

        private async Task ExcuteReplaceAsync(List<LockYemEntity> lockYemEntityList)
        {
            int i = 0;
            int count = 50;
            List<LockYemEntity> lockYemEntityPage;

            do
            {
                lockYemEntityPage = lockYemEntityList.Take(count).Skip(i * count).ToList();
                if (lockYemEntityPage.Count == 0)
                {
                    break;
                }

                await StorageHelper.ExecuteBatchAsync("LockYemRedeem", lockYemEntityPage);
            } while (lockYemEntityPage.Count == count);
        }

        /// <summary>
        ///     初始化message sender.
        /// </summary>
        /// <param name="queneName">Name of the quene.</param>
        /// <returns>MessageSender.</returns>
        private MessageSender InitMessageSender(string queneName)
        {
            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(CloudConfigurationManager.GetSetting("ServiceBusConnectionString"));
            factory.RetryPolicy = RetryPolicy.Default;
            return factory.CreateMessageSender(queneName);
        }

        private void YemRedeemServiceBus(List<MessageWrapper<LockYemEntity>> models)
        {
            MessageWrapper<LockYemEntity> model = models.FirstOrDefault();
            try
            {
                LogHelper.Log($"赎回订单{model?.MessageData.TransactionIdentifier}正在写入赎回队列", models.ToJson(), model?.MessageData.UserId);

                MessageSender messageSender = this.InitMessageSender(CloudConfigurationManager.GetSetting("RedemptionQueueName"));
                models.ForEach(m => messageSender.Send(new BrokeredMessage(m.ToJson())));

                LogHelper.Log($"赎回订单{model?.MessageData.TransactionIdentifier}写入赎回队列成功", models.ToJson(), model?.MessageData.UserId);
            }
            catch (Exception e)
            {
                LogHelper.Error(e, $"该赎回订单{model?.MessageData.TransactionIdentifier}之前尚有申购订单未处理完成", models.ToJson(), model?.MessageData.UserId);
                throw;
            }
        }
    }
}
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Jinyinmao.Daemon.Services
{
    public class NewYearActivityService
    {
        /// <summary>
        ///     News the year cash back to user acount.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        public async Task CashBackToUserAcount()
        {
            HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsJsonAsync("/NewYear/CashBackToUserAccount", string.Empty));
            msg.EnsureSuccessStatusCode();

            string result = await msg.Content.ReadAsStringAsync();
            //var responseResult = JsonConvert.DeserializeObject<NewActivityYearEntities>(result);
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            var log = new
            {
                Time = now,
                Data = result
            };

            await StorageHelper.InsertDataIntoAzureBlobAsync("NewYearActivityService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));
        }

        /// <summary>
        ///     News the year cash back to user acount.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        public async Task SyncBusinessDraft()
        {
            HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsJsonAsync("/NewYear/SyncBusinessDraft", string.Empty));
            msg.EnsureSuccessStatusCode();
            string result = await msg.Content.ReadAsStringAsync();
            //var responseResult = JsonConvert.DeserializeObject<NewActivityYearEntities>(result);
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            var log = new
            {
                Time = now,
                Data = result
            };

            await StorageHelper.InsertDataIntoAzureBlobAsync("NewYearActivityService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));
        }

        /// <summary>
        ///     News the year cash back to user acount.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        public async Task SyncRebate()
        {
            HttpResponseMessage msg = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsJsonAsync("/NewYear/SyncRebate", string.Empty));
            msg.EnsureSuccessStatusCode();

            string result = await msg.Content.ReadAsStringAsync();
            //var responseResult = JsonConvert.DeserializeObject<NewActivityYearEntities>(result);
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            var log = new
            {
                Time = now,
                Data = result
            };

            await StorageHelper.InsertDataIntoAzureBlobAsync("NewYearActivityService" + "/" + now.ToString("yyyyMMddHH") + "/" + GuidUtility.NewSequentialGuid().ToString().ToUpper().Replace("-", ""), JsonConvert.SerializeObject(log));
        }

        private HttpClient InitHttpClient()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(ActivityCenterApiHost) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            return httpClient;
        }

        #region 属性

        private static readonly string ActivityCenterApiHost;

        /// <summary>
        ///     The retry policy
        /// </summary>
        private static readonly RetryPolicy RetryPolicy;

        /// <summary>
        ///     The client
        /// </summary>
        private readonly Lazy<HttpClient> client;

        static NewYearActivityService()
        {
            ActivityCenterApiHost = CloudConfigurationManager.GetSetting("ActivityCenterApiHost");
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(3, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XianFengPaymentService" /> class.
        /// </summary>
        public NewYearActivityService()
        {
            this.client = new Lazy<HttpClient>(this.InitHttpClient);
        }

        /// <summary>
        ///     The clientPaymentApiHost
        /// </summary>
        public HttpClient Client => this.client.Value;

        #endregion 属性

        #region 工作函数

        public void CashBackWork()
        {
            try
            {
                this.CashBackToUserAcount().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("NewYearActivityException {0}".FormatWith(e.Message), e);
            }
        }

        public void Work()
        {
            try
            {
                this.SyncBusinessDraft().Wait();
                this.SyncRebate().Wait();
                this.CashBackToUserAcount().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("NewYearActivityException {0}".FormatWith(e.Message), e);
            }
        }

        #endregion 工作函数
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using jinyinmao.Signature.lib;
using jinyinmao.Signature.lib.Helper;
using jinyinmao.Signature.Service.Model;
using Moe.Lib;
using Newtonsoft.Json;

namespace jinyinmao.Signature.Service
{
    public class TirisfalService
    {
        private readonly HttpClient tirisfalHttpClient = new Lazy<HttpClient>(() => JYMHttpUtility.InitHttpClient(ConfigManager.TirisfalServiceBaseUrl)).Value;

        #region 交易系统

        /// <summary>
        ///     获取产品信息
        /// </summary>
        /// <param name="productid">The productid.</param>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        public async Task<BasicResult<ProductOrderInfo>> GetProductOrderInfoAsync(string productid)
        {
            try
            {
                HttpResponseMessage tirisfalResponse = await this.tirisfalHttpClient.GetAsync($"/Product/Regular/ThirdParty/{productid}");

                if (tirisfalResponse.StatusCode != HttpStatusCode.OK)
                {
                    return BasicResult<ProductOrderInfo>.Failed($"StatusCode:{tirisfalResponse.StatusCode},Response:{await tirisfalResponse.Content.ReadAsStringAsync()}", null);
                }
                ProductOrderInfo product = JsonConvert.DeserializeObject<ProductOrderInfo>(await tirisfalResponse.Content.ReadAsStringAsync());
                return BasicResult<ProductOrderInfo>.Success("成功", product);
            }
            catch (Exception ex)
            {
                return BasicResult<ProductOrderInfo>.Failed($"{ex.Message}", null);
            }
        }

        /// <summary>
        ///     根据订单号获取用户id
        /// </summary>
        /// <param name="orderid">The orderid.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> GetUserIdByOrderId(string orderid)
        {
            return await JYMDBHelper.GetUserIdByOrderIdAsync(orderid);
        }

        public async Task<YEMOrderUserInfo> GetYemUserInfoAsync(string userId)
        {
            HttpResponseMessage tirisfalResponse = await this.tirisfalHttpClient.GetAsync($"/BackOffice/UserInfo/{userId}");

            if (tirisfalResponse.StatusCode == HttpStatusCode.OK)
            {
                return await tirisfalResponse.Content.ReadAsAsync<YEMOrderUserInfo>();
            }
            return await Task.FromResult<YEMOrderUserInfo>(null);
        }

        /// <summary>
        ///     设置定期产品电子合同签署状态
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="agreementInfos"></param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> NotifyTirisfalRegularAgreementAsync(string productId, List<AgreementInfo> agreementInfos)
        {
            AgreementInfoRequest request = new AgreementInfoRequest
            {
                ProductIdentifier = productId,
                AgreementInfos = agreementInfos
            };

            LogHelper.WriteLog($"产品 {productId} 开始通知交易系统更新状态", productId);
            HttpResponseMessage tirisfalresponse = await this.tirisfalHttpClient.PostAsJsonAsync("Product/Regular/SetSignatureStatus", request);

            if (tirisfalresponse.StatusCode == HttpStatusCode.OK)
            {
                //通知交易系统成功
                LogHelper.WriteLog($"产品 {productId} 通知交易系统更新状态成功", productId);
                return await Task.FromResult(true);
            }
            LogHelper.WriteLog($"产品 {productId} 通知交易系统更新状态失败", productId);
            return await Task.FromResult(false);
        }

        /// <summary>
        ///     通知交易系统余额猫电子签章信息
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> NotifyTirisfalYemAgreementAsync(List<AgreementInfo> request)
        {
            LogHelper.WriteLog(" 发送余额猫PDF协议数据到交易系统", "NotifyTirisfalYemAgreementAsync", request.ToJson());

            HttpResponseMessage tirisfalresponse = await this.tirisfalHttpClient.PostAsJsonAsync("User/Yem/SetYEMSignatureStatus", request);

            if (tirisfalresponse.StatusCode == HttpStatusCode.OK)
            {
                //通知交易系统成功
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        #endregion 交易系统
    }
}
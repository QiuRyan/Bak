using System;
using System.Net.Http;
using System.Threading.Tasks;
using Jinyinmao.Coupon.Service.Interface;
using Jinyinmao.Deposit.Config;
using Jinyinmao.Deposit.Domain;
using Jinyinmao.Deposit.Lib;
using Jinyinmao.Deposit.Lib.Enum;
using Jinyinmao.ServiceBus.Service;
using Jinyinmao.Tirisfal.Service.Interface.Dtos;
using Moe.Lib;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;

namespace Jinyinmao.Coupon.Service
{
    public class CouponService : ICouponService
    {
        private readonly Lazy<HttpClient> couponServiceClient = new Lazy<HttpClient>(() => JYMInternalHttpClientFactory.Create(ConfigManager.CouponServiceRole, (TraceEntry)null));

        private readonly ServiceBusService serviceBusService = new ServiceBusService();

        #region ICouponService Members

        public async Task<BasicResult<string>> DouDiIncreaseRateCouponAsync(DouDiCouponMessage request)
        {
            try
            {
                await this.couponServiceClient.Value.PostAsJsonAsync("api/Coupons/DouDiIncreaseRateCoupon", request);

                return BasicResult<string>.Successed("");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "发放兜底奖励异常", request.ToJson(), request.ProductIdentifier + "兜底");

                return BasicResult<string>.Failed(string.Empty, "发放兜底奖励异常");
            }
        }

        /// <summary>
        ///     use cash back coupon as an asynchronous operation.
        /// </summary>
        /// <param name="message">The request.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<BasicResult<string>> UseCashBackCouponAsync(UseCouponMessage message)
        {
            //return await Task.FromResult("");
            try
            {
                HttpResponseMessage responseMessage = await this.couponServiceClient.Value.PostAsJsonAsync("api/Coupons/UseBackCashCoupon", message.CouponMessage);

                responseMessage.EnsureSuccessStatusCode();

                BasicResponse<string> response = await responseMessage.Content.ReadAsAsync<BasicResponse<string>>();

                return BasicResult<string>.Successed(response.Remark);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.CouponUseQueue, message.ToJson());
                    return BasicResult<string>.Failed(string.Empty, $"网络异常正在重试第{message.RetryCount}次,发送卡券{message.CouponType}使用到卡券系统异常:{ex.Message}");
                }

                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送卡券{message.CouponType}使用到卡券系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "使用卡券异常", message.ToJson(), message.CouponMessage.CouponIdentifier);

                return BasicResult<string>.Failed(string.Empty, "卡券使用超时,请稍后重试");
            }
        }

        /// <summary>
        ///     use increase coupon as an asynchronous operation.
        /// </summary>
        /// <param name="message">The request.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<BasicResult<string>> UseIncreaseCouponAsync(UseCouponMessage message)
        {
            try
            {
                HttpResponseMessage responseMessage = await this.couponServiceClient.Value.PostAsJsonAsync("api/Coupons/UseRegularIncreaseRateCoupon", message.CouponMessage);

                responseMessage.EnsureSuccessStatusCode();

                BasicResponse<string> response = await responseMessage.Content.ReadAsAsync<BasicResponse<string>>();

                return BasicResult<string>.Successed(response.Remark);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.CouponUseQueue, message.ToJson());
                    return BasicResult<string>.Failed(string.Empty, $"网络异常正在重试第{message.RetryCount}次,发送卡券{message.CouponType}使用到卡券系统异常:{ex.Message}");
                }

                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送卡券{message.CouponType}使用到卡券系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "使用卡券异常", message.ToJson(), message.CouponMessage.CouponIdentifier);

                return BasicResult<string>.Failed(string.Empty, "卡券使用超时,请稍后重试");
            }
        }

        /// <summary>
        ///     use principal coupon as an asynchronous operation.
        /// </summary>
        /// <param name="message">The request.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<BasicResult<string>> UsePrincipalCouponAsync(UseCouponMessage message)
        {
            try
            {
                HttpResponseMessage responseMessage = await this.couponServiceClient.Value.PostAsJsonAsync("api/Coupons/UsePrincipalCoupon", message.CouponMessage);

                responseMessage.EnsureSuccessStatusCode();

                BasicResponse<string> response = await responseMessage.Content.ReadAsAsync<BasicResponse<string>>();

                return BasicResult<string>.Successed(response.Remark);
            }
            catch (HttpRequestException ex)
            {
                int code = ex.Message.Split(':')[0].ToInt32();
                if (message.RetryCount < ConfigManager.MaxRetryCount && JYMStatusCode.ErrorCodes.Contains(code))
                {
                    message.RetryCount++;
                    await this.serviceBusService.SendMessageToServiceBusAsync(ConfigManager.CouponUseQueue, message.ToJson());
                    return BasicResult<string>.Failed(string.Empty, $"网络异常正在重试第{message.RetryCount}次,发送卡券{message.CouponType}使用到卡券系统异常:{ex.Message}");
                }

                return BasicResult<string>.Failed(string.Empty, $"重试{message.RetryCount}次失败,发送卡券{message.CouponType}使用到卡券系统异常:" + ex.Message, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "使用卡券异常", message.ToJson(), message.CouponMessage.CouponIdentifier);

                return BasicResult<string>.Failed(string.Empty, "卡券使用超时,请稍后重试");
            }
        }

        #endregion ICouponService Members
    }
}
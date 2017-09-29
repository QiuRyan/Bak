using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jinyinmao.Tirisfal.Service.Interface.Dtos
{
    public class DouDiCouponMessage
    {
        /// <summary>
        /// 用户手机号码
        /// </summary>
        /// <value>The cellphone.</value>
        [JsonProperty("cellphone")]
        [Required]
        public string Cellphone { get; set; }

        /// <summary>
        /// 产品唯一标识
        /// </summary>
        /// <value>The product identifier.</value>
        [JsonProperty("productIdentifier")]
        [Required]
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        [Required]
        public string UserIdentifier { get; set; }
    }

    public class UseCouponMessage
    {
        [JsonProperty("couponMessage")]
        public UseCouponMessageBody CouponMessage { get; set; }

        [JsonProperty("couponType")]
        public int CouponType { get; set; }

        [JsonProperty("retryCount")]
        public int RetryCount { get; set; }
    }

    /// <summary>
    /// Class UseCouponRequest.
    /// </summary>
    public class UseCouponMessageBody
    {
        /// <summary>
        /// 卡券唯一标识
        /// </summary>
        /// <value>The coupon identifier.</value>
        [JsonProperty("couponIdentifier")]
        public string CouponIdentifier { get; set; }

        /// <summary>
        /// 是否预申购
        /// </summary>
        /// <value><c>true</c> if this instance is booking; otherwise, <c>false</c>.</value>
        [JsonProperty("isBooking")]
        public bool IsBooking { get; set; }

        /// <summary>
        /// 订单唯一标识
        /// </summary>
        /// <value>The order identifier.</value>
        [JsonProperty("orderIdentifier")]
        public string OrderIdentifier { get; set; }

        /// <summary>
        /// 产品周期
        /// </summary>
        /// <value>The period.</value>
        [JsonProperty("Period")]
        public int Period { get; set; }

        /// <summary>
        /// 本金
        /// </summary>
        /// <value>The principal.</value>
        [JsonProperty("principal")]
        public long Principal { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        /// <value>The product category.</value>
        [JsonProperty("productCategory")]
        public long ProductCategory { get; set; }

        /// <summary>
        /// 产品唯一标识
        /// </summary>
        /// <value>The product identifier.</value>
        [JsonProperty("productIdentifier")]
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// 结息时间
        /// </summary>
        /// <value>The settle date.</value>
        [JsonProperty("settleDate")]
        public DateTime SettleDate { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }

        /// <summary>
        /// 产品利率
        /// </summary>
        [JsonProperty("yield")]
        public long Yield { get; set; }
    }
}
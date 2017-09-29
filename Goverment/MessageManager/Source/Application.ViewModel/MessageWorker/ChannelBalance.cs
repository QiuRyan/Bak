using System.ComponentModel.DataAnnotations;
using Jinyinmao.Application.Constants;

namespace Jinyinmao.Application.ViewModel.MessageWorker
{
    /// <summary>
    ///     ChannelBalance.
    /// </summary>
    public class ChannelBalance
    {
        /// <summary>
        ///     余额
        /// </summary>
        [Required]
        public int Balance { get; set; }

        /// <summary>
        ///     通道类型
        /// </summary>
        [Required]
        public SmsGateway SmsGateWay { get; set; }

        /// <summary>
        ///     是否支付余额查询
        /// </summary>
        [Required]
        public bool SupportBalanceQuery { get; set; }
    }
}
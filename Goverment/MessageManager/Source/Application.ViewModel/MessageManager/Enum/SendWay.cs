using System.ComponentModel;

namespace Jinyinmao.Application.ViewModel.MessageManager.Enum
{
    /// <summary>
    ///     Enum SendWay
    /// </summary>
    public enum SendWay
    {
        /// <summary>
        ///     验证码
        /// </summary>
        [Description("短信")]
        Message = 1,

        /// <summary>
        ///     通知
        /// </summary>
        [Description("推送")]
        Push = 2,

        /// <summary>
        ///     营销
        /// </summary>
        [Description("微信")]
        Wechat = 3
    }
}
using System.ComponentModel;

namespace Jinyinmao.Application.ViewModel.MessageManager.Enum
{
    /// <summary>
    ///     Enum SendResultCode
    /// </summary>
    public enum SendResultCode
    {
        /// <summary>
        ///     The success
        /// </summary>
        [Description("处理成功")]
        Success = 0,

        /// <summary>
        ///     没有对应模板
        /// </summary>
        [Description("没有对应模板")]
        NoTemplates = 1
    }
}
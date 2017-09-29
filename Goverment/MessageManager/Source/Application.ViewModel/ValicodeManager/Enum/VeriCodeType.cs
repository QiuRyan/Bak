using System.Collections.Generic;
using Moe.Lib;

namespace Jinyinmao.Application.ViewModel.ValicodeManager
{
    /// <summary>
    ///     Enum VeriCodeType
    /// </summary>
    public enum VeriCodeType
    {
        /// <summary>
        ///     缺省验证码类型
        /// </summary>
        Default = 0,

        /// <summary>
        ///     注册
        /// </summary>
        SignUp = 10,

        /// <summary>
        ///     重置登录密码
        /// </summary>
        ResetLoginPassword = 20,

        /// <summary>
        ///     重置支付密码
        /// </summary>
        ResetPaymentPassword = 30,

        /// <summary>
        ///     快速登陆
        /// </summary>
        QuickLogin = 40,

        /// <summary>
        ///     微信注册
        /// </summary>
        WeChat = 50
    }

    public static class VeriCodeTypeEx
    {
        private static readonly Dictionary<int, VeriCodeType> veriCodeTypes = new Dictionary<int, VeriCodeType>
        {
            { 0, VeriCodeType.Default },
            { 10, VeriCodeType.SignUp },
            { 20, VeriCodeType.ResetLoginPassword },
            { 30, VeriCodeType.ResetPaymentPassword },
            { 40, VeriCodeType.QuickLogin },
            { 50, VeriCodeType.WeChat }
        };

        public static VeriCodeType ToVeriCodeType(this int code)
        {
            return veriCodeTypes.GetOrDefault(code);
        }
    }
}
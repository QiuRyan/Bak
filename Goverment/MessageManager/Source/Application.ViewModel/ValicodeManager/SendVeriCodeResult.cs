namespace Jinyinmao.Application.ViewModel.ValicodeManager
{
    /// <summary>
    ///     SendVeriCodeResult.
    /// </summary>
    public class SendVeriCodeResult
    {
        /// <summary>
        ///     今天剩余发送次数，若为-1，则今天不能再次发送该类型验证码 ,
        /// </summary>
        /// <returns></returns>
        public int RemainCount { get; set; }

        /// <summary>
        ///     本次发送结果
        /// </summary>
        public bool Success { get; set; }
    }
}
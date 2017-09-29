using System.Threading.Tasks;
using Jinyinmao.Application.ViewModel.ValicodeManager;
using Jinyinmao.ValidateCode.Domain.Entity;
using MoeLib.Diagnostics;

namespace Jinyinmao.ValidateCode.Domain.Bll
{
    public abstract class ValidateCodeServiceBase : IValidateCodeService
    {
        protected readonly int defaultMaxSendTimes;

        protected readonly int maxSendTimeForQuickLogin;

        protected readonly int veriCodeExpiryMinites;

        protected ValidateCodeServiceBase(int defaultMaxSendTimes, int maxSendTimeForQuickLogin, int veriCodeExpiryMinites)
        {
            this.defaultMaxSendTimes = defaultMaxSendTimes;
            this.maxSendTimeForQuickLogin = maxSendTimeForQuickLogin;
            this.veriCodeExpiryMinites = veriCodeExpiryMinites;
        }

        #region IValidateCodeService Members

        /// <summary>
        ///     Gets the vericode by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Task&lt;VeriCode&gt;.</returns>
        public abstract Task<VeriCode> GetVeriCodeByTokenAsync(string token);

        /// <summary>
        ///     Sends the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="type">The type.</param>
        /// <param name="traceEntry"></param>
        /// <returns>Task&lt;SendVeriCodeResult&gt;.</returns>
        public abstract Task<SendVeriCodeResult> SendAsync(string cellphone, VeriCodeType type, TraceEntry traceEntry);

        /// <summary>
        ///     Uses the veri code.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task&lt;VeriCode&gt;.</returns>
        public abstract Task<VeriCode> UseVeriCodeAsync(string token, VeriCodeType type);

        /// <summary>
        ///     Verifies the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="code">The code.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task&lt;VerifyVeriCodeResult&gt;.</returns>
        public abstract Task<VerifyVeriCodeResult> VerifyAsync(string cellphone, string code, VeriCodeType type);

        #endregion IValidateCodeService Members
    }
}
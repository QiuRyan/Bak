using System.Threading.Tasks;
using Jinyinmao.Application.ViewModel.ValicodeManager;
using Jinyinmao.ValidateCode.Domain.Entity;
using MoeLib.Diagnostics;

namespace Jinyinmao.ValidateCode.Domain.Bll
{
    /// <summary>
    ///     Interface IValidateCodeService
    /// </summary>
    public interface IValidateCodeService
    {
        /// <summary>
        ///     Gets the vericode by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Task&lt;VeriCode&gt;.</returns>
        Task<VeriCode> GetVeriCodeByTokenAsync(string token);

        /// <summary>
        ///     Sends the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="type">The type.</param>
        /// <param name="traceEntry"></param>
        /// <returns>Task&lt;SendVeriCodeResult&gt;.</returns>
        Task<SendVeriCodeResult> SendAsync(string cellphone, VeriCodeType type, TraceEntry traceEntry);

        /// <summary>
        ///     Uses the veri code.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task&lt;VeriCode&gt;.</returns>
        Task<VeriCode> UseVeriCodeAsync(string token, VeriCodeType type);

        /// <summary>
        ///     Verifies the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="code">The code.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task&lt;VerifyVeriCodeResult&gt;.</returns>
        Task<VerifyVeriCodeResult> VerifyAsync(string cellphone, string code, VeriCodeType type);
    }
}
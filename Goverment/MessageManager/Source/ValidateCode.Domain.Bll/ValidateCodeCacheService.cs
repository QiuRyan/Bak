using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jinyinmao.Application.ViewModel.ValicodeManager;
using Jinyinmao.ValidateCode.Domain.Bll;
using Jinyinmao.ValidateCode.Domain.Entity;
using MoeLib.Diagnostics;

namespace ValidateCode.Domain.Bll
{
    /// <summary>
    ///     Class ValidateCodeCacheService.
    /// </summary>
    public class ValidateCodeCacheService : IValidateCodeService
    {
        /// <summary>
        ///     The inner service
        /// </summary>
        private readonly IValidateCodeService innerService;

        /// <summary>
        ///     The validate code cache
        /// </summary>
        private readonly Dictionary<string, Tuple<VeriCode, DateTime>> validateCodeCache =
            new Dictionary<string, Tuple<VeriCode, DateTime>>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidateCodeCacheService" /> class.
        /// </summary>
        /// <param name="validateCodeService">The validate code service.</param>
        public ValidateCodeCacheService(IValidateCodeService validateCodeService)
        {
            this.innerService = validateCodeService;
        }

        #region IValidateCodeService Members

        /// <summary>
        ///     Gets the vericode by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Task&lt;VeriCode&gt;.</returns>
        public Task<VeriCode> GetVeriCodeByTokenAsync(string token)
        {
            return this.innerService.GetVeriCodeByTokenAsync(token);
        }

        /// <summary>
        ///     Sends the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="type">The type.</param>
        /// <param name="traceEntry"></param>
        /// <returns>Task&lt;SendVeriCodeResult&gt;.</returns>
        public Task<SendVeriCodeResult> SendAsync(string cellphone, VeriCodeType type, TraceEntry traceEntry)
        {
            return this.innerService.SendAsync(cellphone, type, traceEntry);
        }

        /// <summary>
        ///     Uses the veri code.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task&lt;VeriCode&gt;.</returns>
        public Task<VeriCode> UseVeriCodeAsync(string token, VeriCodeType type)
        {
            return this.innerService.UseVeriCodeAsync(token, type);
        }

        /// <summary>
        ///     Verifies the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="code">The code.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task&lt;VerifyVeriCodeResult&gt;.</returns>
        public Task<VerifyVeriCodeResult> VerifyAsync(string cellphone, string code, VeriCodeType type)
        {
            return this.innerService.VerifyAsync(cellphone, code, type);
        }

        #endregion IValidateCodeService Members
    }
}
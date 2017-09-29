using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Jinyinmao.Application.ViewModel.ValicodeManager;
using Jinyinmao.MessageManager.Services;
using Jinyinmao.ValidateCode.Domain.Entity;
using Moe.Lib;
using MoeLib.Diagnostics;

namespace Jinyinmao.ValidateCode.Domain.Bll
{
    /// <summary>
    ///     ValidateCodeService.
    /// </summary>
    public class ValidateCodeService : ValidateCodeServiceBase
    {
        private readonly ISmsService smsService;

        public ValidateCodeService(int defaultMaxSendTimes, int maxSendTimeForQuickLogin, int veriCodeExpiryMinites, ISmsService smsService)
            : base(defaultMaxSendTimes, maxSendTimeForQuickLogin, veriCodeExpiryMinites)
        {
            this.smsService = smsService;
        }

        public ValidateCodeService(ISmsService smsService)
            : base(10, 20, 30)
        {
            this.smsService = smsService;
        }

        /// <summary>
        ///     Gets the vericode by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Task&lt;VeriCode&gt;.</returns>
        public override async Task<VeriCode> GetVeriCodeByTokenAsync(string token)
        {
            using (ValidateCodeDbContext db = new ValidateCodeDbContext())
            {
                return await db.ReadonlyQuery<VeriCode>().FirstOrDefaultAsync(v => v.Token == token);
            }
        }

        /// <summary>
        ///     Sends the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="type">The type.</param>
        /// <param name="traceEntry"></param>
        /// <returns>Task&lt;SendVeriCodeResult&gt;.</returns>
        public override async Task<SendVeriCodeResult> SendAsync(string cellphone, VeriCodeType type, TraceEntry traceEntry)
        {
            if (type == VeriCodeType.Default)
            {
                return new SendVeriCodeResult { RemainCount = -1, Success = false };
            }

            VeriCode code;

            using (ValidateCodeDbContext context = new ValidateCodeDbContext())
            {
                // 时间大于今天开始日期，就一定是今天发送的验证码
                code = await context.Query<VeriCode>().OrderByDescending(c => c.BuildAt)
                    .FirstOrDefaultAsync(c => c.Cellphone == cellphone && c.Type == (int)type && c.BuildAt >= DateTime.Today);

                // 没有记录，重新生成
                if (code == null)
                {
                    code = BuildVeriCode(cellphone, type);
                    context.Add(code);
                }

                // 超过最大次数，停止发送
                if (this.ExceedMaxSendTimes(code))
                {
                    return new SendVeriCodeResult { RemainCount = -1, Success = false };
                }

                RegenerateVeriCode(code);

                await context.ExecuteSaveChangesAsync();
            }

            MessageManagerResponse response = await this.smsService.SendVeriCodeAsync(code.Type.ToString(), code.Cellphone, code.Code, traceEntry);

            return new SendVeriCodeResult { RemainCount = this.GetRemainCount(code), Success = response.Status == 0L };
        }

        /// <summary>
        ///     Uses the veri code.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task&lt;VeriCode&gt;.</returns>
        public override async Task<VeriCode> UseVeriCodeAsync(string token, VeriCodeType type)
        {
            DateTime availableTime = DateTime.UtcNow.ToChinaStandardTime().AddMinutes(-this.veriCodeExpiryMinites);
            using (ValidateCodeDbContext db = new ValidateCodeDbContext())
            {
                VeriCode code = await db.Query<VeriCode>().OrderByDescending(v => v.BuildAt).FirstOrDefaultAsync(t => t.Token == token && t.Type == (int)type && t.BuildAt >= availableTime && !t.Used);
                if (code != null)
                {
                    code.Used = true;
                }

                await db.ExecuteSaveChangesAsync();

                return code;
            }
        }

        /// <summary>
        ///     Verifies the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="code">The code.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task&lt;VerifyVeriCodeResult&gt;.</returns>
        public override async Task<VerifyVeriCodeResult> VerifyAsync(string cellphone, string code, VeriCodeType type)
        {
            // 只取有效期内的验证码
            DateTime availableTime = DateTime.UtcNow.ToChinaStandardTime().AddMinutes(-this.veriCodeExpiryMinites);
            using (ValidateCodeDbContext db = new ValidateCodeDbContext())
            {
                VeriCode veriCode = await db.Query<VeriCode>().OrderByDescending(v => v.BuildAt).FirstOrDefaultAsync(t => t.Cellphone == cellphone && t.Type == (int)type && t.BuildAt >= availableTime && !t.Verified);

                // 无该手机验证码记录，或者超过3次，验证码失效
                if (veriCode == null || veriCode.ErrorCount >= 3)
                {
                    return new VerifyVeriCodeResult { RemainCount = -1, Success = false };
                }
                // 少于3次，执行验证
                if (veriCode.Code.Split('|').Contains(code))
                {
                    veriCode.Verified = true;
                }
                else
                {
                    // 验证未通过，且失败次数少于2次，递增失败次数
                    veriCode.ErrorCount += 1;
                    veriCode.Verified = false;
                }

                await db.ExecuteSaveChangesAsync();

                // 验证成功，返回token
                return new VerifyVeriCodeResult { Success = veriCode.Verified, Token = veriCode.Verified ? veriCode.Token : "", RemainCount = 3 - veriCode.ErrorCount };
            }
        }

        private static VeriCode BuildVeriCode(string cellphone, VeriCodeType type)
        {
            return new VeriCode
            {
                Cellphone = cellphone,
                Token = GuidUtility.NewSequentialGuid().ToGuidString(),
                Code = GenerateCode(),
                ErrorCount = 0,
                BuildAt = DateTime.UtcNow.ToChinaStandardTime(),
                Times = 1,
                Type = (int)type,
                Used = false,
                Verified = false
            };
        }

        private static string GenerateCode()
        {
            Random r = new Random();
            return r.Next(100000, 999999).ToString();
        }

        private static void RegenerateVeriCode(VeriCode code)
        {
            // 增加验证码发送次数
            code.Times += 1;
            // 重新生成验证码
            code.Code += "|" + GenerateCode();
            // 重置验证码验证数据
            code.ErrorCount = 0;
            code.Verified = false;
            code.Used = false;
            code.BuildAt = DateTime.UtcNow.ToChinaStandardTime();
        }

        private bool ExceedMaxSendTimes(VeriCode code)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (code.Type == (int)VeriCodeType.QuickLogin)
            {
                return code.Times >= this.maxSendTimeForQuickLogin;
            }

            return code.Times >= this.defaultMaxSendTimes;
        }

        private int GetRemainCount(VeriCode code)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (code.Type == (int)VeriCodeType.QuickLogin)
            {
                return this.maxSendTimeForQuickLogin - code.Times;
            }

            return this.defaultMaxSendTimes - code.Times;
        }
    }
}
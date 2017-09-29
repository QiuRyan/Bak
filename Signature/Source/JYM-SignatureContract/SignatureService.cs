using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using jinyinmao.Signature.lib;
using jinyinmao.Signature.lib.Helper;
using jinyinmao.Signature.Service;
using Timer = System.Timers.Timer;

namespace JYM_SignatureContract
{
    public class SignatureService
    {
        private RegularProductService regularProductService;
        private Timer timer;
        private CancellationTokenSource tokenSource;
        private YemService yemService;

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.regularProductService.SignatureRegularAgreement();
                //this.yemService.SignatureYemAgreement();
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex, "Timer_Elapsed");
            }
        }

        #region 对外方法

        public void Start()
        {
            LogHelper.WriteLog("SignContractService 开始启动 ", "OnStart");

            this.tokenSource = new CancellationTokenSource();
            this.regularProductService = new RegularProductService();
            this.yemService = new YemService();

            LogHelper.WriteLog("SignContractService 启动成功 ", "OnStart");

            //this.regularProductService.SignatureRegularAgreement();
            this.yemService.SignatureYemAgreement();

            this.timer = new Timer
            {
                AutoReset = true,
                Interval = 1000 * 60 * ConfigManager.DoWorkInterval
            };
            //分钟计
            this.timer.Elapsed += this.Timer_Elapsed;
            this.timer.Enabled = true;

            LogHelper.WriteLog("SignContractService 启动成功 ", "OnStart");
            Task.Factory.StartNew(() =>
            {
                this.regularProductService.SignatureRegularAgreement();
                //this.yemService.SignatureYemAgreement();
            }, this.tokenSource.Token);
        }

        /// <summary>
        ///     Called when [stop].
        /// </summary>
        public void Stop()
        {
            LogHelper.WriteLog("SignContractService 开始停止", "OnStop");
            this.timer.Enabled = false;
            this.tokenSource.Cancel();
            LogHelper.WriteLog("SignContractService 停止完成 ,任务取消", "OnStop");
        }

        #endregion 对外方法
    }
}
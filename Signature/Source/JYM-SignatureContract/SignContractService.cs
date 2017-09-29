using System.ServiceProcess;

namespace JYM_SignatureContract
{
    /// <summary>
    ///     Class SignContractService.
    /// </summary>
    /// <seealso cref="System.ServiceProcess.ServiceBase" />
    public partial class SignContractService : ServiceBase
    {
        private readonly SignatureService service;

        public SignContractService()
        {
            this.InitializeComponent();
            this.service = new SignatureService();
        }

        public void Start()
        {
            this.service.Start();
        }

        protected override void OnStart(string[] args)
        {
            this.service.Start();
        }

        protected override void OnStop()
        {
            this.service.Stop();
        }
    }
}
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace JYM_SignatureContract
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            this.InitializeComponent();
            Committed += this.ProjectInstaller_Committed;
        }

        private void ProjectInstaller_Committed(object sender, InstallEventArgs e)
        {
            ServiceController controller = new ServiceController("SignContractService");
            controller.Start();
        }
    }
}
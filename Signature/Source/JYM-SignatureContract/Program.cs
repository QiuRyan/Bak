using System.ServiceProcess;

namespace JYM_SignatureContract
{
    internal static class Program
    {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun =
            {
                new SignContractService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
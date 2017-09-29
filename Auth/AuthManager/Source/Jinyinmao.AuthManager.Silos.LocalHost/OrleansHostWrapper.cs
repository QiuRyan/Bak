using Moe.Lib;
using Orleans.Runtime.Host;
using System;
using System.Net;

namespace Jinyinmao.AuthManager.Silos.LocalHost
{
    internal sealed class OrleansHostWrapper : IDisposable
    {
        private SiloHost siloHost;

        public OrleansHostWrapper()
        {
            this.Init();
        }

        public bool Debug
        {
            get { return this.siloHost != null && this.siloHost.Debug; }
            set { this.siloHost.Debug = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.siloHost.Dispose();
            this.siloHost = null;
        }

        #endregion IDisposable Members

        public bool Run()
        {
            bool result = false;

            try
            {
                this.siloHost.InitializeOrleansSilo();
                result = this.siloHost.StartOrleansSilo();

                if (result)
                {
                    Console.WriteLine("Successfully started Orleans silo '{0}' as a {1} node.", this.siloHost.Name, this.siloHost.Type);
                }
                else
                {
                    throw new SystemException($"Failed to start Orleans silo '{this.siloHost.Name}' as a {this.siloHost.Type} node.");
                }
            }
            catch (Exception exc)
            {
                this.siloHost.ReportStartupError(exc);
                string msg = $"{exc.GetType().FullName}:\n{exc.Message}\n{exc.StackTrace}";
                Console.WriteLine(msg);
            }

            return result;
        }

        public bool Stop()
        {
            try
            {
                this.siloHost.StopOrleansSilo();

                Console.WriteLine("Orleans silo '{0}' shutdown.", this.siloHost.Name);
            }
            catch (Exception exc)
            {
                this.siloHost.ReportStartupError(exc);
                string msg = $"{exc.GetType().FullName}:\n{exc.Message}\n{exc.StackTrace}";
                Console.WriteLine(msg);
            }

            return true;
        }

        private void Init()
        {
            this.siloHost = new SiloHost(Dns.GetHostName())
            {
                ConfigFileName = "OrleansConfiguration.xml",
                DeploymentId = GuidUtility.NewSequentialGuid().ToGuidString(),
                Debug = true
            };
            this.siloHost.LoadOrleansConfig();
        }
    }
}
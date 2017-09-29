using Microsoft.WindowsAzure.ServiceRuntime;
using Moe.Lib;
using System;
using System.Diagnostics;
using System.Linq;

namespace Jinyinmao.AuthManager.Api
{
    /// <summary>
    ///     Class WebRole.
    /// </summary>
    public class WebRole : RoleEntryPoint
    {
        /// <summary>
        ///     Called when [start].
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool OnStart()
        {
            Trace.TraceInformation("OrleansAzureWeb-OnStart");

            // For information on handling configuration changes see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            bool ok = base.OnStart();

            Trace.TraceInformation("OrleansAzureWeb-OnStart completed with OK=" + ok);

            return ok;
        }

        /// <summary>
        ///     Called when [stop].
        /// </summary>
        public override void OnStop()
        {
            Trace.TraceInformation("OrleansAzureWeb-OnStop");
            base.OnStop();
        }

        /// <summary>
        ///     Runs this instance.
        /// </summary>
        public override void Run()
        {
            Trace.TraceInformation("OrleansAzureWeb-Run");
            try
            {
                base.Run();
            }
            catch (Exception exc)
            {
                Trace.TraceError("Run() failed with " + exc.GetExceptionString());
            }
        }

        /// <summary>
        ///     Roles the environment changing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoleEnvironmentChangingEventArgs" /> instance containing the event data.</param>
        private static void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}
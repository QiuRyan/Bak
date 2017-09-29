using System.Web;
using System.Web.Http;

namespace Jinyinmao.AuthManager.Api
{
    /// <summary>
    ///     Class WebApiApplication.
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        ///     Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
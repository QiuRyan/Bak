using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Moe.Lib.Jinyinmao;
using MoeLib.Jinyinmao.Azure;

namespace Jinyinmao.Government
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            App.Initialize().ConfigForAzure();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
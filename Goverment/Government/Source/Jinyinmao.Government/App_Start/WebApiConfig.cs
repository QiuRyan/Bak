using System.Web.Http;
using Moe.Lib.Web;

namespace Jinyinmao.Government
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.UseJinyinmaoExceptionHandler();

            config.UseJinyinmaoTraceEntryHandler();
            config.UseJinyinmaoLogHandler();

            config.UseJinyinmaoExceptionLogger();
            config.UseJinyinmaoTraceWriter();

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
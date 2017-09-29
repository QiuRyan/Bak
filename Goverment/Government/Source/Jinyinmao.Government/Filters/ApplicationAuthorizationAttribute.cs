using System;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Jinyinmao.Government.Models;
using Moe.Lib;
using Moe.Lib.Web;

namespace Jinyinmao.Government.Filters
{
    public class ApplicationAuthorizationAttribute : OrderedAuthorizationFilterAttribute
    {
        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization?.Scheme != null && string.Equals(actionContext.Request.Headers.Authorization.Scheme, "JIAUTH", StringComparison.OrdinalIgnoreCase))
                {
                    string ticket = actionContext.Request.Headers.Authorization.Parameter;
                    string roleName = ticket.Split(',')[0];
                    string sign = ticket.Split(',')[1];

                    using (GovernmentDbContext db = new GovernmentDbContext())
                    {
                        Application application = await db.ReadonlyQuery<Application>().FirstOrDefaultAsync(a => a.Role == roleName, cancellationToken);
                        RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048);
                        provider.FromXmlString(application.Keys.HtmlDecode());
                        bool verified = provider.VerifyData(roleName.GetBytesOfASCII(), new SHA1CryptoServiceProvider(), sign.ToBase64Bytes());
                        if (verified)
                        {
                            actionContext.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(roleName), null);
                        }
                        else
                        {
                            throw new ApplicationException("Can not verified the signed application token.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                this.HandleUnauthorizedRequest(actionContext);
            }

            await base.OnAuthorizationAsync(actionContext, cancellationToken);
        }

        /// <summary>
        ///     Processes requests that fail authorization. This default implementation creates a new response with the
        ///     Unauthorized status code. Override this method to provide your own handling for unauthorized requests.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        /// <exception cref="System.ArgumentNullException">@actionContext can not be null</exception>
        private void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext), @"actionContext can not be null");
            }

            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "");
        }
    }
}
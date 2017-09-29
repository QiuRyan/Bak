using System;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Jinyinmao.Government.Models;
using Moe.Lib;
using Moe.Lib.Web;
using MoeLib.Web;

namespace Jinyinmao.Government.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class StaffOperationAttribute : OrderedAuthorizationFilterAttribute
    {
        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (!await this.IsValid(actionContext) && !this.IsFromLocalHost(actionContext))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            await this.LogOperationAsync(actionContext);

            await base.OnAuthorizationAsync(actionContext, cancellationToken);
        }

        /// <summary>
        ///     Processes requests that fail authorization. This default implementation creates a new
        ///     response with the Unauthorized status code. Override this method to provide your own
        ///     handling for unauthorized requests.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        private static void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext), @"actionContext can not be null");
            }

            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Staff Only!");
        }

        private bool IsFromLocalHost(HttpActionContext actionContext)
        {
            if (actionContext.Request.IsLocal())
            {
                actionContext.RequestContext.Principal = new GenericPrincipal(new GenericIdentity("LocalHost"), null);
                return true;
            }

            return false;
        }

        private async Task<bool> IsValid(HttpActionContext actionContext)
        {
            if (actionContext?.Request == null)
            {
                throw new ArgumentNullException(nameof(actionContext), @"actionContext can not be null");
            }

            string staffKey = actionContext.Request.Headers.Authorization?.Parameter ?? actionContext.Request.GetCookie("StaffKey");
            if (staffKey.IsNullOrEmpty())
            {
                return false;
            }

            using (GovernmentDbContext db = new GovernmentDbContext())
            {
                Staff staff = await db.ReadonlyQuery<Staff>().FirstOrDefaultAsync(s => s.Key == staffKey);
                if (staff == null)
                {
                    return false;
                }

                actionContext.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(staff.Name), null);
                return true;
            }
        }

        private async Task LogOperationAsync(HttpActionContext actionContext)
        {
            using (GovernmentDbContext db = new GovernmentDbContext())
            {
                OperationLog log = new OperationLog
                {
                    Operation = actionContext.Request.RequestUri.AbsoluteUri,
                    Operator = actionContext.RequestContext.Principal.Identity.Name,
                    Parameters = await actionContext.Request.Content.ReadAsStringAsync(),
                    Time = DateTime.UtcNow.ToChinaStandardTime()
                };

                await db.SaveAsync(log);
            }
        }
    }
}
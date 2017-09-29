using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Jinyinmao.Government.Filters;
using Jinyinmao.Government.Models;
using Jinyinmao.Government.Response.Logs;
using MoeLib.Jinyinmao.Web;
using Swashbuckle.Swagger.Annotations;

namespace Jinyinmao.Government.Controllers
{
    [RoutePrefix("api/Logs")]
    [StaffOperation]
    public class LogsController : JinyinmaoApiController
    {
        private readonly GovernmentDbContext db = new GovernmentDbContext();

        [HttpGet, Route("ConfigurationFetch/{top:int}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<ConfigurationFetchLogResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> NewestConfigurationFetchLogs(int top = 20)
        {
            top = top > 0 ? top : 1;
            List<ConfigurationFetchLog> logs = await this.db.ReadonlyQuery<ConfigurationFetchLog>().OrderByDescending(l => l.Id).Take(top).ToListAsync();
            return this.Ok((logs ?? Enumerable.Empty<ConfigurationFetchLog>()).Select(l => l.ToResponse()));
        }

        [HttpGet, Route("Operation/{top:int}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<OperationLogResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> NewestOperationLogs(int top = 20)
        {
            top = top > 0 ? top : 1;
            List<OperationLog> logs = await this.db.ReadonlyQuery<OperationLog>().OrderByDescending(l => l.Id).Take(top).ToListAsync();
            return this.Ok((logs ?? Enumerable.Empty<OperationLog>()).Select(l => l.ToResponse()));
        }

        [HttpGet, Route("ConfigurationFetch/Search/{text:length(3, 20)}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<ConfigurationFetchLogResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> SearchConfigurationFetchLogs(string text)
        {
            List<ConfigurationFetchLog> applications = await this.db.ReadonlyQuery<ConfigurationFetchLog>()
                .Where(l => l.SourceRole.Contains(text) || l.SourceIP.Contains(text))
                .OrderByDescending(l => l.Id).Take(20).ToListAsync();

            return this.Ok((applications ?? Enumerable.Empty<ConfigurationFetchLog>()).Select(a => a.ToResponse()));
        }

        [HttpGet, Route("Operation/Search/{text:length(3, 20)}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<OperationLogResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> SearchOperationLogs(string text)
        {
            List<OperationLog> applications = await this.db.ReadonlyQuery<OperationLog>()
                .Where(l => l.Operation.Contains(text) || l.Operator.Contains(text))
                .OrderByDescending(l => l.Id).Take(20).ToListAsync();

            return this.Ok((applications ?? Enumerable.Empty<OperationLog>()).Select(a => a.ToResponse()));
        }

        /// <summary>
        ///     Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            this.db.Dispose();
            base.Dispose(disposing);
        }
    }
}
using Jinyinmao.Government.Filters;
using Jinyinmao.Government.Models;
using Jinyinmao.Government.Request.Applications;
using Jinyinmao.Government.Response.Applications;
using Moe.Lib;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Jinyinmao.Government.Controllers
{
    [RoutePrefix("api/Applications")]
    [StaffOperation]
    public class ApplicationsController : JinyinmaoApiController
    {
        private readonly GovernmentDbContext db = new GovernmentDbContext();

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApplicationResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get(int id)
        {
            Application application = await this.db.ReadonlyQuery<Application>().FirstOrDefaultAsync(a => a.Id == id);
            return application == null ? (IHttpActionResult)this.NotFound() : this.Ok(application.ToResponse());
        }

        [HttpGet]
        [Route("LastModified/{top:int}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<ApplicationResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> LastModified(int top = 20)
        {
            top = top > 0 ? top : 1;
            List<Application> applications = await this.db.ReadonlyQuery<Application>().OrderByDescending(s => s.LastModifiedTime).ThenBy(s => s.Id).Take(top).ToListAsync();
            return this.Ok((applications ?? Enumerable.Empty<Application>()).Select(a => a.ToResponse()));
        }

        [HttpGet]
        [Route("Newest/{top:int}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<ApplicationResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Newest(int top = 20)
        {
            top = top > 0 ? top : 1;
            List<Application> applications = await this.db.ReadonlyQuery<Application>().OrderByDescending(s => s.Id).Take(top).ToListAsync();
            return this.Ok((applications ?? Enumerable.Empty<Application>()).Select(a => a.ToResponse()));
        }

        [HttpPost]
        [Route("")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApplicationResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The application of the role \"{request.Role}\" already exsited.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Post(ApplicationRequest request)
        {
            if (await this.db.ReadonlyQuery<Application>().AnyAsync(a => a.Role == request.Role))
            {
                return this.BadRequest($"The application of the role \"{request.Role}\" already exsited.");
            }

            Application application = new Application
            {
                Configurations = request.Configurations,
                ConfigurationVersion = MD5Hash.ComputeMD5HashString(request.Configurations),
                KeyId = request.KeyId,
                Keys = request.Keys,
                Role = request.Role,
                ServiceEndpoint = request.ServiceEndpoint,
                CreatedBy = this.User.Identity.Name,
                CreatedTime = DateTime.UtcNow.ToChinaStandardTime(),
                LastModifiedBy = this.User.Identity.Name,
                LastModifiedTime = DateTime.UtcNow.ToChinaStandardTime()
            };

            await this.db.SaveAsync(application);
            return this.Ok(application.ToResponse());
        }

        [HttpPut]
        [Route("{id}")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApplicationResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The application of the role \"{request.Role}\" already exsited.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Put([FromUri] int id, ApplicationRequest request)
        {
            Application application = await this.db.Query<Application>().FirstOrDefaultAsync(a => a.Id == id);
            if (application == null)
            {
                return this.NotFound();
            }

            if (await this.db.ReadonlyQuery<Application>().AnyAsync(a => a.Id != application.Id && a.Role == request.Role))
            {
                return this.BadRequest($"The application of the role \"{request.Role}\" already exsited.");
            }

            application.Configurations = request.Configurations;
            application.ConfigurationVersion = MD5Hash.ComputeMD5HashString(request.Configurations);
            application.Keys = request.Keys;
            application.Role = request.Role;
            application.ServiceEndpoint = request.ServiceEndpoint;
            application.LastModifiedBy = this.User.Identity.Name;
            application.LastModifiedTime = DateTime.UtcNow.ToChinaStandardTime();

            await this.db.SaveAsync(application);
            return this.Ok(application.ToResponse());
        }

        [HttpGet]
        [Route("Search/{text:length(3, 20)}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<ApplicationResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Search([FromUri] string text)
        {
            List<Application> applications = await this.db.ReadonlyQuery<Application>().Where(s => s.Role.Contains(text) || s.ServiceEndpoint.Contains(text))
                .OrderByDescending(s => s.Id).Take(20).ToListAsync();

            return this.Ok((applications ?? Enumerable.Empty<Application>()).Select(a => a.ToResponse()));
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
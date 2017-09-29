using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Jinyinmao.Government.Filters;
using Jinyinmao.Government.Models;
using Jinyinmao.Government.Request.Permissions;
using Jinyinmao.Government.Response.Permissions;
using Moe.Lib;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web;
using Swashbuckle.Swagger.Annotations;

namespace Jinyinmao.Government.Controllers
{
    [RoutePrefix("api/Permissions")]
    [StaffOperation]
    public class PermissionsController : JinyinmaoApiController
    {
        private readonly GovernmentDbContext db = new GovernmentDbContext();

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PermissionResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get(int id)
        {
            Permission permission = await this.db.ReadonlyQuery<Permission>().FirstOrDefaultAsync(a => a.Id == id);
            return permission == null ? (IHttpActionResult)this.NotFound() : this.Ok(permission.ToResponse());
        }

        [HttpGet]
        [Route("LastModified/{top:int}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<PermissionResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> LastModified(int top = 20)
        {
            top = top > 0 ? top : 1;
            List<Permission> permissions = await this.db.ReadonlyQuery<Permission>().OrderByDescending(s => s.LastModifiedTime).ThenBy(s => s.Id).Take(top).ToListAsync();
            return this.Ok((permissions ?? Enumerable.Empty<Permission>()).Select(a => a.ToResponse()));
        }

        [HttpGet]
        [Route("Newest/{top:int}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<PermissionResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Newest(int top = 20)
        {
            top = top > 0 ? top : 1;
            List<Permission> permissions = await this.db.ReadonlyQuery<Permission>().OrderByDescending(s => s.Id).Take(top).ToListAsync();
            return this.Ok((permissions ?? Enumerable.Empty<Permission>()).Select(a => a.ToResponse()));
        }

        [HttpPost]
        [Route("")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PermissionResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The permission for the applicatins already exsited.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Post(PermissionRequest request)
        {
            if (await this.db.ReadonlyQuery<Permission>().AnyAsync(a => a.ObjectApplicationId == request.ObjectApplicationId && a.SubjectApplicationId == request.SubjectApplicationId))
            {
                return this.BadRequest("The permission for the applicatins already exsited.");
            }

            Permission permission = new Permission
            {
                Expiry = request.Expiry,
                ObjectApplicationId = request.ObjectApplicationId,
                PermissionLevel = request.PermissionLevel,
                SubjectApplicationId = request.SubjectApplicationId,
                CreatedBy = this.User.Identity.Name,
                CreatedTime = DateTime.UtcNow.ToChinaStandardTime(),
                LastModifiedBy = this.User.Identity.Name,
                LastModifiedTime = DateTime.UtcNow.ToChinaStandardTime()
            };

            await this.db.SaveAsync(permission);
            return this.Ok(permission.ToResponse());
        }

        [HttpPut]
        [Route("id")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PermissionResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The permission for the applicatins already exsited.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Put([FromUri] int id, PermissionRequest request)
        {
            Permission permission = await this.db.Query<Permission>().FirstOrDefaultAsync(a => a.Id == id);
            if (permission == null)
            {
                return this.NotFound();
            }

            if (await this.db.ReadonlyQuery<Permission>().AnyAsync(a => a.Id != permission.Id && a.ObjectApplicationId == request.ObjectApplicationId && a.SubjectApplicationId == request.SubjectApplicationId))
            {
                return this.BadRequest("The permission for the applicatins already exsited.");
            }

            permission.Expiry = request.Expiry;
            permission.ObjectApplicationId = request.ObjectApplicationId;
            permission.PermissionLevel = request.PermissionLevel;
            permission.SubjectApplicationId = request.SubjectApplicationId;
            permission.LastModifiedBy = this.User.Identity.Name;
            permission.LastModifiedTime = DateTime.UtcNow.ToChinaStandardTime();

            await this.db.SaveAsync(permission);
            return this.Ok(permission.ToResponse());
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
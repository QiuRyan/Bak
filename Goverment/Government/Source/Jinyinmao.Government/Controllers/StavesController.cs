using Jinyinmao.Government.Filters;
using Jinyinmao.Government.Models;
using Jinyinmao.Government.Request.Staves;
using Jinyinmao.Government.Response.Staves;
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
    [RoutePrefix("api/Staves")]
    [StaffOperation]
    public class StavesController : JinyinmaoApiController
    {
        private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly GovernmentDbContext db = new GovernmentDbContext();

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StaffResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get(int id)
        {
            Staff staff = await this.db.ReadonlyQuery<Staff>().FirstOrDefaultAsync(s => s.Id == id);
            return staff == null ? (IHttpActionResult)this.NotFound() : this.Ok(staff.ToResponse());
        }

        [HttpGet]
        [Route("GetByCellphone/{cellphone}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StaffResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetByCellphone(string cellphone)
        {
            Staff staff = await this.db.ReadonlyQuery<Staff>().FirstOrDefaultAsync(s => s.Cellphone == cellphone);
            return staff == null ? (IHttpActionResult)this.NotFound() : this.Ok(staff.ToResponse());
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StaffResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetByName(string name)
        {
            Staff staff = await this.db.ReadonlyQuery<Staff>().FirstOrDefaultAsync(s => s.Name == name);
            return staff == null ? (IHttpActionResult)this.NotFound() : this.Ok(staff.ToResponse());
        }

        [HttpGet]
        [Route("LastModified/{top:int}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<StaffResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> LastModified(int top = 20)
        {
            top = top > 0 ? top : 1;
            List<Staff> staves = await this.db.ReadonlyQuery<Staff>().OrderByDescending(s => s.LastModifiedTime).ThenBy(s => s.Id).Take(top).ToListAsync();
            return this.Ok((staves ?? Enumerable.Empty<Staff>()).Select(a => a.ToResponse()));
        }

        [HttpGet]
        [Route("Newest/{top:int}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<StaffResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Newest(int top = 20)
        {
            top = top > 0 ? top : 1;
            List<Staff> staves = await this.db.ReadonlyQuery<Staff>().OrderByDescending(s => s.Id).Take(top).ToListAsync();
            return this.Ok((staves ?? Enumerable.Empty<Staff>()).Select(a => a.ToResponse()));
        }

        [HttpPost]
        [Route("")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StaffWithKeyResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The staff with the name \"{request.Name}\" already exsited..")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Post(StaffReqeust request)
        {
            if (await this.db.ReadonlyQuery<Staff>().AnyAsync(s => s.Name == request.Name))
            {
                return this.BadRequest($"The staff with the name \"{request.Name}\" already exsited.");
            }

            Staff staff = new Staff
            {
                Cellphone = request.Cellphone,
                Email = request.Email,
                Key = GenerateRandomString(),
                Name = request.Name,
                CreatedBy = this.User.Identity.Name,
                CreatedTime = DateTime.UtcNow.ToChinaStandardTime(),
                LastModifiedBy = this.User.Identity.Name,
                LastModifiedTime = DateTime.UtcNow.ToChinaStandardTime()
            };

            await this.db.SaveAsync(staff);
            return this.Ok(staff.ToStaffWithKeyResponse());
        }

        [HttpPut]
        [Route("id")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StaffWithKeyResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The staff with the name \"{request.Name}\" already exsited..")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Put([FromUri] int id, StaffReqeust request)
        {
            Staff staff = await this.db.Query<Staff>().FirstOrDefaultAsync(s => s.Id == id);
            if (staff == null)
            {
                return this.NotFound();
            }

            if (await this.db.ReadonlyQuery<Staff>().AnyAsync(s => s.Id != staff.Id && s.Name == request.Name))
            {
                return this.BadRequest($"The staff with the name \"{request.Name}\" already exsited.");
            }

            staff.Cellphone = request.Cellphone;
            staff.Email = request.Email;
            staff.Name = request.Name;
            staff.LastModifiedBy = this.User.Identity.Name;
            staff.LastModifiedTime = DateTime.UtcNow.ToChinaStandardTime();

            await this.db.SaveAsync(staff);
            return this.Ok(staff.ToStaffWithKeyResponse());
        }

        [HttpPost]
        [Route("{id}/RegenerateStaffKey")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StaffWithKeyResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> RegenerateStaffKey([FromUri] int id)
        {
            Staff staff = await this.db.Query<Staff>().FirstOrDefaultAsync(s => s.Id == id);
            if (staff == null)
            {
                return this.NotFound();
            }

            staff.Key = GenerateRandomString();
            staff.LastModifiedBy = this.User.Identity.Name;
            staff.LastModifiedTime = DateTime.UtcNow.ToChinaStandardTime();

            await this.db.ExecuteSaveChangesAsync();

            return this.Ok(staff.ToStaffWithKeyResponse());
        }

        [HttpGet]
        [Route("Search/{text:length(3, 20)}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<StaffResponse>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Search([FromUri] string text)
        {
            List<Staff> staves = await this.db.ReadonlyQuery<Staff>().Where(s => s.Name.Contains(text) || s.Cellphone.Contains(text))
                .OrderByDescending(s => s.Id).Take(20).ToListAsync();
            return this.Ok((staves ?? Enumerable.Empty<Staff>()).Select(s => s.ToResponse()));
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

        private static string GenerateRandomString()
        {
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
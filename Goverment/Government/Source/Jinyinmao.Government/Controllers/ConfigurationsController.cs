using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using Jinyinmao.Government.Filters;
using Jinyinmao.Government.Models;
using Jinyinmao.Government.Models.Request.Configurations;
using Jinyinmao.Government.Models.Response.Configurations;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using Moe.Lib.Web;
using MoeLib.Jinyinmao.Web;
using MoeLib.Web;
using Swashbuckle.Swagger.Annotations;

namespace Jinyinmao.Government.Controllers
{
    [RoutePrefix("api/Configurations")]
    public class ConfigurationsController : JinyinmaoApiController
    {
        private readonly Lazy<RSACryptoServiceProvider> cryptoServiceProvider = new Lazy<RSACryptoServiceProvider>(() => InitRSACryptoServiceProvider());
        private readonly GovernmentDbContext db = new GovernmentDbContext();

        private RSACryptoServiceProvider CryptoServiceProvider
        {
            get { return this.cryptoServiceProvider.Value; }
        }

        [HttpPost, Route("")]
        [ApplicationAuthorization]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ConfigurationResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Fetch(FetchConfigurationRequest request)
        {
            Application application = await this.db.ReadonlyQuery<Application>().FirstOrDefaultAsync(a => a.Role == this.User.Identity.Name);
            if (application == null)
            {
                return this.NotFound();
            }

            ConfigurationFetchLog log = new ConfigurationFetchLog
            {
                FetchedVersion = application.ConfigurationVersion,
                SourceIP = this.Request.GetUserHostAddress(),
                SourceRole = this.User.Identity.Name,
                SourceRoleInstance = request.RoleInstance,
                SourceVersion = request.SourceVersion
            };

            await this.db.SaveAsync(log);

            return this.Ok(new ConfigurationResponse
            {
                Configurations = application.Configurations,
                ConfigurationVersion = application.ConfigurationVersion,
                Permissions = await this.GeneratePermissionsAsync(application)
            });
        }

        [HttpGet, Route("{applicationId:int}")]
        [StaffOperation]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ConfigurationResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get(int applicationId)
        {
            Application application = await this.db.ReadonlyQuery<Application>().FirstOrDefaultAsync(a => a.Id == applicationId);
            if (application == null)
            {
                return this.NotFound();
            }

            return this.Ok(new ConfigurationResponse
            {
                Configurations = application.Configurations,
                ConfigurationVersion = application.ConfigurationVersion,
                Permissions = await this.GeneratePermissionsAsync(application)
            });
        }

        private static RSACryptoServiceProvider InitRSACryptoServiceProvider()
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048);
            provider.FromXmlString(App.Host.AppKeys);
            return provider;
        }

        private async Task<Dictionary<string, KeyValuePair<string, string>>> GeneratePermissionsAsync(Application application)
        {
            Dictionary<string, KeyValuePair<string, string>> services = new Dictionary<string, KeyValuePair<string, string>>();
            List<Permission> permissions = await this.db.ReadonlyQuery<Permission>().Where(p =>
                p.SubjectApplicationId == application.Id && p.PermissionLevel > 0 && p.Expiry > DateTime.UtcNow).ToListAsync();

            foreach (Permission permission in permissions)
            {
                Application objectApplication = await this.db.ReadonlyQuery<Application>().FirstOrDefaultAsync(a => a.Id == permission.ObjectApplicationId);
                if (objectApplication != null)
                {
                    string ticket = $"{application.Role},{objectApplication.Role},{permission.PermissionLevel.GetPermissionLevelValue()},{DateTime.UtcNow.AddDays(1).UnixTimestamp()}";
                    string sign = this.CryptoServiceProvider.SignData(ticket.GetBytesOfASCII(), new SHA1CryptoServiceProvider()).ToBase64String();
                    string token = $"{ticket},{sign}";
                    services.Add(objectApplication.Role, new KeyValuePair<string, string>(objectApplication.ServiceEndpoint, token));
                }
            }

            return services;
        }
    }
}
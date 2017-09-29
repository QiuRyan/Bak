using System;
using System.ComponentModel.DataAnnotations;
using Jinyinmao.Government.Models;
using Newtonsoft.Json;

namespace Jinyinmao.Government.Response.Applications
{
    public class ApplicationResponse
    {
        [Required]
        [JsonProperty("configurationVersion")]
        public string ConfigurationVersion { get; set; }

        [Required]
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [Required]
        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }

        [Required]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("keyId")]
        public string KeyId { get; set; }

        [Required]
        [JsonProperty("lastModifiedBy")]
        public string LastModifiedBy { get; set; }

        [Required]
        [JsonProperty("lastModifiedTime")]
        public DateTime LastModifiedTime { get; set; }

        [Required]
        [JsonProperty("serviceEndpoint")]
        public string ServiceEndpoint { get; set; }
    }

    internal static class ApplicationEx
    {
        internal static ApplicationResponse ToResponse(this Application application)
        {
            return new ApplicationResponse
            {
                Id = application.Id,
                ServiceEndpoint = application.ServiceEndpoint,
                ConfigurationVersion = application.ConfigurationVersion,
                KeyId = application.KeyId,
                CreatedBy = application.CreatedBy,
                CreatedTime = application.CreatedTime,
                LastModifiedBy = application.LastModifiedBy,
                LastModifiedTime = application.LastModifiedTime
            };
        }
    }
}
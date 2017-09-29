using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.Government.Models.Response.Configurations
{
    public class ConfigurationResponse
    {
        [Required]
        [JsonProperty("configurations")]
        public string Configurations { get; set; }

        [Required]
        [JsonProperty("configurationVersion")]
        public string ConfigurationVersion { get; set; }

        [Required]
        [JsonProperty("permissions")]
        public Dictionary<string, KeyValuePair<string, string>> Permissions { get; set; }
    }
}
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.Government.Models.Request.Configurations
{
    public class FetchConfigurationRequest
    {
        [Required]
        [JsonProperty("role")]
        [StringLength(255)]
        public string Role { get; set; }

        [Required]
        [JsonProperty("roleInstance")]
        [StringLength(255)]
        public string RoleInstance { get; set; }

        [Required]
        [JsonProperty("sourceVersion")]
        [StringLength(255)]
        public string SourceVersion { get; set; }
    }
}
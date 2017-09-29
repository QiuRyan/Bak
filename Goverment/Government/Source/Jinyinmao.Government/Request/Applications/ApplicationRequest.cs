using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Jinyinmao.Government.Request.Applications
{
    public class ApplicationRequest
    {
        [Required]
        [JsonProperty("configurations")]
        [StringLength(50000)]
        public string Configurations { get; set; }

        [Required]
        [JsonProperty("keyId")]
        [StringLength(32, MinimumLength = 32)]
        public string KeyId { get; set; }

        [Required]
        [JsonProperty("keys")]
        [StringLength(50000)]
        public string Keys { get; set; }

        [Required]
        [JsonProperty("role")]
        [StringLength(255)]
        public string Role { get; set; }

        [Required]
        [JsonProperty("serviceEndpoint")]
        [RegularExpression("^(https?|ftp)://[^\\s/$.?#].[^\\s]*$")]
        [StringLength(255)]
        public string ServiceEndpoint { get; set; }
    }
}
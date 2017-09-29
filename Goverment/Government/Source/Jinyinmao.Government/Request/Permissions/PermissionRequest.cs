using Jinyinmao.Government.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.Government.Request.Permissions
{
    public class PermissionRequest
    {
        [Required]
        [JsonProperty("expiry")]
        public DateTime Expiry { get; set; }

        [Required]
        [JsonProperty("objectApplicationId")]
        public int ObjectApplicationId { get; set; }

        [Required]
        [JsonProperty("permissionLevel")]
        public PermissionLevel PermissionLevel { get; set; }

        [Required]
        [JsonProperty("subjectApplicationId")]
        public int SubjectApplicationId { get; set; }
    }
}
using Jinyinmao.Government.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.Government.Response.Permissions
{
    public class PermissionResponse
    {
        [Required]
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [Required]
        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }

        [Required]
        [JsonProperty("expiry")]
        public DateTime Expiry { get; set; }

        [Required]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("lastModifiedBy")]
        public string LastModifiedBy { get; set; }

        [Required]
        [JsonProperty("lastModifiedTime")]
        public DateTime LastModifiedTime { get; set; }

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

    internal static class PermissionEx
    {
        internal static PermissionResponse ToResponse(this Permission permission)
        {
            return new PermissionResponse
            {
                Id = permission.Id,
                Expiry = permission.Expiry,
                ObjectApplicationId = permission.ObjectApplicationId,
                PermissionLevel = permission.PermissionLevel,
                SubjectApplicationId = permission.SubjectApplicationId,
                CreatedBy = permission.CreatedBy,
                CreatedTime = permission.CreatedTime,
                LastModifiedBy = permission.LastModifiedBy,
                LastModifiedTime = permission.LastModifiedTime
            };
        }
    }
}
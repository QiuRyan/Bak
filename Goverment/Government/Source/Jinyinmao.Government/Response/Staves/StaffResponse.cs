using Jinyinmao.Government.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.Government.Response.Staves
{
    public class StaffResponse
    {
        [Required]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        [Required]
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [Required]
        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }

        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }

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
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    internal static partial class StaffEx
    {
        internal static StaffResponse ToResponse(this Staff staff)
        {
            return new StaffResponse
            {
                Id = staff.Id,
                Cellphone = staff.Cellphone,
                Email = staff.Email,
                Name = staff.Name,
                CreatedBy = staff.CreatedBy,
                CreatedTime = staff.CreatedTime,
                LastModifiedBy = staff.LastModifiedBy,
                LastModifiedTime = staff.LastModifiedTime
            };
        }
    }
}
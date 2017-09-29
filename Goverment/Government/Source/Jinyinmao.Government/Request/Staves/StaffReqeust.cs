using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.Government.Request.Staves
{
    public class StaffReqeust
    {
        [Required]
        [JsonProperty("cellphone")]
        [RegularExpression("^(13|14|15|17|18)\\d{9}$")]
        public string Cellphone { get; set; }

        [Required]
        [JsonProperty("email")]
        [RegularExpression("^([a-z0-9_\\.-]+)@([\\da-z\\.-]+)\\.([a-z\\.]{2,6})$")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [JsonProperty("name")]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
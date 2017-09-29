using System;

namespace Jinyinmao.Government.Models
{
    public class Permission : Model
    {
        public DateTime Expiry { get; set; }

        public int ObjectApplicationId { get; set; }

        public PermissionLevel PermissionLevel { get; set; }

        public int SubjectApplicationId { get; set; }
    }
}
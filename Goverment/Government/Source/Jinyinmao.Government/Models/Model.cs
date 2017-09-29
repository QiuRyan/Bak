using System;

namespace Jinyinmao.Government.Models
{
    public abstract class Model
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public int Id { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedTime { get; set; }
    }
}
using System;

namespace Jinyinmao.Government.Models
{
    public class OperationLog
    {
        public int Id { get; set; }

        public string Operation { get; set; }

        public string Operator { get; set; }

        public string Parameters { get; set; }

        public DateTime Time { get; set; }
    }
}
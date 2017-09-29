using System;

namespace Jinyinmao.Government.Models
{
    public class ConfigurationFetchLog
    {
        public string FetchedVersion { get; set; }

        public int Id { get; set; }

        public string SourceIP { get; set; }

        public string SourceRole { get; set; }

        public string SourceRoleInstance { get; set; }

        public string SourceVersion { get; set; }

        public DateTime Time { get; set; }
    }
}
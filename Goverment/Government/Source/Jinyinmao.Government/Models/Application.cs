namespace Jinyinmao.Government.Models
{
    public class Application : Model
    {
        public string Configurations { get; set; }

        public string ConfigurationVersion { get; set; }

        public string KeyId { get; set; }

        public string Keys { get; set; }

        public string Role { get; set; }

        public string ServiceEndpoint { get; set; }
    }
}
namespace Jinyinmao.MessageWorker.Domain.Bll.Config
{
    internal static class ConfigManager
    {
        static ConfigManager()
        {
            IConfigProvider config = new ConfigProvider();
            config.InitConfig();
            config.CheckConfig();
            Config = config;
        }

        internal static IConfigProvider Config { get; private set; }
    }
}
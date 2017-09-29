#if DEBUG
#else

using Moe.Lib.Jinyinmao;

#endif

namespace Jinyinmao.ValidateCode.Api.Config
{
    public static class ConfigsManager
    {
        public static int DefaultMaxSendTimes
        {
            get
            {
#if DEBUG
                return 10;
#else
                return App.Condigurations.GetConfig<ValidateCodeApiConfig>().DefaultMaxSendTimes;
#endif
            }
        }

        public static int MaxSendTimeForQuickLogin
        {
            get
            {
#if DEBUG
                return 20;
#else
                return App.Condigurations.GetConfig<ValidateCodeApiConfig>().MaxSendTimeForQuickLogin;
#endif
            }
        }

        public static int VeriCodeExpiryMinites
        {
            get
            {
#if DEBUG
                return 30;
#else
                return App.Condigurations.GetConfig<ValidateCodeApiConfig>().VeriCodeExpiryMinites;
#endif
            }
        }

        public static string InnerMessageSendUrl
        {
            get
            {
#if DEBUG
                return "/api/MessageManager/SendWithTemplate";
#else
                return App.Condigurations.GetConfig<ValidateCodeApiConfig>().InnerMessageSendUrl;
#endif
            }
        }

        public static string MessageManagerServiceRoleName
        {
            get { return "Jinyinmao.MessageManager.Api"; }
        }

        public static string MessageManagerDbContext
        {
            get
            {
#if DEBUG
                return "Data Source=10.1.25.30;Database=jym-message;Integrated Security=False;User ID=db-admin-dev;Password=0SmDXp8i7MRfg29HJk1N;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
#else
                return App.Condigurations.GetConfig<ValidateCodeApiConfig>().MessageManagerDbContext;
#endif
            }
        }
    }
}
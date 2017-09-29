using System.Collections.Generic;

namespace Jinyinmao.Deposit.Lib.Enum
{
    public class JYMStatusCode
    {
        public static readonly List<int> ErrorCodes = new List<int> { InternetServerError, ServiceUnavailable, GetawatTimeOut };
        public static int GetawatTimeOut => 504;
        public static int InternetServerError => 500;
        public static int ServiceUnavailable => 503;
    }
}
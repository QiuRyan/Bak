namespace Jinyinmao.Deposit.Domain
{
    public static class StatusCodes
    {
        public static int[] ErrorCodes = { InternetServerError, ServiceUnavailable, GetawatTimeOut };
        public static int GetawatTimeOut => 504;
        public static int InternetServerError => 500;
        public static int ServiceUnavailable => 503;
    }
}
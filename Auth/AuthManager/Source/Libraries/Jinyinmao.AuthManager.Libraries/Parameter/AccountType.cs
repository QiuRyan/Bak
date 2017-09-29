using System.Collections.Generic;
using System.Linq;

namespace Jinyinmao.AuthManager.Libraries.Parameter
{
    public class AccountType
    {
        public static readonly AccountType Cellphone = new AccountType(10);

        public static readonly AccountType WeChat = new AccountType(20);

        private static readonly Dictionary<string, AccountType> Dic = new Dictionary<string, AccountType>
        {
            { "WeChat", WeChat },
            { "Cellphone", Cellphone }
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        private AccountType(int code)
        {
            this.Code = code;
        }

        public int Code { get; }

        public static AccountType GetAccountType(string key)
        {
            return Dic.Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault();
        }

        public static int GetAccountTypeCode(string key)
        {
            return Dic.Where(x => x.Key == key).Select(x => x.Value.Code).FirstOrDefault();
        }
    }
}
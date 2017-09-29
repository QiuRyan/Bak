using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using jinyinmao.Signature.lib.Common;

namespace jinyinmao.Signature.lib
{
    public static class JYMDBHelper
    {
        /// <summary>获取需要签章的产品列表</summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetRegularProductLists()
        {
            using (JYMDBContext context = new JYMDBContext(ConfigManager.BizDBConnectionString))
            {
                string sql = $"{ConfigManager.RegularQueryString}";

                return context.Database.SqlQuery<string>(sql).ToListAsync().GetAwaiter().GetResult();
            }
        }

        /// <summary>根据订单号获取用户id</summary>
        /// <param name="orderid">The orderid.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public static async Task<string> GetUserIdByOrderIdAsync(string orderid)
        {
            using (JYMDBContext db = new JYMDBContext(ConfigManager.BizDBConnectionString))
            {
                string sql = $"SELECT UserIdentifier from Orders  WHERE  OrderIdentifier='{orderid}' ";

                return await db.Database.SqlQuery<string>(sql).SingleAsync();
            }
        }

        /// <summary>余额猫订单信息</summary>
        /// <returns>List&lt;YEMOrderInfo&gt;.</returns>
        public static List<YEMOrderInfo> GetYemTransactionList()
        {
            using (JYMDBContext context = new JYMDBContext(ConfigManager.BizDBConnectionString))
            {
                string sql = $" {ConfigManager.YEMQueryString} ";

                return context.Database.SqlQuery<YEMOrderInfo>(sql).ToListAsync().Result;
            }
        }

        #region Nested type: JYMDBContext

        private sealed class JYMDBContext : DbContext
        {
            public JYMDBContext(string name) : base(name)
            {
            }
        }

        #endregion Nested type: JYMDBContext
    }
}
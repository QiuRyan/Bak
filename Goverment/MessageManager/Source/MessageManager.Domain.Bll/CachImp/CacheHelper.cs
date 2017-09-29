using Jinyinmao.MessageManager.Domain.Entity;
using System;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace Jinyinmao.MessageManager.Domain.Bll.CachImp
{
    /// <summary>
    ///
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="cacheMinute"></param>
        public static void Add(string cacheKey, object obj, int cacheMinute)
        {
            HttpRuntime.Cache.Insert(cacheKey, obj, null, DateTime.Now.AddMinutes(cacheMinute),
                Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static object Get(string cacheKey)
        {
            return HttpRuntime.Cache[cacheKey];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <returns></returns>
        public static MessageTemplate GetMemberSigninDays(MessageTemplate messageTemplate)
        {
            const int cacheTime = 5;
            const string cacheKey = "messageTemplate";

            //缓存标记。
            const string cacheSign = cacheKey + "_Sign";
            object sign = Get(cacheSign);

            //获取缓存值
            object cacheValue = Get(cacheKey);
            if (sign != null)
                return cacheValue as MessageTemplate;
            //未过期，直接返回。

            lock (cacheSign)
            {
                sign = Get(cacheSign);
                if (sign != null)
                    return cacheValue as MessageTemplate;

                Add(cacheSign, "1", cacheTime);
                ThreadPool.QueueUserWorkItem(arg =>
                {
                    cacheValue = messageTemplate;
                    //这里一般是 sql查询数据。 例：395 签到天数
                    Add(cacheKey, cacheValue, cacheTime * 2);
                    //日期设缓存时间的2倍，用于脏读。
                });
            }
            return cacheValue as MessageTemplate;
        }
    }
}
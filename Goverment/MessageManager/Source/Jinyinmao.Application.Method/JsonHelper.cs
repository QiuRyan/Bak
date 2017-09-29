using Moe.Lib;
using Newtonsoft.Json;

namespace Jinyinmao.Application.Method
{
    public class JsonHelper
    {
        #region 将一个对象转换为Json字符串

        /// <summary>
        ///     将一个对象转换为Json字符串
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        public static string EntityToStringJson(object obj)
        {
            //var strjson = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            //{
            //    PreserveReferencesHandling = PreserveReferencesHandling.Objects
            //});
            string strjson = obj.ToJson();
            return strjson;
        }

        #endregion 将一个对象转换为Json字符串

        /// <summary>
        ///     Json字符串类型转换成对象
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <param name="obj">The object.</param>
        /// <returns>System.Object.</returns>
        public static object StringJsonToEntity(string jsonString, object obj)
        {
            object myobj = JsonConvert.DeserializeObject<object>(jsonString);
            return myobj;
        }
    }
}
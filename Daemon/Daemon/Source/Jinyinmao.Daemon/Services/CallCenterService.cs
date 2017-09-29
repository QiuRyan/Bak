using Dapper;
using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Microsoft.Azure;
using Moe.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jinyinmao.Daemon.Services
{
    public class CallCenterService
    {
        #region 获取时间段的注册/认证用户SQL

        private static readonly string UserSelectSql = @"SELECT
                                                            UserIdentifier AS [customerGuid],--用户ID
                                                            Cellphone AS [registerPhone],--注册手机号
                                                            RealName AS [Name],--真实姓名
                                                            [Credential] AS [papersType],--证件类型
                                                            CredentialNo AS [papersNumber],--证件号码
                                                            RegisterTime AS [registerDate],--注册时间
                                                            VerifiedTime AS [validateDate],--认证通过时间
                                                            ClientType as useplatform, --注册平台
                                                            ContractId as channelid --渠道ID
                                                        FROM dbo.[Users]
                                                        WHERE {0}>='{1}' and {0}<'{2}'";

        #endregion 获取时间段的注册/认证用户SQL

        #region 修改手机号码sql

        private static readonly string UpdatephoneSql = @"SELECT NewCellphone as registerPhone
                                                            ,UserIdentifier as customerGuid
                                                            FROM dbo.ChangeLog where CreateTime>='{0}'";

        #endregion 修改手机号码sql

        #region 忽略null值

        private CallCenterEntity SetData(CallCenterEntity obj, List<AgreementTable> listAt = null)
        {
            if (string.IsNullOrEmpty(obj.customerguid))
            {
                obj.customerguid = "";
            }
            if (string.IsNullOrEmpty(obj.registerphone))
            {
                obj.registerphone = "";
            }
            if (string.IsNullOrEmpty(obj.name))
            {
                obj.name = "";
            }
            if (string.IsNullOrEmpty(obj.papersnumber))
            {
                obj.papersnumber = "";
            }
            if (string.IsNullOrEmpty(obj.registerdate))
            {
                obj.registerdate = "";
            }
            if (string.IsNullOrEmpty(obj.validatedate))
            {
                obj.validatedate = "";
            }

            if (string.IsNullOrEmpty(obj.custlevelname))
            {
                obj.custlevelname = "";
            }
            if (string.IsNullOrEmpty(obj.custstatusname))
            {
                obj.custstatusname = "";
            }
            if (string.IsNullOrEmpty(obj.useplatform))
            {
                obj.useplatform = "";
            }
            else
            {
                switch (obj.useplatform)
                {
                    case "900":
                        obj.useplatform = "PC";
                        break;

                    case "901":
                        obj.useplatform = "IOS";
                        break;

                    case "902":
                        obj.useplatform = "Android";
                        break;

                    case "903":
                        obj.useplatform = "M";
                        break;
                }
            }
            if (string.IsNullOrEmpty(obj.tel1))
            {
                obj.tel1 = "";
            }
            if (string.IsNullOrEmpty(obj.tel2))
            {
                obj.tel2 = "";
            }
            if (string.IsNullOrEmpty(obj.validatephone))
            {
                obj.validatephone = "";
            }
            if (string.IsNullOrEmpty(obj.sex))
            {
                obj.sex = "";
            }
            if (string.IsNullOrEmpty(obj.paperstype))
            {
                obj.paperstype = "";
            }
            if (string.IsNullOrEmpty(obj.lastcontactdate))
            {
                obj.lastcontactdate = "";
            }
            if (string.IsNullOrEmpty(obj.custtagname))
            {
                obj.custtagname = "";
            }
            if (string.IsNullOrEmpty(obj.birthday))
            {
                obj.birthday = "";
            }
            if (string.IsNullOrEmpty(obj.pathway))
            {
                obj.pathway = "";
            }
            if (string.IsNullOrEmpty(obj.serviceusername))
            {
                obj.serviceusername = "";
            }
            if (string.IsNullOrEmpty(obj.OB))
            {
                obj.OB = "";
            }
            if (string.IsNullOrEmpty(obj.IB))
            {
                obj.IB = "";
            }

            if (string.IsNullOrEmpty(obj.channelname))
            {
                obj.channelname = "";
            }

            if (string.IsNullOrEmpty(obj.channelid))
            {
                obj.channelid = "";
            }
            else
            {
                obj.channelname = this.GetChannelTableName(listAt, obj.channelid);
            }

            

            return obj;
        }

        #endregion 忽略null值

        #region Json反序列化

        /// <summary>
        ///     Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson">The string json.</param>
        /// <returns>T.</returns>
        private static T FromJson<T>(string strJson) where T : class
        {
            return !string.IsNullOrEmpty(strJson) ? JsonConvert.DeserializeObject<T>(strJson) : null;
        }

        #endregion Json反序列化

        #region 逻辑方法

        #region 添加注册用户

        private async Task SyncRegisterUserDataToCallCenterRun()
        {
            List<AgreementTable> listAt = this.ChannelTableGetList();
            using (SqlConnection connection = GetOpenConnection(bizConnectionString))
            {
                string strSql = UserSelectSql.FormatWith("RegisterTime", DateTime.UtcNow.ToChinaStandardTime().AddDays(-(int.Parse(callCenterDays) + 1)).ToString("yyyy-MM-dd"), DateTime.UtcNow.ToChinaStandardTime().ToString("yyyy-MM-dd"));

                //string strSql = UserSelectSql.FormatWith("RegisterTime", "2015-10-21 00:00:00.000", "2015-10-28 23:59:59.999");
                IEnumerable<CallCenterEntity> callCenterEntities = await connection.QueryAsync<CallCenterEntity>(strSql);
                List<CallCenterEntity> listerror = new List<CallCenterEntity>();
                CallCenterEntity[] centerEntities = callCenterEntities as CallCenterEntity[] ?? callCenterEntities.ToArray();
             
                foreach (CallCenterEntity info in centerEntities)
                {
                    #region 插入呼叫中心

                    CallCenterEntity info1 = this.SetData(info, listAt);
                    try
                    {
                        string strinfoJson = info1.ToJson();
                        FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            { "Token", "hfkwhkfwwwtretertybh345" },
                            { "Jsondata", strinfoJson }
                        });

                        //写入呼叫系统
                        HttpResponseMessage responseMessage = await this.CallCenterClient.PostAsync($"{callCenterApiHost}restapi/postCust.ashx", content);
                        if (responseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            dynamic str = FromJson<dynamic>(responseMessage.Content.ReadAsStringAsync().Result);
                            if (str.res.ToString() != "200" && str.res.ToString() != "404")
                            {
                                listerror.Add(info1);
                            }
                        }
                        else
                        {
                            listerror.Add(info1);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError($"呼叫中心注册用户导入 Error:连接失败{ex.Message}", new Exception());
                        break;
                        // listerror.Add(info);
                    }
                    #endregion
                }
                if (listerror.Count > 0)
                {
                    string strlisterror = listerror.ToJson();
                    LogHelper.LogError($"呼叫中心注册用户导入 Error:{strlisterror}", new Exception());
                }
            }
        }

        #endregion 添加注册用户

        #region 修改认证用户

        private async Task SyncRegisterUserDataToCallCenterUpdateRun()
        {
            using (SqlConnection connection = GetOpenConnection(bizConnectionString))
            {
                string strSql = UserSelectSql.FormatWith("VerifiedTime", DateTime.UtcNow.ToChinaStandardTime().AddDays(-(int.Parse(callCenterDays) + 1)).ToString("yyyy-MM-dd"), DateTime.UtcNow.ToChinaStandardTime().ToString("yyyy-MM-dd"));
                //string strSql = UserSelectSql.FormatWith("VerifiedTime", "2016-08-15");
                IEnumerable<CallCenterEntity> callCenterEntities = await connection.QueryAsync<CallCenterEntity>(strSql);
                List<CallCenterEntity> listerror = new List<CallCenterEntity>();
                CallCenterEntity[] centerEntities = callCenterEntities as CallCenterEntity[] ?? callCenterEntities.ToArray();
                foreach (CallCenterEntity info in centerEntities)
                {
                    CallCenterEntity info1 = this.SetData(info);
                    try
                    {
                        string strinfoJson = info1.ToJson();
                        FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            { "Token", "hfkwhkfwwwtretertybh345" },
                            { "Jsondata", strinfoJson }
                        });

                        //写入呼叫系统
                        HttpResponseMessage responseMessage = await this.CallCenterClient.PostAsync($"{callCenterApiHost}restapi/updateCust.ashx", content);
                        if (responseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            dynamic str = FromJson<dynamic>(responseMessage.Content.ReadAsStringAsync().Result);
                            if (str.res.ToString() != "200")
                            {
                                if (str.res.ToString() == "406")
                                {
                                    LogHelper.LogError($"呼叫中心认证用户更新 Error:{str.resMsg},userid{info1.customerguid}", new Exception());
                                }
                                else
                                {
                                    listerror.Add(info1);
                                }
                            }
                        }
                        else
                        {
                            listerror.Add(info1);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError($"呼叫中心认证用户更新 Error:连接失败{ex.Message}", new Exception());
                        break;
                        // listerror.Add(info);
                    }
                }
                if (listerror.Count > 0)
                {
                    string strlisterror = listerror.ToJson();
                    LogHelper.LogError($"呼叫中心认证用户更新 Error:{strlisterror}", new Exception());
                }
            }
        }

        #endregion 修改认证用户

        #region 修改用户手机号码

        private async Task UpdateCellphone()
        {
            using (SqlConnection connection = GetOpenConnection(authConnectionString))
            {
                string strSql = UpdatephoneSql.FormatWith(DateTime.UtcNow.ToChinaStandardTime().AddDays(-(int.Parse(callCenterDays))).ToString("yyyy-MM-dd"));
                IEnumerable<CallCenterEntity> callCenterEntities = await connection.QueryAsync<CallCenterEntity>(strSql);
                List<CallCenterEntity> listerror = new List<CallCenterEntity>();

                CallCenterEntity[] centerEntities = callCenterEntities as CallCenterEntity[] ?? callCenterEntities.ToArray();
                foreach (CallCenterEntity info in centerEntities)
                {
                    CallCenterEntity info1 = this.SetData(info);
                    #region 写入呼叫系统
                    try
                    {
                        string strinfoJson = info1.ToJson();
                        FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            { "Token", "hfkwhkfwwwtretertybh345" },
                            { "Jsondata", strinfoJson }
                        });

                        //写入呼叫系统
                        HttpResponseMessage responseMessage = await this.CallCenterClient.PostAsync($"{callCenterApiHost}restapi/updateCust.ashx", content);
                        if (responseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            dynamic str = FromJson<dynamic>(responseMessage.Content.ReadAsStringAsync().Result);
                            if (str.res.ToString() != "200")
                            {
                                if (str.res.ToString() == "406")
                                {
                                    LogHelper.LogError($"呼叫中心更新手机号码 Error_res:{str.res.ToString()}  Error_Msg:{str.resMsg},userid{info1.customerguid}", new Exception());
                                }
                                else
                                {
                                    listerror.Add(info1);
                                }
                            }
                        }
                        else
                        {
                            listerror.Add(info1);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError($"呼叫中心更新手机号码 Error:连接失败{ex.Message}", new Exception());
                        break;
                        // listerror.Add(info);
                    }
                    #endregion
                }
                if (listerror.Count > 0)
                {
                    string strlisterror = listerror.ToJson();
                    LogHelper.LogError($"呼叫中心更新手机号码 Error:{strlisterror}", new Exception());
                }
            }
        }

        #endregion 修改用户手机号码

        #region 修改会员等级

        /// <summary>
        ///     修改会员等级
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private async Task UpdateLevel()
        {
            using (SqlConnection connection = GetOpenConnection(memberConnectionString))
            {
                string sqlcount = "select count(*) as RowNum from Members where Closed=0 and Verified=1";
                IEnumerable<int> rowNumb = await connection.QueryAsync<int>(sqlcount);
                int[] enumerable = rowNumb as int[] ?? rowNumb.ToArray();
                if (enumerable.ToList()[0] > 0)
                {
                    #region 页数判断
                    int mycount = enumerable.ToList()[0];
                    int pageNumber = 1;
                    int pageSize = 1000;
                    int zpage;
                    if (mycount % pageSize == 0)
                    {
                        zpage = mycount / pageSize;
                    }
                    else
                    {
                        zpage = mycount / pageSize + 1;
                    }
                    #endregion

                    while (pageNumber <= zpage)
                    {
                        #region sql语句
                        StringBuilder sbsql = new StringBuilder();
                        sbsql.Append("select CustomerGuid,CustLevelName from ( ");
                        sbsql.Append("SELECT TOP " + ((((pageNumber - 1) * pageSize) + pageSize)) + " row_number() OVER (ORDER BY UpdateTime desc,CustomerGuid asc) n,mytables1.* ");
                        sbsql.Append("FROM (select UserIdentifier as CustomerGuid,Level as CustLevelName,UpdateTime from Members where Closed=0 and Verified=1) mytables1 ");
                        sbsql.Append(") tablesname1 ");
                        sbsql.Append("where tablesname1.n>" + ((pageNumber - 1) * pageSize) + " order by tablesname1.n asc ");
                        #endregion

                        IEnumerable<CallCenterEntity> list = await connection.QueryAsync<CallCenterEntity>(sbsql.ToString());
                        ParallelOptions option = new ParallelOptions { MaxDegreeOfParallelism = int.Parse(maxDegreeOfParallelism) };
                        
                        Parallel.ForEach(list, option, async info =>
                        {
                            #region 插入呼叫中心
                            try
                            {
                                CallCenterEntity info1 = this.SetData(info);
                                string strinfojson = info1.ToJson();
                                FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                                {
                                    { "Token", "hfkwhkfwwwtretertybh345" },
                                    { "Jsondata",  strinfojson}
                                });
                                //写入呼叫系统
                                HttpResponseMessage responseMessage = await this.CallCenterClient.PostAsync($"{callCenterApiHost}restapi/updateCust.ashx", content);
                                if (responseMessage.StatusCode == HttpStatusCode.OK)
                                {
                                    dynamic str = FromJson<dynamic>(responseMessage.Content.ReadAsStringAsync().Result);
                                    if (str.res.ToString() != "200" && str.res.ToString() != "406")
                                    {                                        
                                        LogHelper.LogError($"呼叫中心更新会员等级 接口更新失败，UserIdentifier：{info1.customerguid}，error_res:{str.res.ToString()}，error_resMsg：{str.resMsg.ToString()}", new Exception());
                                    }
                                }
                                else
                                {
                                    LogHelper.LogError("呼叫中心更新会员等级 接口更新失败，链接接口失败", new Exception());
                                }
                            }
                            catch (Exception ex)
                            {
                                string exMessage = ex.Message;
                                LogHelper.LogError($"呼叫中心更新会员等级 Error:连接报异常 Message:{exMessage}", new Exception());
                                //LogHelper.LogError($"呼叫中心更新会员等级 Error:连接失败{ex.Message}", new Exception());
                            }
                            #endregion
                        });
                        
                        pageNumber++;
                    }
                }
            }
        }

        #endregion 修改会员等级

        #region 修改用户状态        
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <returns>Task.</returns>
        private async Task UpdateUserStatus()
        {
            List<HjzxStateTable> listerror = new List<HjzxStateTable>();

            foreach (var item in RedisCacheHelper.GetRedisClient(3).HashScan("UserInfo", "*", 1))
            {
                var myvalue = item.Value;
                HjzxStateTable info1 = JsonConvert.DeserializeObject<HjzxStateTable>(myvalue);
                CallCenterEntity info = new CallCenterEntity();
                info.customerguid = info1.UserIdentifier;
                info.custtagname = info1.UserState;
                info = this.SetData(info);
                try
                {
                    string strinfoJson = info.ToJson();
                    #region 写入呼叫系统
                    FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                            { "Token", "hfkwhkfwwwtretertybh345" },
                            { "Jsondata", strinfoJson}
                    });

                   
                    //写入呼叫系统
                    HttpResponseMessage responseMessage = await this.CallCenterClient.PostAsync($"{callCenterApiHost}restapi/updateCust.ashx", content);
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        dynamic str = FromJson<dynamic>(responseMessage.Content.ReadAsStringAsync().Result);
                        if (str.res.ToString() != "200")
                        {
                            if (str.res.ToString() == "406")
                            {
                                LogHelper.LogError($"呼叫中心更新用户状态 Error:{str.resMsg},userid{info1.UserIdentifier}", new Exception());
                            }
                            else
                            {
                                listerror.Add(info1);
                            }
                        }
                    }
                    else
                    {
                        listerror.Add(info1);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    LogHelper.LogError($"呼叫中心更新用户状态 Error:连接失败{ex.Message}", new Exception());
                    break;
                }
            }

            if (listerror.Count > 0)
            {
                string strlisterror = listerror.ToJson();
                LogHelper.LogError($"呼叫中心更新用户状态 Error:{strlisterror}", new Exception());
            }
        }
        #endregion

        #region 返回协议名称
        public List<AgreementTable> ChannelTableGetList()
        {
            try
            {
                using (SqlConnection connection = GetOpenConnection(jymOperContextConnection))
                {
                    string strSql = "select Id,ACode,ChineseName from [dbo].[AgreementTable] where IsDelete=0";
                    List<AgreementTable> callCenterEntities = connection.Query<AgreementTable>(strSql).ToList();
                    if (callCenterEntities?.Count > 0)
                    {
                        return callCenterEntities;
                    }
                }
            }
            catch(Exception ex)
            { }
            return new List<AgreementTable>();
        }

       

        public string GetChannelTableName(List<AgreementTable> list,string ACode)
        {
            if (string.IsNullOrEmpty(ACode) || list == null)
            {
                return "";
            }
            if (list.Count == 0)
            {
                return "";
            }
            List<AgreementTable>  list1 = list.Where(c => c.ACode == ACode).ToList();
            if (list1?.Count > 0)
            {
                return list1[0].ChineseName;
            }
            return "";
        }

        #endregion

        #endregion

        #region hangfire 调用

        #region 查询数据库流水表并发出请求调用

        /// <summary>
        ///     查询数据库流水表并发出请求调用
        /// </summary>
        public void WorkInsert()
        {
            try
            {
                this.SyncRegisterUserDataToCallCenterRun().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("呼叫中心：Error: {0}".FormatWith(e.Message), e);
            }
        }

        #endregion 查询数据库流水表并发出请求调用

        #region 查询数据库流水表并发出请求调用

        /// <summary>
        ///     查询数据库流水表并发出请求调用
        /// </summary>
        public void WorkUpdate()
        {
            try
            {
                this.SyncRegisterUserDataToCallCenterUpdateRun().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("呼叫中心：Error:  {0}".FormatWith(e.Message), e);
            }
        }

        #endregion 查询数据库流水表并发出请求调用

        #region 修改手机号码调用

        /// <summary>
        ///     修改手机号码调用
        /// </summary>
        public void WordUpdatePhone()
        {
            try
            {
                this.UpdateCellphone().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("呼叫中心：Error:  {0}".FormatWith(e.Message), e);
            }
        }

        #endregion 修改手机号码调用

        #region 更新会员等级调用

        /// <summary>
        ///     更新会员等级
        /// </summary>
        public void WordUpdateLevel()
        {
            try
            {
                this.UpdateLevel().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("呼叫中心：Error:  {0}".FormatWith(e.Message), e);
            }
        }

        #endregion 更新会员等级调用

        #region 修改用户状态
        public void WordUpdataUserStatus()
        {
            try
            {
                this.UpdateUserStatus().Wait();
            }
            catch (Exception e)
            {
                LogHelper.LogError("呼叫中心：Error:  {0}".FormatWith(e.Message), e);
            }
        }
        #endregion

        #endregion

        #region 常量/变量

        private static readonly string authConnectionString;

        /// <summary>
        ///     前台数据库链接字符串
        /// </summary>
        private static readonly string bizConnectionString;

        /// <summary>
        ///     呼叫中心主地址
        /// </summary>
        private static readonly string callCenterApiHost;

        private static readonly string callCenterDays;
        private static readonly string maxDegreeOfParallelism;
        private static readonly string memberConnectionString;
        

        private static readonly string jymOperContextConnection;
        private readonly Lazy<HttpClient> callCenterClient;
        private HttpClient CallCenterClient => this.callCenterClient.Value;

        #endregion 常量/变量

        #region 构造函数

        static CallCenterService()
        {
            callCenterApiHost = CloudConfigurationManager.GetSetting("CallCenterApiHost");
            bizConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
            authConnectionString = CloudConfigurationManager.GetSetting("JYMDBAuthContextConnectionString");
            memberConnectionString = CloudConfigurationManager.GetSetting("JYMDBMemberContextConnectionString");
            
            jymOperContextConnection = CloudConfigurationManager.GetSetting("JymOperContextConnection");
            maxDegreeOfParallelism = CloudConfigurationManager.GetSetting("MaxDegreeOfParallelism");
            callCenterDays = CloudConfigurationManager.GetSetting("CallCenterDays");
        }

        public CallCenterService()
        {
            this.callCenterClient = new Lazy<HttpClient>(this.InitCallCenterHttpClient);
        }

        #endregion 构造函数

        #region HttpClient/SqlConnection

        /// <summary>
        ///     创建连接
        /// </summary>
        /// <returns></returns>
        private static SqlConnection GetOpenConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        private HttpClient InitCallCenterHttpClient()
        {
            HttpClient httpClient = HttpClientHelper.InitHttpClient(callCenterApiHost);
            return httpClient;
        }

        #endregion HttpClient/SqlConnection
    }
}
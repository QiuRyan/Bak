// ***********************************************************************
// Project          : MessageManager
// File             : ApiHandler.cs
// Created          : 2015-12-07  14:27
//
// Last Modified By : 陈兆斌(chen.zhaobin@jinyinmao.com.cn)
// Last Modified On : 2015-12-08  12:50
// ***********************************************************************
// <copyright file="ApiHandler.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.IO;
using System.Net;
using System.Text;

namespace Jinyinmao.FrameWork.Commons
{
    public class ApiHandler
    {
        public static string GetListFromApi(string postDataStr, string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream?.Close();
            return retString;
        }

        #region get调用web Api 方法

        public static string GetListFromApi(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream?.Close();
            return retString;
        }

        #endregion get调用web Api 方法

        /// <summary>
        ///     post api
        /// </summary>
        /// <param name="postDataStr">条件字符串</param>
        /// <param name="url">请求url</param>
        /// <returns></returns>
        public static string PostListFromApi(string postDataStr, string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/text";
                request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream?.Close();
                return retString;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static bool RequestWebPage(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                sr.Dispose();
                sr.Close();
                if (s != null)
                {
                    s.Dispose();
                    s.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //// 从一个Json串生成对象信息
        //public static object JsonToObject(string jsonString, object obj)
        //{
        //    var serializer = new DataContractJsonSerializer(obj.GetType());
        //    var mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        //    return serializer.ReadObject(mStream);
        //}

        //// 从一个对象信息生成Json串
        //public static string ObjectToJson(object obj)
        //{
        //    var serializer = new DataContractJsonSerializer(obj.GetType());
        //    var stream = new MemoryStream();
        //    serializer.WriteObject(stream, obj);
        //    var dataBytes = new byte[stream.Length];
        //    stream.Position = 0;
        //    stream.Read(dataBytes, 0, (int)stream.Length);
        //    return Encoding.UTF8.GetString(dataBytes);
        //}

        #region Post调用web Api 方法

        //x-www-form-urlencoded
        /// <summary>
        ///     Post调用web Api 方法
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="postData">The post data.</param>
        /// <returns>System.String.</returns>
        public static string HttpPostConnectToServer(string serverUrl, string postData, string authHeader)
        {
            byte[] dataArray = Encoding.UTF8.GetBytes(postData);
            //创建请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverUrl);
            request.Method = "POST";
            request.ContentLength = dataArray.Length;
            //设置上传服务的数据格式
            request.ContentType = "application/json";
            //请求的身份验证信息为默认
            request.Credentials = CredentialCache.DefaultCredentials;
            //请求超时时间
            request.Timeout = 2000000;
            request.Headers.Add("X-JYM-MSM", authHeader);
            //创建输入流
            Stream dataStream;
            //using (var dataStream = request.GetRequestStream())
            //{
            //    dataStream.Write(dataArray, 0, dataArray.Length);
            //    dataStream.Close();
            //}
            try
            {
                dataStream = request.GetRequestStream();
            }
            catch (Exception)
            {
                return null; //连接服务器失败
            }
            //发送请求
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();
            //读取返回消息
            string res;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                //var result = new ServerResult();
                return "{\"error\":\"connectToServer\",\"error_description\":\"" + ex.Message + "\"}"; //连接服务器失败
            }
            return res;
        }

        public static string HttpPostConnectToServer(string serverUrl, string postData)
        {
            byte[] dataArray = Encoding.UTF8.GetBytes(postData);
            //创建请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverUrl);
            request.Method = "POST";
            request.ContentLength = dataArray.Length;
            //设置上传服务的数据格式
            request.ContentType = "application/json";
            //请求的身份验证信息为默认
            request.Credentials = CredentialCache.DefaultCredentials;
            //请求超时时间
            request.Timeout = 2000000;
            //创建输入流
            Stream dataStream;
            //using (var dataStream = request.GetRequestStream())
            //{
            //    dataStream.Write(dataArray, 0, dataArray.Length);
            //    dataStream.Close();
            //}
            try
            {
                dataStream = request.GetRequestStream();
            }
            catch (Exception)
            {
                return null; //连接服务器失败
            }
            //发送请求
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();
            //读取返回消息
            string res;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                //var result = new ServerResult();
                return "{\"error\":\"connectToServer\",\"error_description\":\"" + ex.Message + "\"}"; //连接服务器失败
            }
            return res;
        }

        #endregion Post调用web Api 方法
    }
}
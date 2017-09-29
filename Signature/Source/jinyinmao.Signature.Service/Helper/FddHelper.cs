using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;

namespace jinyinmao.Signature.Service
{
    public class FddHelper
    {
        #region //构造方法

        public FddHelper()
        {
            this.bytesArray = new ArrayList();
            string flag = DateTime.Now.Ticks.ToString("x");
            this.boundary = "---------------------------" + flag;
        }

        #endregion //构造方法

        #region //字段

        private readonly string boundary;
        private readonly ArrayList bytesArray;
        private readonly Encoding encoding = Encoding.UTF8;

        #endregion //字段

        #region //方法

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt3Des(string strString, string key)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform desDecrypt = DES.CreateDecryptor();

            byte[] buffer = Convert.FromBase64String(strString);
            return ToHexString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt3Des(string strString, string key)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform desEncrypt = DES.CreateEncryptor();

            byte[] buffer = Encoding.UTF8.GetBytes(strString);

            return ToHexString(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public static void GetByWebRequest(string url)
        {
            WebClient client = new WebClient();

            client.DownloadFile(url, "d:/p.pdf");
        }

        /// <summary>
        /// md5
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Md5(string value)
        {
#pragma warning disable 618
            return FormsAuthentication.HashPasswordForStoringInConfigFile(value, FormsAuthPasswordFormat.MD5.ToString());
#pragma warning restore 618
        }

        /// <summary>
        /// 普通的Post请求，只有参数没有上传文件流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string PostByWebRequest(string url, List<KeyValuePair<string, string>> parameters)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.KeepAlive = true;

                List<string> items = new List<string>(parameters.Count);
                items.AddRange(parameters.Select(item => string.Concat(item.Key, "=", item.Value)));
                string postData = string.Join("&", items.ToArray());

                byte[] postByte = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = postByte.Length;
                request.GetRequestStream().Write(postByte, 0, postByte.Length);
                HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                if (!request.HaveResponse)
                {
                    return "";
                }

                StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);
                string str = sr.ReadToEnd();
                sr.Close();
                httpResponse.Close();
                return str;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// sha1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Sha1(string value)
        {
#pragma warning disable 618
            return FormsAuthentication.HashPasswordForStoringInConfigFile(value, FormsAuthPasswordFormat.SHA1.ToString());
#pragma warning restore 618
        }

        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;

            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                foreach (byte t in bytes)
                {
                    strB.Append(t.ToString("X2"));
                }

                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="requestUrl">请求url</param>
        /// <param name="responseText">响应</param>
        /// <returns></returns>
        public bool HttpPost(string requestUrl, out string responseText)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "multipart/form-data; boundary=" + this.boundary);

            byte[] responseBytes;
            byte[] bytes = this.MergeContent();

            try
            {
                responseBytes = webClient.UploadData(requestUrl, bytes);
                responseText = Encoding.UTF8.GetString(responseBytes);
                return true;
            }
            catch (WebException ex)
            {
                Stream responseStream = ex.Response.GetResponseStream();
                responseBytes = new byte[ex.Response.ContentLength];
                responseStream?.Read(responseBytes, 0, responseBytes.Length);
            }
            responseText = Encoding.UTF8.GetString(responseBytes);
            return false;
        }

        /// <summary>
        /// 设置表单数据字段
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <returns></returns>
        public void SetFieldValue(string fieldName, string fieldValue)
        {
            string httpRow = "--" + this.boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
            string httpRowData = string.Format(httpRow, fieldName, fieldValue);

            this.bytesArray.Add(this.encoding.GetBytes(httpRowData));
        }

        /// <summary>
        /// 设置表单文件数据
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="filename">字段值</param>
        /// <param name="contentType">内容内型</param>
        /// <param name="fileBytes">文件字节流</param>
        /// <returns></returns>
        public void SetFieldValue(string fieldName, string filename, string contentType, byte[] fileBytes)
        {
            string end = "\r\n";
            string httpRow = "--" + this.boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string httpRowData = string.Format(httpRow, fieldName, filename, contentType);

            byte[] headerBytes = this.encoding.GetBytes(httpRowData);
            byte[] endBytes = this.encoding.GetBytes(end);
            byte[] fileDataBytes = new byte[headerBytes.Length + fileBytes.Length + endBytes.Length];

            headerBytes.CopyTo(fileDataBytes, 0);
            fileBytes.CopyTo(fileDataBytes, headerBytes.Length);
            endBytes.CopyTo(fileDataBytes, headerBytes.Length + fileBytes.Length);

            this.bytesArray.Add(fileDataBytes);
        }

        /// <summary>
        /// 合并请求数据
        /// </summary>
        /// <returns></returns>
        private byte[] MergeContent()
        {
            int readLength = 0;
            string endBoundary = "--" + this.boundary + "--\r\n";
            byte[] endBoundaryBytes = this.encoding.GetBytes(endBoundary);

            this.bytesArray.Add(endBoundaryBytes);

            int length = this.bytesArray.Cast<byte[]>().Sum(b => b.Length);

            byte[] bytes = new byte[length];

            foreach (byte[] b in this.bytesArray)
            {
                b.CopyTo(bytes, readLength);
                readLength += b.Length;
            }

            return bytes;
        }

        #endregion //方法
    }
}
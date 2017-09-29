using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Moe.Lib;

namespace jinyinmao.Signature.lib
{
    public static class Utility
    {
        private static readonly char[] Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private static readonly object LockObject = new object();

        /// <summary>
        ///     混淆手机号码
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>System.String.</returns>
        public static string ConfusionCellphone(this string cellphone)
        {
            return new Regex("(\\d{3})(\\d{4})(\\d{4})", RegexOptions.None).Replace(cellphone, "$1****$3");
        }

        /// <summary>
        ///     混淆身份证号
        /// </summary>
        /// <param name="credentialNo">The credential no.</param>
        /// <returns>System.String.</returns>
        public static string ConfusionCredentialNo(this string credentialNo)
        {
            return credentialNo.Length < 15 ? new Regex("(\\d{" + (credentialNo.Length - 4) + "})(\\d{4})", RegexOptions.None).Replace(credentialNo, "$1" + "".PadLeft(4, '*')) : new Regex("(\\d{4})(\\d{" + (credentialNo.Length - 8) + "})(\\d{4})", RegexOptions.None).Replace(credentialNo, "$1" + "".PadLeft(credentialNo.Length - 8, '*') + "$3");
        }

        /// <summary>
        ///     生成14位长度的随机数
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GenerateNo()
        {
            DateTime currentTime;
            lock (LockObject)
            {
                currentTime = DateTime.UtcNow.ToChinaStandardTime();
            }

            int year = currentTime.Year - 2013;
            if (year < 0)
            {
                year = 0;
            }
            int month = currentTime.Month;
            int day = currentTime.Day;
            int hour = currentTime.Hour;

            string yearChar = Characters[year].ToString(CultureInfo.InvariantCulture);
            string monthChar = Characters[month].ToString(CultureInfo.InvariantCulture);
            string dayChar = Characters[day].ToString(CultureInfo.InvariantCulture);
            string hourChar = Characters[hour].ToString(CultureInfo.InvariantCulture);

            string time = currentTime.ToString("mmssffffff");

            StringBuilder sb = new StringBuilder();
            sb.Append(yearChar).Append(monthChar).Append(dayChar).Append(hourChar).Append(time);
            return sb.ToString().ToUpperInvariant();
        }

        /// <summary>
        ///     Shes the a1 hash string.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>System.String.</returns>
        public static string SHA1HashString(string content)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_in = Encoding.UTF8.GetBytes(content);
            byte[] bytes_out = sha1.ComputeHash(bytes_in);
            sha1.Dispose();
            string result = BitConverter.ToString(bytes_out);
            result = result.Replace("-", "");
            return result;
        }

        /// <summary>
        ///     Shes the a256 hash string.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>System.String.</returns>
        public static string SHA256HashString(string payload, string salt)
        {
            string stringToHash = payload + salt;
            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(stringToHash.GetBytesOfUTF8());
            StringBuilder hashString = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                hashString.Append(b.ToString("x2"));
            }
            return hashString.ToString();
        }
    }
}
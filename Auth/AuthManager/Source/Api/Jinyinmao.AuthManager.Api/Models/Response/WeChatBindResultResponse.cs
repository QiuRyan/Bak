using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class WeChatBindResultResponse.
    /// </summary>
    public class WeChatBindResultResponse
    {
        /// <summary>
        ///     访问令牌
        /// </summary>
        [Required]
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        ///     用户的手机号码
        /// </summary>
        /// <value>The cellphone.</value>
        [Required]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }
    }

    internal static class WeChatBindResultResponseEx
    {
        internal static WeChatBindResultResponse ToBindResponse(this UserInfo info, string token)
        {
            return new WeChatBindResultResponse
            {
                AccessToken = token,
                Cellphone = info.Cellphone
            };
        }
    }
}
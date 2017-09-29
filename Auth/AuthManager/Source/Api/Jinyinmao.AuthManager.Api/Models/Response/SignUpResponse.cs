using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Moe.Lib;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jinyinmao.AuthManager.Api.Models.Response
{
    /// <summary>
    ///     Class SignUpResponse.
    /// </summary>
    public class SignUpResponse
    {
        /// <summary>
        ///     用户的手机号码
        /// </summary>
        [Required]
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [Required]
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }

    /// <summary>
    ///     Class UserInfoEx.
    /// </summary>
    internal static class UserInfoEx
    {
        internal static SignUpResponse ToSignUpResponse(this UserInfo info)
        {
            return new SignUpResponse
            {
                Cellphone = info.Cellphone,
                UserIdentifier = info.UserId.ToGuidString()
            };
        }
    }
}
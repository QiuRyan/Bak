// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : IUser.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-26  10:52
// ***********************************************************************
// <copyright file="IUser.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Jinyinmao.AuthManager.Domain.Interface.Commands;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Orleans;
using System.Threading.Tasks;

namespace Jinyinmao.AuthManager.Domain.Interface
{
    public interface IUser : IGrainWithGuidKey
    {
        Task<UserInfo> AdminCancelAccountAsync(AdminCancelAccount command);

        Task BindWeChatAsync(string openId);

        Task<UserInfo> ChangeLoginCellphoneAsync(ChangeLoginCellphone command);

        Task<CheckPasswordResult> CheckPasswordAsync(string loginName, string password);

        Task<bool> CheckPasswordAsync(string password);

        Task DumpUserToDBAsync();

        Task DumpUserToMemoryAsync();

        Task<UserInfo> GetUserInfoAsync();

        Task<BindInfo> GetWeChatBindInfoById();

        Task<UserInfo> LockAsync();

        Task<UserInfo> RegisterAsync(UserRegister command);

        Task ReloadAsync();

        Task<UserInfo> ResetCellphoneAsync(ResetCellphone command);

        Task<UserInfo> ResetLoginPasswordAsync(ResetLoginPassword command);

        Task<UserInfo> SetLoginPasswordAsync(SetLoginPassword command);

        Task SyncUserAsync();

        Task UnbindWeChatAsync();

        Task<UserInfo> UnlockAsync();
    }
}
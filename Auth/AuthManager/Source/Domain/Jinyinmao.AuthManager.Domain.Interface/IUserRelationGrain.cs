// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : IUserRelationGrain.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-02-16  13:59
// ***********************************************************************
// <copyright file="IUserRelationGrain.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Orleans;

namespace Jinyinmao.AuthManager.Domain.Interface
{
    public interface IUserRelationGrain : IGrainWithStringKey
    {
        Task BindCellphoneAsync(string userIdentifier);

        Task BindWeChatAsync(string userIdentifier);

        Task<CheckCellphoneResult> CheckCellphoneAsync();

        Task<string> DumpUserRelationToDBAsync();

        Task DumpUserRelationToMemoryAsync();

        Task<string> GetUserIdentifierAsync();

        Task ReloadAsync();

        Task UnregisterAsync();
    }
}
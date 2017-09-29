// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : UserRelationGrain.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-02-16  14:03
// ***********************************************************************
// <copyright file="UserRelationGrain.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Jinyinmao.AuthManager.Domain.Core.SQLDB;
using Jinyinmao.AuthManager.Domain.Core.SQLDB.Model;
using Jinyinmao.AuthManager.Domain.Interface;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Jinyinmao.AuthManager.Libraries.Extension;
using Moe.Lib;
using Orleans;

namespace Jinyinmao.AuthManager.Domain
{
    public class UserRelationGrainGrain : EntityGrain, IUserRelationGrain
    {
        public int AccountType { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsAlive { get; set; }

        public string LoginName { get; set; }

        public string UserIdentifier { get; set; }

        #region IUserRelationGrain Members

        public async Task BindCellphoneAsync(string userIdentifier)
        {
            if (this.IsAlive)
            {
                throw new Exception("用户已注册");
            }

            this.AccountType = Libraries.Parameter.AccountType.Cellphone.Code;
            this.CreateTime = DateTime.UtcNow.ToChinaStandardTime();
            this.IsAlive = true;
            this.LoginName = this.GetPrimaryKeyString();
            this.LastModified = DateTime.UtcNow.ToChinaStandardTime();
            this.UserIdentifier = userIdentifier;

            await this.SaveChangeAsync();
        }

        public async Task BindWeChatAsync(string userIdentifier)
        {
            if (this.IsAlive)
            {
                throw new Exception("用户已绑定");
            }

            this.AccountType = Libraries.Parameter.AccountType.WeChat.Code;
            this.CreateTime = DateTime.UtcNow.ToChinaStandardTime();
            this.IsAlive = true;
            this.LoginName = this.GetPrimaryKeyString();
            this.LastModified = DateTime.UtcNow.ToChinaStandardTime();
            this.UserIdentifier = userIdentifier;

            await this.SaveChangeAsync();
        }

        public Task<CheckCellphoneResult> CheckCellphoneAsync()
        {
            return Task.FromResult(new CheckCellphoneResult
            {
                Result = this.LoginName.IsNotNullOrEmpty() && this.IsAlive && this.AccountType == Libraries.Parameter.AccountType.Cellphone.Code,
                UserIdentifier = this.UserIdentifier
            });
        }

        public async Task<string> DumpUserRelationToDBAsync()
        {
            if (this.UserIdentifier.IsNullOrEmpty())
            {
                return string.Empty;
            }

            using (AuthContext db = new AuthContext())
            {
                int weChatCode = Libraries.Parameter.AccountType.WeChat.Code;
                int cellphoneCode = Libraries.Parameter.AccountType.Cellphone.Code;
                string cellphone = this.GetPrimaryKeyString();

                UserRelation result = await db.UserRelation.FirstOrDefaultAsync(u => u.IsAlive && u.LoginName == cellphone && (u.AccountType == weChatCode || u.AccountType == cellphoneCode));
                if (result == null)
                {
                    db.UserRelation.Add(await this.BuildUserRelationAsync());
                }
                else
                {
                    result.AccountType = this.AccountType;
                    result.CreateTime = this.CreateTime;
                    result.IsAlive = this.IsAlive;
                    result.LoginName = this.LoginName;
                    result.LastModified = DateTime.UtcNow.ToChinaStandardTime();
                    result.UserIdentifier = this.UserIdentifier.ToUpper();
                }

                await db.SaveChangesAsync();
            }

            await this.ReloadAsync();

            return this.UserIdentifier;
        }

        public async Task DumpUserRelationToMemoryAsync()
        {
            await this.ReloadAsync();
        }

        public Task<string> GetUserIdentifierAsync()
        {
            return Task.FromResult(this.AccountType == Libraries.Parameter.AccountType.Cellphone.Code ? this.UserIdentifier : string.Empty);
        }

        public async Task ReloadAsync()
        {
            UserRelation relation = await this.GetUserRelationAsync();

            if (relation != null)
            {
                this.AccountType = relation.AccountType;
                this.CreateTime = relation.CreateTime;
                this.IsAlive = relation.IsAlive;
                this.LastModified = relation.LastModified.GetValueOrDefault();
                this.LoginName = relation.LoginName;
                this.UserIdentifier = relation.UserIdentifier.ToUpper();
            }
        }

        public async Task UnregisterAsync()
        {
            this.IsAlive = false;
            await this.SaveChangeAsync();
        }

        #endregion IUserRelationGrain Members

        public override async Task OnActivateAsync()
        {
            await this.ReloadAsync();
            await base.OnActivateAsync();
        }

        protected override async Task SaveChangeAsync()
        {
            int weChatCode = Libraries.Parameter.AccountType.WeChat.Code;
            int cellphoneCode = Libraries.Parameter.AccountType.Cellphone.Code;
            string cellphone = this.GetPrimaryKeyString();

            if (cellphone.IsNullOrEmpty() || this.UserIdentifier.IsNullOrEmpty())
            {
                return;
            }

            using (AuthContext db = new AuthContext())
            {
                UserRelation userRelation = await db.UserRelation.FirstOrDefaultAsync(u => u.IsAlive && u.LoginName == cellphone && (u.AccountType == weChatCode || u.AccountType == cellphoneCode));

                if (userRelation != null)
                {
                    userRelation.IsAlive = this.IsAlive;
                    userRelation.LastModified = DateTime.UtcNow.ToChinaStandardTime();
                }
                else
                {
                    UserRelation relation = new UserRelation
                    {
                        AccountType = this.AccountType,
                        CreateTime = this.CreateTime,
                        IsAlive = this.IsAlive,
                        LoginName = this.LoginName,
                        LastModified = DateTime.UtcNow.ToChinaStandardTime(),
                        UserIdentifier = this.UserIdentifier.ToUpper()
                    };

                    db.UserRelation.Add(relation);
                }

                await db.ExecuteSaveChangesAsync();
            }
        }

        private async Task<UserRelation> BuildUserRelationAsync()
        {
            return await Task.FromResult(new UserRelation
            {
                AccountType = this.AccountType,
                CreateTime = this.CreateTime,
                IsAlive = this.IsAlive,
                LoginName = this.LoginName,
                LastModified = DateTime.UtcNow.ToChinaStandardTime(),
                UserIdentifier = this.UserIdentifier.ToUpper()
            });
        }

        private async Task<UserRelation> GetUserRelationAsync()
        {
            int weChatCode = Libraries.Parameter.AccountType.WeChat.Code;
            int cellphoneCode = Libraries.Parameter.AccountType.Cellphone.Code;
            UserRelation relation = null;

            using (AuthContext db = new AuthContext())
            {
                string cellphone = this.GetPrimaryKeyString();
                UserRelation userRelation = await db.UserRelation.FirstOrDefaultAsync(u => u.IsAlive && u.LoginName == cellphone && (u.AccountType == weChatCode || u.AccountType == cellphoneCode));

                if (userRelation != null)
                    relation = userRelation.ToMapper<UserRelation, UserRelation>();
            }

            return relation;
        }
    }
}
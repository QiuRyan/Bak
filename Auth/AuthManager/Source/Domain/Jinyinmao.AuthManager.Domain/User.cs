// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : User.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-02-14  14:43
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Jinyinmao.AuthManager.Domain.Core.SQLDB;
using Jinyinmao.AuthManager.Domain.Core.SQLDB.Model;
using Jinyinmao.AuthManager.Domain.Interface;
using Jinyinmao.AuthManager.Domain.Interface.Commands;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Jinyinmao.AuthManager.Libraries;
using Jinyinmao.AuthManager.Libraries.Helper;
using Jinyinmao.AuthManager.Libraries.Parameter;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using MoeLib.Jinyinmao.Orleans;
using Orleans;

namespace Jinyinmao.AuthManager.Domain
{
    public partial class User : EntityGrain, IUser
    {
        private string AuthorizationKey
        {
            get { return App.Configurations.GetConfig<AuthSiloConfig>().AuthorizationKey; }
        }

        private string EndpointUrl
        {
            get { return App.Configurations.GetConfig<AuthSiloConfig>().EndpointUrl; }
        }

        #region IUser Members

        public async Task<UserInfo> AdminCancelAccountAsync(AdminCancelAccount command)
        {
            if (this.Cellphone.IsNullOrEmpty())
            {
                this.Logger.Error("User does not exist,PrimaryKey:{0}".FormatWith(this.GetPrimaryKey()), this);
                return null;
            }

            if (this.Cellphone.Contains("X"))
            {
                return await this.GetUserInfoAsync();
            }

            IUserRelationGrain userRelationGrain = this.GrainFactory.GetGrain<IUserRelationGrain>(this.Cellphone);
            await userRelationGrain.UnregisterAsync();

            command.Cellphone = this.Cellphone;
            this.Cellphone = "X" + this.Cellphone;
            this.LoginNames.RemoveAll(n => n == command.Cellphone);
            this.LoginNames.Add(this.Cellphone);
            this.LastModified = DateTime.UtcNow.ToChinaStandardTime();

            await this.SaveChangeAsync();

            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                { "Operation", "人工注销手机号码" }
            };
            await this.SaveChangeLogsAsync(dic, command.Cellphone);

            return await this.GetUserInfoAsync();
        }

        public async Task BindWeChatAsync(string openId)
        {
            this.OpenId = openId;
            this.Flag = true;
            IUserRelationGrain grain = this.GrainFactory.GetGrain<IUserRelationGrain>(openId);
            await grain.BindWeChatAsync(this.GetPrimaryKey().ToGuidString());
        }

        public async Task<UserInfo> ChangeLoginCellphoneAsync(ChangeLoginCellphone command)
        {
            if (this.Cellphone.IsNullOrEmpty() || this.Cellphone != command.LoginCellphone)
            {
                this.Error("手机号码不正确。Command: " + command.ToJson());
            }

            this.LoginNames = new List<string> { command.NewCellphone };

            if (this.LoginNames == null)
            {
                this.LoginNames = new List<string> { command.NewCellphone };
            }
            else
            {
                this.LoginNames.RemoveAll(n => n == command.LoginCellphone || n == command.NewCellphone);
                this.LoginNames.Add(command.NewCellphone);
            }

            //await this.RaiseUserChangedLoginCellphoneEventAsync(command);
            return await this.GetUserInfoAsync();
        }

        public async Task<CheckPasswordResult> CheckPasswordAsync(string loginName, string password)
        {
            if (this.Cellphone.IsNullOrEmpty())
            {
                return await Task.FromResult(await this.BuildCheckPasswordResultAsync(this.Cellphone, 1, false, false));
            }

            if (this.PasswordErrorCount > 10)
            {
                return await Task.FromResult(await this.BuildCheckPasswordResultAsync(this.Cellphone, -1, false, true));
            }

            if (this.Cellphone == loginName || this.LoginNames.Contains(loginName))
            {
                if (CryptographyHelper.Check(password, this.Salt, this.EncryptedPassword))
                {
                    this.PasswordErrorCount = 0;
                    return await Task.FromResult(await this.BuildCheckPasswordResultAsync(this.Cellphone, 10 - this.PasswordErrorCount, true, true));
                }
            }

            this.PasswordErrorCount += 1;
            return await Task.FromResult(await this.BuildCheckPasswordResultAsync(this.Cellphone, 10 - this.PasswordErrorCount, false, true));
        }

        public async Task<bool> CheckPasswordAsync(string password)
        {
            return await Task.FromResult(CryptographyHelper.Check(password, this.Salt, this.EncryptedPassword));
        }

        public async Task DumpUserToDBAsync()
        {
            await this.SyncUserAsync();
        }

        public async Task DumpUserToMemoryAsync()
        {
            await this.ReloadAsync();
        }

        public async Task<UserInfo> GetUserInfoAsync()
        {
            if (this.Cellphone.IsNullOrEmpty())
            {
                this.Error("User does not exist,PrimaryKey:{0}".FormatWith(this.GetPrimaryKey()));
                return await Task.FromResult<UserInfo>(null);
            }

            return await this.BuildUserInfoAsync();
        }

        public async Task<BindInfo> GetWeChatBindInfoById()
        {
            return await Task.FromResult(new BindInfo
            {
                Flag = this.Flag ? BindFlag.Yes.Code : BindFlag.No.Code,
                UserIdentifier = this.UserId.ToGuidString(),
                OpenId = this.OpenId
            });
        }

        public async Task<UserInfo> LockAsync()
        {
            this.Closed = true;
            this.LastModified = DateTime.UtcNow.ToChinaStandardTime();

            await this.SaveChangeAsync();
            return await this.GetUserInfoAsync();
        }

        public async Task<UserInfo> RegisterAsync(UserRegister command)
        {
            if (this.UserId == command.UserId)
            {
                this.Error("User exist, cellphone: {0}".FormatWith(command.Cellphone));
                return null;
            }

            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            this.UserId = command.UserId;
            this.Cellphone = command.Cellphone;
            this.ClientType = command.ClientType;
            this.Closed = false;
            this.ContractId = command.ContractId;
            this.Info = command.Info ?? await this.CreateDic();
            this.InviteBy = command.InviteBy;
            this.InviteFor = await this.GetInviteFor(command.Cellphone);
            this.LoginNames = new List<string> { command.Cellphone };
            this.OutletCode = command.OutletCode;
            this.RegisterTime = now;
            this.Salt = command.UserId.ToGuidString();
            this.EncryptedPassword = CryptographyHelper.Encrypting(command.Password, command.UserId.ToGuidString());
            this.LastModified = now;

            IUserRelationGrain relationGrain = this.GrainFactory.GetGrain<IUserRelationGrain>(command.Cellphone);
            await relationGrain.BindCellphoneAsync(command.UserId.ToGuidString());
            await this.SaveChangeAsync();
            //await this.RaiseUserRegisteredEventAsync(command);
            return await this.GetUserInfoAsync();
        }

        public async Task ReloadAsync()
        {
            try
            {
                string userIdentifier = this.GetPrimaryKey().ToGuidString();

                using (AuthContext db = new AuthContext())
                {
                    DBUser userModel = await db.User.FirstOrDefaultAsync(p => p.UserIdentifier == userIdentifier);
                    if (userModel != null)
                    {
                        await this.SetMemoryDataFromUserModelAsync(userModel);
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = ex.EntityValidationErrors.ToJson();
                this.Error(errorMessage, "Grains Biz database operation failed.", 0UL, "", null, ex);
                throw;
            }
            catch (Exception e)
            {
                this.Error(e.Message, "Grains Biz database operation failed.", 0UL, "", null, e);
                throw;
            }
        }

        public async Task<UserInfo> ResetCellphoneAsync(ResetCellphone command)
        {
            if (this.Cellphone.IsNullOrEmpty())
            {
                this.Logger.Error("User does not exist,PrimaryKey:{0}".FormatWith(this.GetPrimaryKey()), this);
                return null;
            }

            string oldCellphone = this.Cellphone;
            this.Cellphone = command.NewCellphone;
            this.LastModified = DateTime.UtcNow.ToChinaStandardTime();

            this.LoginNames.RemoveAll(n => n == oldCellphone || n == command.NewCellphone);
            this.LoginNames.Add(command.NewCellphone);

            IUserRelationGrain oldRelationGrain = this.GrainFactory.GetGrain<IUserRelationGrain>(oldCellphone);
            await oldRelationGrain.UnregisterAsync();

            IUserRelationGrain newNelationGrain = this.GrainFactory.GetGrain<IUserRelationGrain>(command.NewCellphone);
            await newNelationGrain.BindCellphoneAsync(this.GetPrimaryKey().ToGuidString());
            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                { "Operation", command.Messager }
            };

            await this.SaveChangeAsync();
            await this.SaveChangeLogsAsync(dic, oldCellphone);

            return await this.GetUserInfoAsync();
        }

        public async Task<UserInfo> ResetLoginPasswordAsync(ResetLoginPassword command)
        {
            if (this.Cellphone.IsNullOrEmpty())
            {
                this.Logger.Error("User does not exist,PrimaryKey:{0}".FormatWith(this.GetPrimaryKey()), this);
                return null;
            }

            this.Salt = command.Salt;
            this.EncryptedPassword = CryptographyHelper.Encrypting(command.Password, command.Salt);
            this.PasswordErrorCount = 0;
            this.LastModified = DateTime.UtcNow.ToChinaStandardTime();

            await this.SaveChangeAsync();
            //await this.RaiseUserResetLoginPasswordEventAsync(command);
            return await this.GetUserInfoAsync();
        }

        public async Task<UserInfo> SetLoginPasswordAsync(SetLoginPassword command)
        {
            if (this.Cellphone.IsNullOrEmpty())
            {
                this.Error("User does not exist, PrimaryKey: {0}".FormatWith(this.GetPrimaryKey()));
                return null;
            }

            this.EncryptedPassword = CryptographyHelper.Encrypting(command.Password, command.Salt);
            this.PasswordErrorCount = 0;
            this.LastModified = DateTime.UtcNow.ToChinaStandardTime();
            await this.SaveChangeAsync();
            //await this.RaiseUserSetLoginPasswordEventAsync(command);
            return await this.GetUserInfoAsync();
        }

        public async Task SyncUserAsync()
        {
            await this.SaveChangeAsync();
        }

        public async Task UnbindWeChatAsync()
        {
            IUserRelationGrain grain = this.GrainFactory.GetGrain<IUserRelationGrain>(this.OpenId);
            await grain.UnregisterAsync();
            this.OpenId = string.Empty;
            this.Flag = false;
        }

        public async Task<UserInfo> UnlockAsync()
        {
            this.Closed = false;
            this.LastModified = DateTime.UtcNow.ToChinaStandardTime();
            await this.SaveChangeAsync();
            return await this.GetUserInfoAsync();
        }

        #endregion IUser Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override async Task OnActivateAsync()
        {
            await this.ReloadAsync();
            await this.ReloadBindAsync();

            await base.OnActivateAsync();
        }

        public async Task ReloadBindAsync()
        {
            int code = AccountType.WeChat.Code;
            string userIdentifier = this.UserId.ToGuidString();

            using (AuthContext db = new AuthContext())
            {
                UserRelation userRelation = await db.UserRelation.FirstOrDefaultAsync(u => u.IsAlive && u.UserIdentifier == userIdentifier && u.AccountType == code);
                this.Flag = userRelation != null;
                this.OpenId = userRelation != null ? userRelation.LoginName : string.Empty;
            }
        }

        protected override async Task SaveChangeAsync()
        {
            if (this.Cellphone.IsNullOrEmpty())
            {
                return;
            }

            using (AuthContext db = new AuthContext())
            {
                string userIdentifier = this.GetPrimaryKey().ToGuidString();
                try
                {
                    DBUser user = new DBUser
                    {
                        Cellphone = this.Cellphone,
                        ClientType = this.ClientType,
                        Closed = this.Closed,
                        ContractId = this.ContractId,
                        EncryptedPassword = this.EncryptedPassword,
                        Info = (this.Info ?? await this.CreateDic()).ToJson(),
                        InviteBy = this.InviteBy,
                        InviteFor = this.InviteFor,
                        LastModified = DateTime.UtcNow.ToChinaStandardTime(),
                        LoginNames = this.LoginNames.ToJson(),
                        OutletCode = this.OutletCode,
                        RegisterTime = this.RegisterTime,
                        Salt = this.Salt,
                        UserIdentifier = this.UserId.ToGuidString()
                    };
                    await db.SaveOrUpdateAsync(user, t => t.UserIdentifier == userIdentifier);
                }
                catch (DbEntityValidationException ex)
                {
                    string errorMessage = ex.EntityValidationErrors.ToJson();
                    this.Error(errorMessage, "Grains Biz database operation failed.", 0UL, "", null, ex);
                    throw;
                }
                catch (Exception e)
                {
                    this.Error(e.Message, "Grains Biz database operation failed.", 0UL, "", null, e);
                    throw;
                }
            }
        }

        protected async Task SaveChangeLogsAsync(Dictionary<string, string> dic, string oldCellphone)
        {
            string changeIdentifier = GuidUtility.NewSequentialGuid().ToGuidString();

            using (AuthContext db = new AuthContext())
            {
                db.ChangeLog.Add(await this.BuildChangeLogAsync(dic, oldCellphone, changeIdentifier));

                await db.SaveChangesAsync();
            }
        }

        private async Task<ChangeLog> BuildChangeLogAsync(Dictionary<string, string> dic, string oldCellphone, string changeIdentifier)
        {
            return await Task.FromResult(new ChangeLog
            {
                ChangeIdentifier = changeIdentifier,
                CreateTime = DateTime.UtcNow.ToChinaStandardTime(),
                Info = dic.ToJson(),
                NewCellphone = this.Cellphone.Contains("X") ? oldCellphone : this.Cellphone,
                OldCellphone = oldCellphone,
                UserIdentifier = this.UserId.ToGuidString()
            });
        }

        private async Task<CheckPasswordResult> BuildCheckPasswordResultAsync(string cellphone, int remainCount, bool success, bool userExist)
        {
            return await Task.FromResult(new CheckPasswordResult
            {
                Cellphone = cellphone,
                RemainCount = remainCount,
                Success = success,
                UserExist = userExist,
                UserId = this.UserId
            });
        }

        private async Task<UserInfo> BuildUserInfoAsync()
        {
            return await Task.FromResult(new UserInfo
            {
                Cellphone = this.Cellphone,
                Closed = this.Closed,
                ContractId = this.ContractId,
                HasSetPassword = this.HasSetPassword,
                Info = this.Info,
                InviteBy = this.InviteBy,
                InviteFor = this.InviteFor,
                LastModified = this.LastModified,
                LoginNames = this.LoginNames,
                OutletCode = this.OutletCode,
                PasswordErrorCount = this.PasswordErrorCount,
                RegisterTime = this.RegisterTime,
                UserId = this.UserId
            });
        }

        private async Task<DBUser> BuildUserModelAsync()
        {
            return await Task.FromResult(new DBUser
            {
                Cellphone = this.Cellphone,
                ClientType = this.ClientType,
                Closed = this.Closed,
                ContractId = this.ContractId,
                EncryptedPassword = this.EncryptedPassword,
                Info = (this.Info ?? await this.CreateDic()).ToJson(),
                InviteBy = this.InviteBy,
                InviteFor = this.InviteFor,
                LastModified = DateTime.UtcNow.ToChinaStandardTime(),
                LoginNames = this.LoginNames.ToJson(),
                OutletCode = this.OutletCode,
                RegisterTime = this.RegisterTime,
                Salt = this.Salt,
                UserIdentifier = this.UserId.ToGuidString()
            });
        }

        private async Task<Dictionary<string, object>> CreateDic()
        {
            return await Task.FromResult(new Dictionary<string, object>());
        }

        private async Task<string> GetInviteFor(string cellphone)
        {
            using (AuthContext db = new AuthContext())
            {
                IEnumerable<ChangeLog> cellphoneList = await db.ChangeLog.Where(u => u.OldCellphone == cellphone).ToListAsync();
                return ((cellphoneList.Count() + 10) + cellphone.GetLast(10)).X10ToX36();
            }
        }

        //private async Task RaiseUserChangedLoginCellphoneEventAsync(ChangeLoginCellphone command)
        //{
        //    await this.RaiseEventAsync(new UserChangedLoginCellphone
        //    {
        //        EventId = command.CommandId,
        //        Payload = new Dictionary<string, object>
        //        {
        //            { "Command", command.ToJson() },
        //            { "Cellphone", command.NewCellphone },
        //            { "UserIdentifier", this.UserId.ToGuidString() }
        //        }
        //    });
        //}

        //[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        //private async Task RaiseUserRegisteredEventAsync(UserRegister command)
        //{
        //    await this.RaiseEventAsync(new UserRegistered
        //    {
        //        EventId = command.CommandId,
        //        Payload = new Dictionary<string, object>
        //        {
        //            { "Command", command.ToJson() },
        //            { "UserIdentifier", this.UserId.ToGuidString() }
        //        }
        //    });
        //}

        //[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        //private async Task RaiseUserResetLoginPasswordEventAsync(ResetLoginPassword command)
        //{
        //    await this.RaiseEventAsync(new UserResetLoginPassword
        //    {
        //        EventId = command.CommandId,
        //        Payload = new Dictionary<string, object>
        //        {
        //            { "Command", command.ToJson() },
        //            { "UserIdentifier", this.UserId.ToGuidString() }
        //        }
        //    });
        //}

        //[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        //private async Task RaiseUserSetLoginPasswordEventAsync(SetLoginPassword command)
        //{
        //    await this.RaiseEventAsync(new UserSetLoginPassword
        //    {
        //        EventId = command.CommandId,
        //        Payload = new Dictionary<string, object>
        //        {
        //            { "Command", command.ToJson() },
        //            { "UserIdentifier", this.UserId.ToGuidString() }
        //        }
        //    });
        //}

        private async Task SetMemoryDataFromUserModelAsync(DBUser user)
        {
            this.Cellphone = user.Cellphone;
            this.ClientType = user.ClientType;
            this.Closed = user.Closed;
            this.ContractId = user.ContractId;
            this.EncryptedPassword = user.EncryptedPassword;
            this.Info = user.Info.IsNotNullOrEmpty() ? user.Info.FromJson<Dictionary<string, object>>() : await this.CreateDic();
            this.InviteBy = user.InviteBy;
            this.InviteFor = user.InviteFor;
            this.LastModified = user.LastModified;
            this.LoginNames = user.LoginNames.FromJson<List<string>>();
            this.OutletCode = user.OutletCode;
            this.RegisterTime = user.RegisterTime;
            this.Salt = user.Salt;
            this.UserId = user.UserIdentifier.ToGuid();
        }
    }
}
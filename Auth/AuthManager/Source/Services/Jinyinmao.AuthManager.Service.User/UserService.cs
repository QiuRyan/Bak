// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : UserService.cs
// Created          : 2016-12-14  17:44
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2017-02-03  13:59
// ***********************************************************************
// <copyright file="UserService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jinyinmao.AuthManager.Domain.Core.SQLDB;
using Jinyinmao.AuthManager.Domain.Core.SQLDB.Model;
using Jinyinmao.AuthManager.Domain.Interface;
using Jinyinmao.AuthManager.Domain.Interface.Commands;
using Jinyinmao.AuthManager.Domain.Interface.Dtos;
using Jinyinmao.AuthManager.Libraries;
using Jinyinmao.AuthManager.Libraries.Extension;
using Jinyinmao.AuthManager.Libraries.Helper;
using Jinyinmao.AuthManager.Libraries.Parameter;
using Jinyinmao.AuthManager.Service.Misc;
using Jinyinmao.AuthManager.Service.Misc.Interface;
using Jinyinmao.AuthManager.Service.User.Interface;
using Jinyinmao.AuthManager.Service.User.Interface.Dtos;
using Jinyinmao.AuthManager.Service.User.Interface.Request;
using Jinyinmao.AuthManager.Service.User.Interface.Response;
using Moe.Lib;
using Moe.Lib.Jinyinmao;
using MoeLib.Diagnostics;
using MoeLib.Jinyinmao.Web;
using Newtonsoft.Json.Linq;
using Orleans;
using StackExchange.Redis;

namespace Jinyinmao.AuthManager.Service.User
{
    /// <summary>
    ///     Class UserService.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMessageManagerServices messageManagerServices = new MessageManagerServices();

        //private readonly string queryTemplate = "(AccountType eq {0}) and (((PartitionKey eq '{1}') or (RowKey eq '{2}')) and (IsAlive eq true))";
        private string JymTirisfalServiceRole
        {
            get { return App.Configurations.GetConfig<AuthApiConfig>().JYMTirisfalServiceRole; }
        }

        private IDatabase RedisDatabase
        {
            get { return RedisHelper.GetUserRedisClient(); }
        }

        #region IUserService Members

        public async Task<string> AdminCancelAccountAsync(AdminCancelAccount command)
        {
            string userIdentifier = command.UserId.ToGuidString();

            HttpClient client = JYMInternalHttpClientFactory.Create(this.JymTirisfalServiceRole, (TraceEntry)null);
            HttpResponseMessage response = await client.GetAsync("Latest/BackOffice/AdminCancelAccount/{0}".FormatWith(userIdentifier));
            try
            {
                await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                throw new ApplicationException($"交易系统注销用户异常,command:{command.UserId}");
            }

            IUser userGrain = GrainClient.GrainFactory.GetGrain<IUser>(command.UserId);
            UserInfo userInfo = await userGrain.AdminCancelAccountAsync(command);

            //注销手机号码短信发送
            await this.messageManagerServices.SendMessageAsync("1000018003", 100002, userInfo.Cellphone.Replace("X", ""));

            return userInfo.Cellphone;
        }

        public async Task AdminModifyCellphoneAsync(AdminModifyCellphoneCommand command)
        {
            HttpClient client = JYMInternalHttpClientFactory.Create(this.JymTirisfalServiceRole, (TraceEntry)null);
            HttpResponseMessage response = await client.PostAsJsonAsync("ModifyLoginCellphone", this.BuildAdminModifyCellphoneRequest(command));
            ModifyLoginCellphoneReponse reponse = await response.Content.ReadAsAsync<ModifyLoginCellphoneReponse>();
            if (reponse == null || reponse.UserIdentifier.IsNullOrEmpty())
            {
                throw new ApplicationException($"交易系统修改手机号异常,command:{command.ToJson()}");
            }

            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(command.UserId);
            await user.ResetCellphoneAsync(new ResetCellphone { NewCellphone = command.NewCellphone, Messager = "人工修改" });

            await this.messageManagerServices.SendMessageAsync("1000018004", 100002, command.NewCellphone);
        }

        public async Task<UserInfo> ChangeLoginCellphoneAsync(ChangeLoginCellphone command)
        {
            int type = AccountType.Cellphone.Code;
            Expression<Func<UserRelation, bool>> expression = u => u.IsAlive && command.LoginCellphone == u.LoginName && u.AccountType == type;

            using (AuthContext db = new AuthContext(App.Configurations.GetConfig<AuthApiConfig>().JYMAuthDBContextConnectionString))
            {
                UserRelation relation = await db.Query<UserRelation>().FirstOrDefaultAsync(expression);

                if (relation == null || relation.UserIdentifier.IsNullOrEmpty())
                {
                    throw new ApplicationException($"Cellphone not exist, {command.LoginCellphone}!");
                }

                relation.IsAlive = false;
                relation.LastModified = DateTime.UtcNow.ToChinaStandardTime();

                db.Add(new UserRelation
                {
                    AccountType = AccountType.Cellphone.Code,
                    CreateTime = DateTime.UtcNow.ToChinaStandardTime(),
                    IsAlive = true,
                    LoginName = command.NewCellphone,
                    UserIdentifier = relation.UserIdentifier
                });

                await db.ExecuteSaveChangesAsync();

                IUser user = GrainClient.GrainFactory.GetGrain<IUser>(relation.UserIdentifier.AsGuid());
                return await user.ChangeLoginCellphoneAsync(command);
            }
        }

        public async Task<CheckCellphoneResult> CheckCellphoneAsync(string cellphone)
        {
            IUserRelationGrain relationGrain = GrainClient.GrainFactory.GetGrain<IUserRelationGrain>(cellphone);
            return await relationGrain.CheckCellphoneAsync();
        }

        /// <summary>
        ///     check password as an asynchronous operation.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> CheckPasswordAsync(string userIdentifier, string password)
        {
            Guid userId = userIdentifier.AsGuid("N", Guid.Empty);
            if (userId == Guid.Empty)
            {
                return false;
            }

            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userId);
            return await user.CheckPasswordAsync(password);
        }

        public async Task<SignInResult> CheckPasswordViaCellphoneAsync(string cellphone, string password)
        {
            IUserRelationGrain relationGrain = GrainClient.GrainFactory.GetGrain<IUserRelationGrain>(cellphone);
            string userIdentifier = await relationGrain.GetUserIdentifierAsync();
            Guid userId = userIdentifier.AsGuid("N", Guid.Empty);
            if (userId == Guid.Empty)
            {
                return new SignInResult
                {
                    Cellphone = cellphone,
                    RemainCount = 1,
                    Success = false,
                    UserExist = false,
                    UserId = Guid.Empty
                };
            }
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userId);
            CheckPasswordResult result = await user.CheckPasswordAsync(cellphone, password);
            return new SignInResult
            {
                Cellphone = result.Cellphone,
                RemainCount = result.RemainCount,
                Success = result.Success,
                UserExist = result.UserExist,
                UserId = result.UserId
            };
        }

        public async Task DeleteWeChatRelationAsync(string userIdentifier)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userIdentifier.ToGuid());
            await user.UnbindWeChatAsync();
        }

        public async Task<UserInfo> GetInviterUserByInviteByAsync(string inviteBy)
        {
            using (AuthContext db = new AuthContext(App.Configurations.GetConfig<AuthApiConfig>().JYMAuthDBContextConnectionString))
            {
                DBUser user = await db.User.FirstOrDefaultAsync(p => p.InviteFor == inviteBy);
                UserInfo userInfo = null;
                if (user != null)
                {
                    userInfo = new UserInfo
                    {
                        Cellphone = user.Cellphone,
                        Closed = user.Closed,
                        ContractId = user.ContractId,
                        InviteBy = user.InviteBy,
                        InviteFor = user.InviteFor,
                        LastModified = user.LastModified,
                        OutletCode = user.OutletCode,
                        RegisterTime = user.RegisterTime,
                        UserId = user.UserIdentifier.ToGuid()
                    };
                }

                return userInfo;
            }
        }

        public async Task<string> GetOpenIdAsync(string code)
        {
            HttpClient client = JYMInternalHttpClientFactory.Create("", (TraceEntry)null);
            client.BaseAddress = new Uri("https://api.weixin.qq.com");
            HttpResponseMessage response = await client.GetAsync($@"/sns/oauth2/access_token?appid={App.Configurations.GetConfig<AuthApiConfig>().WeChatAppId}&secret={App.Configurations.GetConfig<AuthApiConfig>().WeChatAppSecret}&code={code}&grant_type=authorization_code");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JObject.Parse(content).SelectToken("openid")?.Value<string>();
            }
            throw new ApplicationException("获取OpenId失败");
        }

        public async Task<UserInfo> GetUserByCellphoneAsync(string cellphone)
        {
            IUserRelationGrain relationGrain = GrainClient.GrainFactory.GetGrain<IUserRelationGrain>(cellphone);
            string userIdentifier = await relationGrain.GetUserIdentifierAsync();
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userIdentifier.ToGuid());
            return await user.GetUserInfoAsync();
        }

        public async Task<string> GetUserIdentifierByOpenIdAsync(string openId)
        {
            IUserRelationGrain relationGrain = GrainClient.GrainFactory.GetGrain<IUserRelationGrain>(openId);
            return await relationGrain.GetUserIdentifierAsync();
        }

        public async Task<UserInfo> GetUserInfoAsync(Guid userId)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userId);
            return await user.GetUserInfoAsync();
        }

        /// <summary>
        ///     获取指定用户的信息
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="traceEntry">The trace entry.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserBizInfo> GetUserInfoFromTirisferAsync(string userIdentifier, TraceEntry traceEntry = null)
        {
            HttpClient client = JYMInternalHttpClientFactory.Create(this.JymTirisfalServiceRole, traceEntry);
            HttpResponseMessage response = await client.GetAsync("/BackOffice/UserInfo/{0}".FormatWith(userIdentifier));
            return await response.Content.ReadAsAsync<UserBizInfo>();
        }

        public async Task<BindInfo> GetWeChatBindInfoByIdAsync(string userIdentifier)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userIdentifier.ToGuid());
            return await user.GetWeChatBindInfoById();
        }

        public async Task<bool> IsValidateCredential(SetThirdAuthStep command)
        {
            List<AuthStepInfo> authSteps = this.RedisDatabase.StringGet("ModifyCellphone:" + command.UserId.ToGuidString() + ":SecondStep").ToString().FromJson<List<AuthStepInfo>>() ?? new List<AuthStepInfo>();
            AuthStepInfo lastAuthStep = authSteps.OrderByDescending(p => p.CreateTime).FirstOrDefault();
            if (lastAuthStep != null && lastAuthStep.Token == command.PreviousToken)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> IsValidateOldCellphone(SetSecondAuthStep command)
        {
            List<AuthStepInfo> authSteps = this.RedisDatabase.StringGet("ModifyCellphone:" + command.UserId.ToGuidString() + ":FirstStep").ToString().FromJson<List<AuthStepInfo>>() ?? new List<AuthStepInfo>();
            AuthStepInfo lastAuthStep = authSteps.OrderByDescending(p => p.CreateTime).FirstOrDefault();
            if (lastAuthStep != null && lastAuthStep.Token == command.PreviousToken)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<UserInfo> LockAsync(string userIdentifier)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userIdentifier.ToGuid());
            return await user.LockAsync();
        }

        public async Task<UserInfo> RegisterUserAsync(UserRegister command, TraceEntry traceEntry)
        {
            HttpClient client = JYMInternalHttpClientFactory.Create(this.JymTirisfalServiceRole, traceEntry);
            HttpResponseMessage response = await client.PostAsJsonAsync("api/CreateUser", this.BuildRequest(command));
            SignUpResponse signUpResponse = await response.Content.ReadAsAsync<SignUpResponse>();
            if (signUpResponse == null || signUpResponse.UserIdentifier.IsNullOrEmpty())
            {
                throw new ApplicationException($"交易系统插入用户异常,command:{command.ToJson()}");
            }

            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(signUpResponse.UserIdentifier.ToGuid());

            return await user.RegisterAsync(await this.BuildCommand(command, signUpResponse));
        }

        public Task<UserInfo> ResetLoginPasswordAsync(ResetLoginPassword command)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(command.UserId);
            return user.ResetLoginPasswordAsync(command);
        }

        public async Task<AuthStepInfo> SetFirstAuthStepAsync(SetFirstAuthStep command)
        {
            List<AuthStepInfo> authSteps = this.RedisDatabase.StringGet("ModifyCellphone:" + command.UserId.ToGuidString() + ":FirstStep").ToString().FromJson<List<AuthStepInfo>>() ?? new List<AuthStepInfo>();
            AuthStepInfo lastAuthStep = authSteps.OrderByDescending(p => p.CreateTime).FirstOrDefault();
            if (lastAuthStep != null && lastAuthStep.SMSToken == command.SMSToken)
            {
                return lastAuthStep;
            }

            AuthStepInfo authStepInfo = BuildFirstAuthStep(command);
            authSteps.Add(authStepInfo);

            await this.RedisDatabase.StringSetAsync("ModifyCellphone:" + command.UserId.ToGuidString() + ":FirstStep", authSteps.DateTimeJsonFormat(), TimeSpan.FromDays(60));

            return authStepInfo;
        }

        public Task<UserInfo> SetLoginPasswordAsync(SetLoginPassword command)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(command.UserId);
            return user.SetLoginPasswordAsync(command);
        }

        public async Task<AuthStepInfo> SetSecondAuthStepAsync(SetSecondAuthStep command)
        {
            List<AuthStepInfo> authSteps = this.RedisDatabase.StringGet("ModifyCellphone:" + command.UserId.ToGuidString() + ":SecondStep").ToString().FromJson<List<AuthStepInfo>>() ?? new List<AuthStepInfo>();

            AuthStepInfo authStepInfo = BuildSecondAuthStep(command);
            authSteps.Add(authStepInfo);

            await this.RedisDatabase.StringSetAsync("ModifyCellphone:" + command.UserId.ToGuidString() + ":SecondStep", authSteps.DateTimeJsonFormat(), TimeSpan.FromDays(60));

            return authStepInfo;
        }

        public async Task<AuthStepInfo> SetThirdAuthStepAsync(SetThirdAuthStep command)
        {
            HttpClient client = JYMInternalHttpClientFactory.Create(this.JymTirisfalServiceRole, (TraceEntry)null);
            HttpResponseMessage response = await client.PostAsJsonAsync("ModifyLoginCellphone", this.BuildThirdAuthStepRequest(command));
            ModifyLoginCellphoneReponse reponse = await response.Content.ReadAsAsync<ModifyLoginCellphoneReponse>();
            if (reponse == null || reponse.UserIdentifier.IsNullOrEmpty())
            {
                throw new ApplicationException($"交易系统修改手机号异常,command:{command.ToJson()}");
            }

            List<AuthStepInfo> authSteps = this.RedisDatabase.StringGet("ModifyCellphone:" + command.UserId.ToGuidString() + ":ThirdStep").ToString().FromJson<List<AuthStepInfo>>() ?? new List<AuthStepInfo>();

            AuthStepInfo authStepInfo = BuildThirdAuthStep(command);
            authSteps.Add(authStepInfo);

            await this.RedisDatabase.StringSetAsync("ModifyCellphone:" + command.UserId.ToGuidString() + ":ThirdStep", authSteps.DateTimeJsonFormat(), TimeSpan.FromDays(60));

            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(command.UserId);
            UserInfo userInfo = await user.GetUserInfoAsync();

            await user.ResetCellphoneAsync(new ResetCellphone { NewCellphone = command.NewCellphone, Messager = "自助修改" });

            await this.SendSuccessSmsToUserAsyc(command, userInfo.Cellphone);

            return authStepInfo;
        }

        public async Task<UserInfo> UnLockAsync(string userIdentifier)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(userIdentifier.ToGuid());
            return await user.UnlockAsync();
        }

        public async Task UnregisterAsync(string cellphone)
        {
            IUserRelationGrain grain = GrainClient.GrainFactory.GetGrain<IUserRelationGrain>(cellphone);
            await grain.UnregisterAsync();
        }

        public async Task WeChatBindAsync(WeChatBind command)
        {
            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(command.UserIdentifier.ToGuid());
            await user.BindWeChatAsync(command.OpenId);
        }

        public async Task<UserInfo> WeChatSignUpAsync(WeChatRegister command)
        {
            HttpClient client = JYMInternalHttpClientFactory.Create(this.JymTirisfalServiceRole, (TraceEntry)null);
            HttpResponseMessage response = await client.PostAsJsonAsync("api/CreateUser", this.BuildRequest(command));
            SignUpResponse signUpResponse = await response.Content.ReadAsAsync<SignUpResponse>();
            if (signUpResponse == null || signUpResponse.UserIdentifier.IsNullOrEmpty())
            {
                throw new ApplicationException($"交易系统插入用户异常,command:{command.ToJson()}，token:{command.Token}");
            }

            IUser user = GrainClient.GrainFactory.GetGrain<IUser>(signUpResponse.UserIdentifier.ToGuid());
            await user.RegisterAsync(await this.BuildCommand(command, signUpResponse));
            IUserRelationGrain relationGrain = GrainClient.GrainFactory.GetGrain<IUserRelationGrain>(command.OpenId);
            await relationGrain.BindWeChatAsync(command.UserId.ToGuidString());
            return await user.GetUserInfoAsync();
        }

        #endregion IUserService Members

        private static AuthStepInfo BuildFirstAuthStep(SetFirstAuthStep command)
        {
            return new AuthStepInfo
            {
                CreateTime = DateTime.UtcNow.ToChinaStandardTime(),
                SMSToken = command.SMSToken,
                Token = command.Token,
                ValidateMessage = command.ValidateMessage,
                UserIdentifier = command.UserId.ToGuidString()
            };
        }

        private static AuthStepInfo BuildSecondAuthStep(SetSecondAuthStep command)
        {
            return new AuthStepInfo
            {
                CreateTime = DateTime.UtcNow.ToChinaStandardTime(),
                PreviousToken = command.PreviousToken,
                ValidateMessage = command.ValidateMessage,
                Token = command.Token,
                UserIdentifier = command.UserId.ToGuidString()
            };
        }

        private static AuthStepInfo BuildThirdAuthStep(SetThirdAuthStep command)
        {
            return new AuthStepInfo
            {
                CreateTime = DateTime.UtcNow.ToChinaStandardTime(),
                Token = command.Token,
                ValidateMessage = command.ValidateMessage,
                SMSToken = command.SMSToken,
                PreviousToken = command.PreviousToken,
                UserIdentifier = command.UserId.ToGuidString()
            };
        }

        private ModifyLoginCellphoneRequest BuildAdminModifyCellphoneRequest(AdminModifyCellphoneCommand command)
        {
            return new ModifyLoginCellphoneRequest
            {
                Cellphone = command.NewCellphone,
                UserIdentifier = command.UserId.ToGuidString()
            };
        }

        private async Task<UserRegister> BuildCommand(UserRegister command, SignUpResponse signUpResponse)
        {
            return await Task.FromResult(new UserRegister
            {
                CommandId = command.CommandId,
                EntityId = signUpResponse.UserIdentifier.AsGuid(),
                Cellphone = command.Cellphone,
                ClientType = command.ClientType,
                ContractId = command.ContractId,
                Info = command.Info,
                InviteBy = command.InviteBy,
                InviteFor = "",
                OutletCode = command.OutletCode,
                Password = command.Password,
                UserId = signUpResponse.UserIdentifier.AsGuid()
            });
        }

        private async Task<UserRegister> BuildCommand(WeChatRegister command, SignUpResponse signUpResponse)
        {
            return await Task.FromResult(new UserRegister
            {
                EntityId = signUpResponse.UserIdentifier.AsGuid(),
                Cellphone = command.Cellphone,
                ClientType = command.ClientType,
                ContractId = command.ContractId,
                Info = command.Info,
                InviteBy = command.InviteBy,
                InviteFor = "",
                OutletCode = command.OutletCode,
                Password = command.Password,
                UserId = signUpResponse.UserIdentifier.AsGuid()
            });
        }

        private CreateUserRequest BuildRequest(WeChatRegister command)
        {
            return new CreateUserRequest
            {
                Cellphone = command.Cellphone,
                ClientType = command.ClientType,
                ContractId = command.ContractId,
                InviteBy = command.InviteBy,
                OutletCode = command.OutletCode,
                Password = GuidUtility.GuidShortCode().GetFirst(6)
            };
        }

        private CreateUserRequest BuildRequest(UserRegister command)
        {
            return new CreateUserRequest
            {
                Cellphone = command.Cellphone,
                ClientType = command.ClientType,
                ContractId = command.ContractId,
                InviteBy = command.InviteBy,
                OutletCode = command.OutletCode,
                Password = command.Password
                //UserIdentifier = command.UserId.ToGuidString()
            };
        }

        private ModifyLoginCellphoneRequest BuildThirdAuthStepRequest(SetThirdAuthStep command)
        {
            return new ModifyLoginCellphoneRequest
            {
                Cellphone = command.NewCellphone,
                UserIdentifier = command.UserId.ToGuidString()
            };
        }

        private async Task<UserRelation> BuildUserRelationAsync(ChangeLoginCellphone command, UserRelation relation)
        {
            return await Task.FromResult(new UserRelation
            {
                AccountType = AccountType.Cellphone.Code,
                CreateTime = DateTime.UtcNow.ToChinaStandardTime(),
                IsAlive = true,
                LoginName = command.NewCellphone,
                UserIdentifier = relation.UserIdentifier
            });
        }

        private async Task SendSuccessSmsToUserAsyc(SetThirdAuthStep command, string oldCellphone)
        {
            //新手机号码
            await this.messageManagerServices.SendMessageAsync("1000018003", 100002, command.NewCellphone);

            //老手机号码
            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                { "Cellphone", new Regex("(\\d{3})(\\d{4})(\\d{4})", RegexOptions.None).Replace(command.NewCellphone, "$1****$3") }
            };

            await this.messageManagerServices.SendMessageAsync("1000018002", 100002, oldCellphone, dic);
        }
    }
}
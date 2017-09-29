using Jinyinmao.AuthManager.Domain.Core.Models;
using Jinyinmao.AuthManager.Service.User;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain;

namespace DataTransfer
{
    internal class Program
    {
        private static async Task<bool> CheckIfInRedis(string userId, string referenceKey)
        {
            return await RedisHelper.GetActivityRedisClientById(userId).CheckIfKeyExist(referenceKey);
        }

        //private static async Task<List<string>> GetAllId()
        //{
        //    using (JYMDBContext context = new JYMDBContext())
        //    {
        //        try
        //        {
        //            return await context.Users.AsNoTracking().Where(u => u.EncryptedPassword == string.Empty).Select(u => u.UserIdentifier).ToListAsync();
        //        }
        //        catch (Exception x)
        //        {
        //            Console.WriteLine(x.GetExceptionString());
        //        }
        //        return null;
        //    }
        //}

        //private static async Task<Dictionary<string, bool>> GetAllUserIdsFromSiloTable(int grainIndex, int tableIndex)
        //{
        //    IEnumerable<string> list = await SqlDatabaseHelper.GetTableUserIds(grainIndex, tableIndex);
        //    IList<string> enumerable = list as IList<string> ?? list.ToList();
        //    return enumerable.IsNullOrEmpty() ? new Dictionary<string, bool>() : enumerable.ToDictionary(s => s, s => true);
        //}

        //private static List<string> GetIds()
        //{
        //    string path = Directory.GetCurrentDirectory() + "/id.txt";
        //    return File.ReadAllLines(path).ToList();
        //}

        //private static async Task<List<string>> GetIdsByRegisterTime()
        //{
        //    using (JYMDBContext context = new JYMDBContext())
        //    {
        //        return await context.Users.AsNoTracking().Where(u => u.RegisterTime == DateTime.MinValue).Select(u => u.UserIdentifier).ToListAsync();
        //    }
        //}

        private static async Task<List<string>> GetListId()
        {
            using (JYMDBContext context = new JYMDBContext())
            {
                return await context.Users.Where(u => u.RegisterTime <= DateTime.MinValue).Select(u => u.UserIdentifier).ToListAsync();
            }
        }

        private static async Task<UserState> GetUserDataFromRedisOrSiloDB(string id, string referenceKey)
        {
            UserState state = await RedisHelper.GetActivityRedisClientById(id).ReadDataFromCacheAsync<UserState>(referenceKey);
            if (state == null || state.UserId == Guid.Empty)
            {
                state = await SqlDatabaseHelper.ReadStateAsync(id);
            }
            return state;
        }

        private static void Main(string[] args)
        {
            //Transfer();
            Console.WriteLine("Start update registerTime");
            try
            {
                UpdateReisterTime().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetExceptionString());
            }

            Console.WriteLine("End update registerTime");
            Console.ReadKey();
        }

        private static async Task<bool> SaveDataToCloudTable(UserState state)
        {
            await AzureStorageHelper.UserCloudTable.ExecuteAsync(TableOperation.InsertOrMerge(new UserEntity
            {
                PartitionKey = state.UserId.ToGuidString(),
                RowKey = state.Cellphone,
                AccountType = 10,
                CreateTime = DateTime.UtcNow.ToChinaStandardTime(),
                IsAlive = true
            }));
            return true;
        }

        //private static void Transfer()
        //{
        //    //Console.WriteLine("input grain index:");
        //    //int grainIndex = Console.ReadLine().AsInt();

        //    //Console.WriteLine("input table index:");
        //    //int tableIndex = Console.ReadLine().AsInt();

        //    //Dictionary<string, bool> tableUsers = await GetAllUserIdsFromSiloTable(grainIndex, tableIndex);

        //    //List<string> allNotReadyUser = await GetAllId();

        //    //List<string> dealIds = tableUsers.Keys.Where(t => allNotReadyUser.Contains(t)).ToList();
        //    List<string> dealIds = GetIds();

        //    Console.WriteLine("to do ids count: " + dealIds.Count);

        //    if (dealIds.IsNullOrEmpty())
        //    {
        //        Console.WriteLine("all done...");
        //        return;
        //    }
        //    Parallel.ForEach(Partitioner.Create(0, dealIds.Count), async range =>
        //    {
        //        using (JYMDBContext context = new JYMDBContext())
        //        {
        //            for (int i = range.Item1; i < range.Item2; i++)
        //            {
        //                string id = dealIds[i];
        //                try
        //                {
        //                    string referenceKey = await SqlDatabaseHelper.ReadUserKeyAsync(id);
        //                    UserState data;
        //                    if (await CheckIfInRedis(id, referenceKey))
        //                    {
        //                        data = await GetUserDataFromRedisOrSiloDB(id, referenceKey);
        //                    }
        //                    else
        //                    {
        //                        data = await SqlDatabaseHelper.ReadStateAsync(id);
        //                    }

        //                    if (await SaveDataToCloudTable(data))
        //                    {
        //                        User user = new User
        //                        {
        //                            Cellphone = data.Cellphone,
        //                            ClientType = data.ClientType,
        //                            Closed = data.Closed,
        //                            ContractId = data.ContractId,
        //                            EncryptedPassword = data.EncryptedPassword,
        //                            LoginNames = data.LoginNames.ToJson(),
        //                            OutletCode = data.OutletCode,
        //                            RegisterTime = data.RegisterTime.ToChinaStandardTime().AddHours(-8),
        //                            InviteBy = "",
        //                            InviteFor = data.Cellphone.ToX36String(),
        //                            UserIdentifier = data.UserId.ToGuidString(),
        //                            Salt = data.Salt,
        //                            Info = "{}",
        //                            LastModified = data.LastModified ?? DateTime.UtcNow.ToChinaStandardTime()
        //                        };
        //                        DbEntityEntry<User> entry = context.Entry(user);
        //                        entry.State = EntityState.Modified;
        //                        entry.Property(t => t.Id).IsModified = false;
        //                        //entry.Property(t => t.UserIdentifier).IsModified = false;
        //                        //entry.Property(t => t.EncryptedPassword).IsModified = true;
        //                        //entry.Property(t => t.LastModified).IsModified = true;
        //                        await context.SaveChangesAsync();
        //                    }
        //                }
        //                catch (DbEntityValidationException e)
        //                {
        //                    Console.WriteLine(e.GetExceptionString());
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.GetExceptionString());
        //                }
        //            }
        //        }
        //    });
        //}

        [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
        private static async Task UpdateReisterTime()
        {
            List<string> list = await GetListId();
            Parallel.ForEach(Partitioner.Create(0, list.Count), async range =>
            {
                using (JYMDBContext context = new JYMDBContext())
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        try
                        {
                            User user = await context.Users.FirstOrDefaultAsync(u => u.UserIdentifier == list[i]);
                            string referenceKey = await SqlDatabaseHelper.ReadUserKeyAsync(user.UserIdentifier);
                            UserState data;
                            if (await CheckIfInRedis(user.UserIdentifier, referenceKey))
                            {
                                data = await GetUserDataFromRedisOrSiloDB(user.UserIdentifier, referenceKey);
                            }
                            else
                            {
                                GrainStateRecord record = await BlobHelper.ReadStateToBlob(user.UserIdentifier);
                                data = JsonConvert.DeserializeObject<UserState>(record.Data);
                            }
                            user.RegisterTime = data.RegisterTime;
                            await context.SaveChangesAsync();
                            Console.WriteLine("Update RegisterTime,UserIdentifier:" + user.UserIdentifier);
                        }
                        catch (DbEntityValidationException e)
                        {
                            Console.WriteLine(e.GetExceptionString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.GetExceptionString());
                        }
                    }
                }
            });
        }
    }
}
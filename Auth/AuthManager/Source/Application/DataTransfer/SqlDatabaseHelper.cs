using DataTransfer;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SqlMapper = Dapper.SqlMapper;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class SqlDatabaseProvider.
    /// </summary>
    public class SqlDatabaseHelper
    {
        /// <summary>
        ///     The dbcount
        /// </summary>
        private const int DBCOUNT = 6;

        /// <summary>
        ///     The tablecount
        /// </summary>
        private const int TABLECOUNT = 9;

        /// <summary>
        ///     The connection string template
        /// </summary>
        private static readonly string connectionStringTemplate;

        /// <summary>
        ///     The get all user command string
        /// </summary>
        private static readonly string GetAllUserCommandString;

        /// <summary>
        ///     The get key command string
        /// </summary>
        private static readonly string GetKeyCommandString;

        /// <summary>
        ///     The letters
        /// </summary>
        private static readonly string Letters;

        /// <summary>
        ///     The retry policy
        /// </summary>
        private static readonly RetryPolicy retryPolicy;

        /// <summary>
        ///     The select command string
        /// </summary>
        private static readonly string SelectCommandString;

        /// <summary>
        ///     The table name template
        /// </summary>
        private static readonly string tableNameTemplate;

        /// <summary>
        ///     Initializes static members of the <see cref="SqlDatabaseHelper" /> class.
        /// </summary>
        static SqlDatabaseHelper()
        {
            Letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            SelectCommandString = "SELECT TOP 1 Data FROM [dbo].[{0}] WHERE [Id] = @Id ORDER BY [TimeStamp] DESC";
            GetKeyCommandString = "SELECT TOP 1 [Key] FROM [dbo].[{0}] WHERE [Id] = @Id";
            GetAllUserCommandString = "SELECT DISTINCT Id FROM [dbo].[{0}] WHERE Type='Yuyi.Jinyinmao.Domain.User'";
            tableNameTemplate = "Grains_{0}";
            connectionStringTemplate = ConfigurationManager.AppSettings.Get("SqlStorageProviderConnectionString");
            retryPolicy = RetryPolicyHelper.SqlDatabase;
        }

        /// <summary>
        ///     Gets the table user ids.
        /// </summary>
        /// <param name="grainIndex">Index of the grain.</param>
        /// <param name="tableIndex">Index of the table.</param>
        /// <returns>Task&lt;IEnumerable&lt;System.String&gt;&gt;.</returns>
        public static async Task<IEnumerable<string>> GetTableUserIds(int grainIndex, int tableIndex)
        {
            string connectionString = connectionStringTemplate.FormatWith(grainIndex);
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    IEnumerable<string> data = await retryPolicy.ExecuteAsync(async () =>
                            await SqlMapper.QueryAsync<string>(db, GetAllUserCommandString.FormatWith(tableNameTemplate.FormatWith(tableIndex))));
                    return data;
                }
                catch (DbEntityValidationException ex)
                {
                    Console.WriteLine(ex.GetExceptionString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetExceptionString());
                }
                return null;
            }
        }

        /// <summary>
        ///     read state as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;UserState&gt;.</returns>
        public static async Task<UserState> ReadStateAsync(string id)
        {
            string connectionString = GetConnectionString(id.AsGuid().ToString());
            string tableName = GetTableName(id.AsGuid().ToString());
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    IEnumerable<string> data = await retryPolicy.ExecuteAsync(async () =>
                        await SqlMapper.QueryAsync<string>(db, SelectCommandString.FormatWith(tableName),
                            new { Id = id }));
                    return JsonConvert.DeserializeObject<UserState>(data.FirstOrDefault());
                }
                catch (DbEntityValidationException ex)
                {
                    Console.WriteLine(ex.GetExceptionString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetExceptionString());
                }
                return null;
            }
        }

        /// <summary>
        ///     read user key as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public static async Task<string> ReadUserKeyAsync(string id)
        {
            string connectionString = GetConnectionString(id);
            string tableName = GetTableName(id.AsGuid().ToString());
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    IEnumerable<string> data = await retryPolicy.ExecuteAsync(async () =>
                        await SqlMapper.QueryAsync<string>(db, GetKeyCommandString.FormatWith(tableName),
                            new { Id = id }));
                    return data.FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    Console.WriteLine(ex.GetExceptionString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetExceptionString());
                }
                return null;
            }
        }

        /// <summary>
        ///     Gets the connection string.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.String.</returns>
        private static string GetConnectionString(string id)
        {
            char lastLetter = char.ToUpperInvariant(id.LastOrDefault());
            return connectionStringTemplate.FormatWith(Letters.IndexOf(lastLetter) % DBCOUNT);
        }

        /// <summary>
        ///     Gets the name of the table.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.String.</returns>
        private static string GetTableName(string id)
        {
            char penultimateLetter = char.ToUpperInvariant(id.Substring(0, id.Length - 1).LastOrDefault());
            return tableNameTemplate.FormatWith(Letters.IndexOf(penultimateLetter) % TABLECOUNT);
        }
    }
}